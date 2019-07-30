using System;
using UnityEngine;
using NPOI.SS.UserModel;

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

        private readonly Manifest LocalManifest = new Manifest();
        private readonly Manifest OutsideManifest = new Manifest();

        void Awake()
        {
            try
            {
                DataPath = Application.dataPath + @"\LocalData\";

                LocalManifest.Load(DataPath + "Manifest.txt");
                CurrentManifest = LocalManifest;

                if (LocalManifest.PathOutsideData != null)
                {
                    if (OutsideManifest.Load(LocalManifest.PathOutsideData + "Manifest.txt"))
                    {
                        CurrentManifest = OutsideManifest;
                    }
                }

                try { CallsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameCallsMatrix).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e.Message); }
                try { LessonsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameLessonsMatrix).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e.Message); }
                try { ExtraMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameExtraMatrix).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e.Message); }
                if (CurrentManifest.SupportChangesSchedules)
                {
                    try { ChangeCallsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameChangeCallsMatrix).GetSheetAt(0); }
                    catch (Exception e) { Loger.Error<Data>(e.Message); }
                    try { ChangeLessonsMatrix = WorkbookFactory.Create(DataPath + CurrentManifest.NameChangeLessonsMatrix).GetSheetAt(0); }
                    catch (Exception e) { Loger.Error<Data>(e.Message); }
                }

                /*DateTime lol = new DateTime(2019, 7, 25);
                string kek = "";
                Debug.Log(Parser.ContainsDate(ref kek, ref lol));*/

                ApplicationController.Instance.DownTime = CurrentManifest.DownTime;
            }
            catch (Exception e)
            {
                Loger.Error<Data>(e.Message);
            }
        }
    }
}