using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("src/Cars/fuel.csv");

            var query = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name
                        select car;
            var last = query.LastOrDefault();

            var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .First();

            System.Console.WriteLine($"top : {top.Name} Vs last : {last.Name}");

            foreach (var car in query.Take(10))
            {
                System.Console.WriteLine($"{car.Manufacturer.PadRight(10)} {car.Name.PadRight(30)} : {car.Combined}");
            }

            var anyFord = cars.Any(c => c.Manufacturer == "Ford");
            var allFord = cars.All(c => c.Manufacturer == "Ford");
            System.Console.WriteLine($"Any Ford : {anyFord} Vs All are Ford : {allFord}");
        }

        private static List<Car> ProcessFile(string path)
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
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
