using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Stand
{
    public class Data : Singleton<Data>
    {
        public List<string[]> TimetableMatrix = new List<string[]>();
        public List<string[]> LessonScheduleMatrix = new List<string[]>();
        public List<string[]> ExtraClassesMatrix = new List<string[]>();
        public int Downtime = 0;

        public void Load()
        {

            string ManifestPath = Application.dataPath + @"\Manifest.txt";
            StreamReader ManifestReader = new StreamReader(ManifestPath);

            TimetableMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
            LessonScheduleMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
            ExtraClassesMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
            Downtime = int.Parse(ManifestReader.ReadLine());

        }
    }
}