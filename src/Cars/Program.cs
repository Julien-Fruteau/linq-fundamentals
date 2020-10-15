﻿using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("src/Cars/fuel.csv");
            var manufacturers = ProcessManufacturers("src/Cars/manufacturers.csv");

            var query =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)
                } into result
                orderby result.Max descending
                select result;

            var q2 =
                cars.GroupBy(c => c.Manufacturer)
                    .Select(g =>
                        {
                            var results = g.Aggregate(new CarStatistics(),
                                                      (acc, c) => acc.Accumulate(c),
                                                      acc => acc.Compute());
                            return new
                            {
                                Name = g.Key,
                                Avg = results.Avg,
                                Min = results.Min,
                                Max = results.Max
                            };
                        })
                    .OrderByDescending( r => r.Max);

            foreach (var result in query)
            {
                System.Console.WriteLine(result.Name);
                System.Console.WriteLine($"\tMax : {result.Max}");
                System.Console.WriteLine($"\tMin : {result.Min}");
                System.Console.WriteLine($"\tAvg : {result.Avg}");
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
