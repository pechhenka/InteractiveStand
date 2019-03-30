using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Stand
{
    public class Stock : Singleton<Stock>
    {
        public List<string[]> TimetableMatrix = new List<string[]>();
        public List<string[]> LessonScheduleMatrix = new List<string[]>();
        public List<string[]> ExtraClassesMatrix = new List<string[]>();

        void Awake()
        {
            try
            {
                string ManifestPath = Application.dataPath + @"\Manifest.txt";
                StreamReader ManifestReader = new StreamReader(ManifestPath);

                TimetableMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
                LessonScheduleMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
                ExtraClassesMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
                ApplicationController.Instance.Downtime = int.Parse(ManifestReader.ReadLine());
                Loger.CompleteInitialized<Stock>();
            }
            catch (Exception e)
            {
                Loger.ErrorInitialized<Stock>(e);
            }
        }
    }
}