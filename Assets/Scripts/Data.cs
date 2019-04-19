using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    /* Хранит в себе загружаемые данные
     */
    public class Data : Singleton<Data>
    {
        public List<string[]> CallsMatrix = new List<string[]>();
        public List<string[]> LessonsMatrix = new List<string[]>();
        public List<string[]> ExtraMatrix = new List<string[]>();
        public string DataPath = "";

        public Manifest LocalManifest = new Manifest();
        public Manifest OutsideManifest = new Manifest();
        public Manifest CurrentManifest = new Manifest();

        void Awake()
        {
            try
            {
                
                DataPath = Application.dataPath + @"\Data\";

                LocalManifest.Load(DataPath + "Manifest.txt");

                CallsMatrix = CSVReader.Read(DataPath + LocalManifest.NameCallsMatrix);
                LessonsMatrix = CSVReader.Read(DataPath + LocalManifest.NameLessonsMatrix);
                ExtraMatrix = CSVReader.Read(DataPath + LocalManifest.NameExtraMatrix);
                ApplicationController.Instance.DownTime = LocalManifest.DownTime;

                Loger.CompleteInitialized<Data>();
            }
            catch (Exception e)
            {
                Loger.ErrorInitialized<Data>(e);
            }
        }
    }
}