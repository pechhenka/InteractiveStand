using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Stand
{
    /* Хранит в себе загруженные расписание из таблиц *.csv
     * В виде листов массивов строк - двумерных массивов
     */
    public class Data : Singleton<Data>
    {
        public List<string[]> TimetableMatrix = new List<string[]>();
        public List<string[]> LessonScheduleMatrix = new List<string[]>();
        public List<string[]> ExtraClassesMatrix = new List<string[]>();
        public string DataPath = "";

        void Awake()
        {
            try
            {
                DataPath = Application.dataPath + @"\Data\";
                string ManifestPath = DataPath + "Manifest.txt";
                StreamReader ManifestReader = new StreamReader(ManifestPath);

                TimetableMatrix = CSVReader.Read(DataPath + ManifestReader.ReadLine());
                LessonScheduleMatrix = CSVReader.Read(DataPath + ManifestReader.ReadLine());
                ExtraClassesMatrix = CSVReader.Read(DataPath + ManifestReader.ReadLine());
                ApplicationController.Instance.Downtime = int.Parse(ManifestReader.ReadLine());
                Loger.CompleteInitialized<Data>();
            }
            catch (Exception e)
            {
                Loger.ErrorInitialized<Data>(e);
            }
        }
    }
}