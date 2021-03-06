﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight",   Rating = 8.9f, Year = 2008 },
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Casablanca",        Rating = 8.5f, Year = 1942 },
                new Movie { Title = "Star Wars V",       Rating = 8.7f, Year = 1980 }
            };

            var query = movies.Where(m => m.Year > 2000)
                              .OrderByDescending(m => m.Rating);
            foreach (var movie in query)
            {
                Console.WriteLine(movie.Title);
            }

            Console.WriteLine("*************");

            var query2 = movies.Filter(m => m.Year > 2000);
            var enumerator = query2.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

            Console.WriteLine("*************");

            var q3 = MyLinq.Random().Where(x => x > 0.5).Take(10);
            foreach (var i in q3)
            {
                System.Console.WriteLine(i);
            }
        }
    }
}
