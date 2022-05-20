using System;
using System.IO;

namespace sortPhotos
{
    internal class Program
    {
        private static string workingDir;

        static void Main(string[] args)
        {
            if (args.Length == 0 || !Directory.Exists(args[0]))
            {
                workingDir = Environment.CurrentDirectory;
            }
            else
            {
                workingDir = args[0];
            }

            Console.WriteLine($"Working directory is ''{workingDir}");

            PhotoSorter.SortPhotosInFolder(workingDir);
        }
    }
}
