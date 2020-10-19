﻿using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config/dbSettings.json");
            var config = builder.Build();
            // System.Console.WriteLine(config["ConnectionString"]);
            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData(config["ConnectionString"]);
            QueryData(config["ConnectionString"]);
        }

        private static void QueryData(string ConnectionString)
        {
            var db = new CarDb(ConnectionString);
            db.Database.Log = System.Console.WriteLine;

           var query =
                from car in db.Cars
                group car by car.Manufacturer.ToUpper() into manufacturer
                select new {
                    Name = manufacturer.Key,
                    Cars =
                        (from car in manufacturer
                        orderby car.Combined descending
                        select car).Take(2)
                };

            foreach (var manufacturer in query)
            {
                System.Console.WriteLine(manufacturer.Name);
                foreach (var car in manufacturer.Cars)
                {
                    System.Console.WriteLine($"\t{car.Name}: {car.Combined}");
                }
            }
            
        }

        private static void InsertData(string ConnectionString)
        {
            var cars = ProcessCars("src/Cars/fuel.csv");
            // ConnectionString "Server=localhost;Database=CarDb;User Id=sa;Password=<PWD>;"
            var db = new CarDb(ConnectionString);
            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);   // does not insert
                }
                db.SaveChanges();       // will insert lines in table
            }
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("src/Cars/fuel.xml");
            var query =
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car")
                                                            ?? Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;
            foreach (var name in query)
            {
                System.Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("src/Cars/fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars",
                    from record in records
                    select new XElement(ex + "Car",
                            new XAttribute("Name", record.Name),
                            new XAttribute("Combined", record.Combined),
                            new XAttribute("Manufacturer", record.Manufacturer)));
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
            document.Add(cars);
            document.Save("src/Cars/fuel.xml");
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Where(l => l.Length > 1)
                            .Select(l =>
                                {
                                    var columns = l.Split(',');
                                    return new Manufacturer
                                    {
                                        Name = columns[0],
                                        Headquarters = columns[1],
                                        Year = int.Parse(columns[2])
                                    };
                                });
                return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .ToCar();

            return query.ToList();
        }
    }


    public class CarStatistics
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public double Avg { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }

        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        public CarStatistics Accumulate(Car c)
        {
            Total += c.Combined;
            Count += 1;
            Max = Math.Max(Max, c.Combined);
            Min = Math.Min(Min, c.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Avg = Total / Count;
            return this;
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3], CultureInfo.InvariantCulture),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
