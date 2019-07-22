using System;
using System.Collections.Generic;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace Stand
{
    /* Хранит в себе загружаемые данные
     */
    public class Data : Singleton<Data>
    {
        public ISheet CallsMatrix = null;
        public ISheet LessonsMatrix = null;
        public ISheet ExtraMatrix = null;
        public ISheet ChangeCallsMatrix = null;
        public ISheet ChangeLessonsMatrix = null;

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

                CallsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameCallsMatrix).GetSheetAt(0);
                LessonsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameLessonsMatrix).GetSheetAt(0);
                ExtraMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameExtraMatrix).GetSheetAt(0);
                if (CurrentManifest.SupportChangesSchedules)
                {
                    ChangeCallsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameChangeCallsMatrix).GetSheetAt(0);
                    ChangeLessonsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameChangeLessonsMatrix).GetSheetAt(0);
                }
                ApplicationController.Instance.DownTime = CurrentManifest.DownTime;
            }
            catch (Exception e)
            {
                Loger.Error<Data>(e.Message);
            }
        }
    }
}