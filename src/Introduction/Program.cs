using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath;
            if (OperatingSystem.isMacOS())
            {
                rootPath = @"/Users/julien/Downloads";
            }
            else
            {
                rootPath = @"C:\Users\20012454\Downloads";
            }
            ShowLargeFilesWithoutLinq(rootPath);
            Console.WriteLine("**********");
            ShowLargeFilesWithLinq(rootPath);
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            // var query = from file in new DirectoryInfo(path).GetFiles()
            //             orderby file.Length descending
            //             select file;
            var query = new DirectoryInfo(path).GetFiles()
                            .OrderByDescending(x => x.Length)
                            .Take(5);
            foreach (var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name.PadRight(50)} : {file.Length,15:N0}");
            }
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            Array.Sort(files, new FileInfoComparer());
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{files[i].Name.PadRight(50)} : {files[i].Length, 15:N0}");
            }
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}
