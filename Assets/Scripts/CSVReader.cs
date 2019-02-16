﻿using System.Collections.Generic;
using System.IO;
using System;

namespace Stand
{
    public static class CSVReader
    {
        public static List<string[]> Read(string path)
        {
            List<string[]> Matrix = new List<string[]>();

            StreamReader reader = new StreamReader(path);
            string[] separator = { ('"'.ToString() + ','.ToString() + '"'.ToString()), (','.ToString()) };

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                line = line.Substring(1, line.Length - 2);

                Matrix.Add(line.Split(separator, StringSplitOptions.None));
            }

            return Matrix;
        }
    }
}