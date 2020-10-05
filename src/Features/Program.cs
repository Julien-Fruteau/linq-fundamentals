using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
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
