﻿using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("src/Cars/fuel.csv");
            var manufacturers = ProcessManufacturers("src/Cars/manufacturers.csv");

            var group = from car in cars
                        group car by car.Manufacturer.ToUpper() into manufacturer
                        orderby manufacturer.Key
                        select manufacturer;

            var g2 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                         .OrderBy(g => g.Key);

            foreach (var g in g2)
            {
                System.Console.WriteLine($"{g.Key} : {g.Count()} cars");
                foreach (var car in g.OrderByDescending(c => c.Combined).Take(2))
                {
                    System.Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

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
