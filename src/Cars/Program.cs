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
            foreach (var car in cars)
            {
                System.Console.WriteLine(car.Name);
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
