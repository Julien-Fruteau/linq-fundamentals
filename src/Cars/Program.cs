using System.Globalization;
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

            var query = from car in cars
                        join manufacturer in manufacturers
                            on car.Manufacturer equals manufacturer.Name
                        orderby car.Combined descending, car.Name
                        select new {
                            car.Manufacturer,
                            manufacturer.Headquarters,
                            car.Name,
                            car.Combined
                        };
            
            var q2 = cars.Join(manufacturers, 
                                c => c.Manufacturer, 
                                m => m.Name,
                                (c, m) => new
                                {
                                    c.Manufacturer,
                                    m.Headquarters,
                                    c.Name,
                                    c.Combined
                                })
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

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

            foreach (var q in q2.Take(10))
            {
                System.Console.WriteLine($"{q.Manufacturer.PadRight(10)} {q.Name.PadRight(30)} : {q.Combined}");
            }

            // var anyFord = cars.Any(c => c.Manufacturer == "Ford");
            // var allFord = cars.All(c => c.Manufacturer == "Ford");
            // System.Console.WriteLine($"Any Ford : {anyFord} Vs All are Ford : {allFord}");

            // var characters = query.SelectMany(c => c.Name).OrderBy(c => c);
            // foreach (var c in characters)
            // {
            //     System.Console.WriteLine(c);
            // }
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
