using System;
using System.IO;

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
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();

            foreach (var file in files)
            {
                Console.WriteLine($"{file.Name}, {file.Length}");
            }
        }
    }
}
