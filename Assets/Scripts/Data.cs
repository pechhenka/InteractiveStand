﻿using System;
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

        private FileSystemWatcher watcherLocal;
        private FileSystemWatcher watcherOutside;

        void Awake()
        {
            try
            {
#if UNITY_EDITOR
                DataPath = Application.dataPath + "/Editor/LocalData/";
#else
                DataPath = Application.dataPath + "/LocalData/";
#endif
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

                try { CallsMatrix = WorkbookFactory.Create(CurrentManifest.CallsMatrixCurrentPath).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e); }
                try { LessonsMatrix = WorkbookFactory.Create(CurrentManifest.LessonsMatrixCurrentPath).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e); }
                try { ExtraMatrix = WorkbookFactory.Create(CurrentManifest.ExtraMatrixCurrentPath).GetSheetAt(0); }
                catch (Exception e) { Loger.Error<Data>(e); }
                if (CurrentManifest.SupportChangesSchedules)
                {
                    try { ChangeCallsMatrix = WorkbookFactory.Create(CurrentManifest.ChangeCallsMatrixCurrentPath).GetSheetAt(0); }
                    catch (Exception e) { Loger.Error<Data>(e); }
                    try { ChangeLessonsMatrix = WorkbookFactory.Create(CurrentManifest.ChangeLessonsMatrixCurrentPath).GetSheetAt(0); }
                    catch (Exception e) { Loger.Error<Data>(e); }
                }

                ProcessingSignals.Default.Add(CallsParser.Instance);
                ProcessingSignals.Default.Add(LessonsParser.Instance);
                ProcessingSignals.Default.Add(ExtraParser.Instance);

                try { StartWatch(); }
                catch (Exception e) { Loger.Error<Data>(e); }

                ApplicationController.Instance.DownTime = CurrentManifest.DownTime;
            }
            catch (Exception e)
            {
                Loger.Error<Data>(e);
            }
        }

        void StartWatch()
        {
            watcherLocal = new FileSystemWatcher
            {
                Path = CurrentManifest.LocalPathData,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            watcherLocal.Created += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.Changed += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.Deleted += new FileSystemEventHandler(OnEventLocal);
            watcherLocal.EnableRaisingEvents = true;

            if (CurrentManifest.OutsidePathData != null)
            {
                watcherOutside = new FileSystemWatcher
                {
                    Path = CurrentManifest.OutsidePathData,
                    NotifyFilter = NotifyFilters.LastWrite,
                    Filter = "*.*"
                };
                watcherOutside.Created += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.Changed += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.Deleted += new FileSystemEventHandler(OnEventOutside);
                watcherOutside.EnableRaisingEvents = true;
            }
        }

        void OnEventLocal(object source, FileSystemEventArgs e)
        {
            ISheet GetMatrix(string s)
            {
                ISheet res = null;
                if ((LocalManifest.OutsidePathData != null && File.Exists(s)) || (e.ChangeType == WatcherChangeTypes.Deleted))
                    return res;

                try { res = WorkbookFactory.Create(e.FullPath).GetSheetAt(0); }
                catch (Exception ex) { Loger.Error<Data>(ex); }
                return res;
            }
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
            ISheet GetMatrix()
            {
                ISheet res = null;
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                    return res;

                try { res = WorkbookFactory.Create(e.FullPath).GetSheetAt(0); }
                catch (Exception ex) { Loger.Error<Data>(ex); }
                return res;
            }
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