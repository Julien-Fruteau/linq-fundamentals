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
        }

        private static List<Car> ProcessFile(string path)
        {
            var query = from line in File.ReadAllLines(path).Skip(1)
                        where line.Length > 1
                        select Car.ParseFromCsv(line);
            return query.ToList();
        }
    }
}
