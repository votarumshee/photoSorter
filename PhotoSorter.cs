using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sortPhotos
{
    internal class PhotoSorter
    {
        private const char defaultSeparateChar = '_';
        private const int YearMonthDayStringLen = 8;
        private static string workingDir;
        private static char separateChar = defaultSeparateChar;
        private static bool needLog;

        private static object GetMonthFormatted(int month)
        {
            if (month < 10)
                return $"0{month}";
            else
                return month.ToString();
        }

        private static void CreateDirectoryIfItDoesntExist(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Log($"Trying to create directory '{dirName}'...");
                Directory.CreateDirectory(dirName);
                Log($"Directory '{dirName}' was created successfully");
            }
        }

        private static void Log(string message)
        {
            if (needLog)
            {
                Console.WriteLine(message);
            }
        }

        public static void SortPhotosInFolder(string folder, bool isNeedToLog = true)
        {
            workingDir = folder;
            needLog = isNeedToLog;

            string[] fileNames = Directory.GetFiles(workingDir, "*.*");

            bool wrongFilesFormat = true;

            foreach (string currentFileName in fileNames)
            {
                Log($"File {currentFileName} is processing...");

                int firstIndexOf_ = currentFileName.IndexOf(separateChar);

                if (firstIndexOf_ >= 0)
                {
                    int secondIndexOf_ = currentFileName.IndexOf(separateChar, firstIndexOf_ + 1);

                    if (secondIndexOf_ >= 0 && secondIndexOf_ - firstIndexOf_ - 1 == YearMonthDayStringLen)
                    {
                        string datePart = currentFileName.Substring(firstIndexOf_ + 1, YearMonthDayStringLen);

                        DateTime date;
                        if (DateTime.TryParse($"{datePart.Substring(0, 4)}.{datePart.Substring(4, 2)}.{datePart.Substring(6, 2)}", out date))
                        {
                            wrongFilesFormat = false;
                            string yearDir = $@"{workingDir}\{date.Year}";
                            string monthDir = $@"{workingDir}\{date.Year}\{GetMonthFormatted(date.Month)}";
                            CreateDirectoryIfItDoesntExist(yearDir);
                            CreateDirectoryIfItDoesntExist(monthDir);

                            string filePath = Path.GetDirectoryName(currentFileName);
                            string fileName = Path.GetFileName(currentFileName);
                            string newFilePath = monthDir;
                            File.Move($@"{filePath}\{fileName}", $@"{newFilePath}\{fileName}");
                        }
                    }
                }

            }

            if (wrongFilesFormat)
            {
                Console.WriteLine("No one file was processed.");
            }
            else
            {
                Console.WriteLine("All done!");
            }
        }
    }
}
