using System;
using System.Collections.Generic;
using UnityEngine;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

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

        public Manifest CurrentManifest = new Manifest();

        private Manifest LocalManifest = new Manifest();
        private Manifest OutsideManifest = new Manifest();

        void Awake()
        {
            try
            {
                DataPath = Application.dataPath + @"\LocalData\";

                LocalManifest.Load(DataPath + "Manifest.txt");
                CurrentManifest = LocalManifest;

                if (LocalManifest.PathOutsideData != "")
                {
                    if (OutsideManifest.Load(LocalManifest.PathOutsideData + "Manifest.txt"))
                    {
                        CurrentManifest = OutsideManifest;
                    }
                }

                CallsMatrix = CSVReader.Read(DataPath + CurrentManifest.NameCallsMatrix);
                LessonsMatrix = CSVReader.Read(DataPath + CurrentManifest.NameLessonsMatrix);
                ExtraMatrix = CSVReader.Read(DataPath + CurrentManifest.NameExtraMatrix);
                ApplicationController.Instance.DownTime = CurrentManifest.DownTime;

            }
            catch (Exception e)
            {
                Loger.Error<Data>(e.Message);
            }
        }
    }
}