﻿using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var records = ProcessCars("src/Cars/fuel.csv");
            var document = new XDocument();
            var cars = new XElement("Cars");
            foreach (var record in records)
            {
                var car = new XElement("Car");
                var name = new XAttribute("Name", record.Name);
                var combined = new XAttribute("Combined", record.Combined);
                car.Add(name);
                car.Add(combined);
                cars.Add(car);
            }
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
