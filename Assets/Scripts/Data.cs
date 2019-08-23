using System;
using UnityEngine;
using NPOI.SS.UserModel;
using System.IO;

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
                DataPath = Application.dataPath + "/LocalData/";
                LocalManifest.LocalPathData = DataPath;
                OutsideManifest.LocalPathData = DataPath;
                LocalManifest.CurrentPathData = DataPath;
                OutsideManifest.CurrentPathData = DataPath;

                LocalManifest.Load(DataPath + "Manifest.txt");
                CurrentManifest = LocalManifest;

                if (LocalManifest.OutsidePathData != null)
                {
                    if (OutsideManifest.Load(LocalManifest.OutsidePathData + "Manifest.txt"))
                    {
                        LocalManifest.CurrentPathData = LocalManifest.OutsidePathData;
                        OutsideManifest.OutsidePathData = LocalManifest.OutsidePathData;
                        OutsideManifest.CurrentPathData = LocalManifest.OutsidePathData;
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

                ProcessingSignals.Default.Add(CallsParser.Instance);
                ProcessingSignals.Default.Add(LessonsParser.Instance);
                ProcessingSignals.Default.Add(ExtraParser.Instance);

                try { StartWatch(); }
                catch (Exception e) { Loger.Error<Data>(e.Message); }

                ApplicationController.Instance.DownTime = CurrentManifest.DownTime;
            }
            catch (Exception e)
            {
                Loger.Error<Data>(e.Message);
            }
        }

        void StartWatch()
        {
            FileSystemWatcher watcherLocal = new FileSystemWatcher();
            watcherLocal.Path = CurrentManifest.LocalPathData;
            watcherLocal.NotifyFilter = NotifyFilters.LastWrite;
            watcherLocal.Filter = "*.*";
            watcherLocal.Created += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.Changed += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.Deleted += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.EnableRaisingEvents = true;

            if (CurrentManifest.OutsidePathData != null)
            {
                FileSystemWatcher watcherOutside = new FileSystemWatcher();
                watcherOutside.Path = CurrentManifest.OutsidePathData;
                watcherOutside.NotifyFilter = NotifyFilters.LastWrite;
                watcherOutside.Filter = "*.*";
                watcherOutside.Created += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.Changed += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.Deleted += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.EnableRaisingEvents = true;
            }
        }

        void OnEventLocal(object source, FileSystemEventArgs e)
        {
            Func<string, ISheet> GetMatrix = (s) =>
            {
                ISheet res = null;
                if ((LocalManifest.OutsidePathData != null && File.Exists(s)) || (e.ChangeType == WatcherChangeTypes.Deleted))
                    return res;

                try { res = WorkbookFactory.Create(e.FullPath).GetSheetAt(0); }
                catch (Exception ex) { Loger.Error<Data>(ex.Message); }
                return res;
            };
            if (e.FullPath.EqualsPath(LocalManifest.CallsMatrixLocalPath))
            {
                ISheet t = GetMatrix(OutsideManifest.CallsMatrixOutsidePath);
                if (t != null)
                {
                    CallsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalCallsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(LocalManifest.LessonsMatrixLocalPath))
            {
                ISheet t = GetMatrix(OutsideManifest.LessonsMatrixOutsidePath);
                if (t != null)
                {
                    LessonsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalLessonsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(LocalManifest.ExtraMatrixLocalPath))
            {
                ISheet t = GetMatrix(OutsideManifest.ExtraMatrixOutsidePath);
                if (t != null)
                {
                    ExtraMatrix = t;
                    ProcessingSignals.Default.Send(new SignalExtraMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(LocalManifest.ChangeCallsMatrixLocalPath))
            {
                ISheet t = GetMatrix(OutsideManifest.ChangeCallsMatrixOutsidePath);
                if (t != null)
                {
                    ChangeCallsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalChangeCallsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(LocalManifest.ChangeLessonsMatrixLocalPath))
            {
                ISheet t = GetMatrix(OutsideManifest.ChangeLessonsMatrixOutsidePath);
                if (t != null)
                {
                    ChangeLessonsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalChangeLessonsMatrixChanged());
                }
            }
        }

        void OnEventOutside(object source, FileSystemEventArgs e)
        {
            Func<ISheet> GetMatrix = () =>
            {
                ISheet res = null;
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                    return res;

                try { res = WorkbookFactory.Create(e.FullPath).GetSheetAt(0); }
                catch (Exception ex) { Loger.Error<Data>(ex.Message); }
                return res;
            };
            if (e.FullPath.EqualsPath(OutsideManifest.CallsMatrixOutsidePath))
            {
                ISheet t = GetMatrix();
                if (t != null)
                {
                    CallsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalCallsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(OutsideManifest.LessonsMatrixOutsidePath))
            {
                ISheet t = GetMatrix();
                if (t != null)
                {
                    LessonsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalLessonsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(OutsideManifest.ExtraMatrixOutsidePath))
            {
                ISheet t = GetMatrix();
                if (t != null)
                {
                    ExtraMatrix = t;
                    ProcessingSignals.Default.Send(new SignalExtraMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(OutsideManifest.ChangeCallsMatrixOutsidePath))
            {
                ISheet t = GetMatrix();
                if (t != null)
                {
                    ChangeCallsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalChangeCallsMatrixChanged());
                }
            }
            else if (e.FullPath.EqualsPath(OutsideManifest.ChangeLessonsMatrixOutsidePath))
            {
                ISheet t = GetMatrix();
                if (t != null)
                {
                    ChangeLessonsMatrix = t;
                    ProcessingSignals.Default.Send(new SignalChangeLessonsMatrixChanged());
                }
            }
        }
    }
}