using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Data : Singleton<Data>
{
    public List<string[]> TimetableMatrix = new List<string[]>();
    public List<string[]> LessonScheduleMatrix = new List<string[]>();
    public List<string[]> ExtraClassesMatrix = new List<string[]>();

    public void Load()
    {

        string ManifestPath = Application.dataPath + @"\Manifest.txt";
        StreamReader ManifestReader = new StreamReader(ManifestPath, encoding: Encoding.GetEncoding(1251));

        TimetableMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
        LessonScheduleMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());
        ExtraClassesMatrix = CSVReader.Read(Application.dataPath + @"\" + ManifestReader.ReadLine());

    }
}