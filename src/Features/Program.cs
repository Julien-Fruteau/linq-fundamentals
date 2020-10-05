using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            // Func indicates lambda definition
            // takes an int and return an int
            Func<int, int> square = x => x * x;
            Console.WriteLine(square(3));

            Console.WriteLine("**********");
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine(add(3, 4));

            Console.WriteLine("**********");
            Func<int, int, int> add2 = (x, y) => 
            {
                int temp = x + y;
                return temp;
            };
            Console.WriteLine("**********");

            // returns void
            Action<int> write = x => Console.WriteLine(x);
            write(999);
            Console.WriteLine("**********");

            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Julien"},
                new Employee {Id = 2, Name = "Bibi"}
            };

            Console.WriteLine(developers.MyCount());
            Console.WriteLine("**********");
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Lulu"}
            };

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
            Console.WriteLine("**********");
            foreach (var Employee in developers.Where(e => e.Name.StartsWith('B')))
            {
                Console.WriteLine(Employee.Name);
            }
        }
    }
}
