﻿using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Stand
{
    public class Manifest
    {
        #region Имена файлов
        public string NameCallsMatrix = "Calls.xls"; // Имя файла со звонками
        public string NameLessonsMatrix = "Lessons.xls"; // Имя файла с уроками
        public string NameExtraMatrix = "Extra.xls"; // Имя файла с дополнительными занятиями

        public string NameChangeCallsMatrix = "ChangeCalls.xls"; // Имя файла с изменёнными звонками
        public string NameChangeLessonsMatrix = "ChangeLessons.xls"; // Имя файла с измененными уроками

        public string ManifestLocalPath { get { return LocalPathData + "Manifest.txt"; } }
        public string CallsMatrixLocalPath { get { return LocalPathData + NameCallsMatrix; } }
        public string LessonsMatrixLocalPath { get { return LocalPathData + NameLessonsMatrix; } }
        public string ExtraMatrixLocalPath { get { return LocalPathData + NameExtraMatrix; } }
        public string ChangeCallsMatrixLocalPath { get { return LocalPathData + NameChangeCallsMatrix; } }
        public string ChangeLessonsMatrixLocalPath { get { return LocalPathData + NameChangeLessonsMatrix; } }

        public string ManifestOutsidePath { get { return OutsidePathData + "Manifest.txt"; } }
        public string CallsMatrixOutsidePath { get { return OutsidePathData + NameCallsMatrix; } }
        public string LessonsMatrixOutsidePath { get { return OutsidePathData + NameLessonsMatrix; } }
        public string ExtraMatrixOutsidePath { get { return OutsidePathData + NameExtraMatrix; } }
        public string ChangeCallsMatrixOutsidePath { get { return OutsidePathData + NameChangeCallsMatrix; } }
        public string ChangeLessonsMatrixOutsidePath { get { return OutsidePathData + NameChangeLessonsMatrix; } }

        public string ManifestCurrentPath { get { return CurrentPathData + "Manifest.txt"; } }
        public string CallsMatrixCurrentPath { get { return CurrentPathData + NameCallsMatrix; } }
        public string LessonsMatrixCurrentPath { get { return CurrentPathData + NameLessonsMatrix; } }
        public string ExtraMatrixCurrentPath { get { return CurrentPathData + NameExtraMatrix; } }
        public string ChangeCallsMatrixCurrentPath { get { return CurrentPathData + NameChangeCallsMatrix; } }
        public string ChangeLessonsMatrixCurrentPath { get { return CurrentPathData + NameChangeLessonsMatrix; } }
        #endregion

        #region Смещения
        public int CallsMatrixOffsetX = 0;
        public int CallsMatrixOffsetY = 0;

        public int LessonsMatrixOffsetX = 1;
        public int LessonsMatrixOffsetY = 2;

        public int ExtraMatrixOffsetX = 0;
        public int ExtraMatrixOffsetY = 0;

        public int ChangeCallsMatrixOffsetX = 0;
        public int ChangeCallsMatrixOffsetY = 0;

        public int ChangeLessonsMatrixOffsetX = 0;
        public int ChangeLessonsMatrixOffsetY = 0;
        #endregion

        public string CurrentPathData = null; // Папка с текущими данными
        public string LocalPathData = null; // Папка с локальными данными
        public string OutsidePathData = null; // Папка с внешними данными

        public int DownTime = 120; // Время через которое стенд вернётся в начальное состояние

        public bool LogNotesRecording = false; // Запись логов с заметками
        public bool LogWarningsRecording = true; // Запись логов с предупреждениями
        public bool LogErrorsRecording = true; // Запись логов с ошибками
        public bool SendLogsToOutsideData = false; // Отправка логов в папку с внешними данными

        public bool SupportChangesSchedules = false; // Поддержка меню с кнопками с изменениями в расписаниях
        public bool SupportAutomaticCalling = false; // Включить автоматическую подачу звонков

        public bool HideChangeButtonsIfOutdated = true; // Если изменения устарели скрыть соответствующие кнопки

        public string WebsiteAddress = "http://192.168.1.211/";

        public bool Load(string path)
        {
            try
            {
                string ReadText = (String.Join("", File.ReadAllLines(path))).Replace(" ", "");
                FieldInfo[] Fields = typeof(Manifest).GetFields();

                foreach (string Values in ReadText.Split(';'))
                {
                    string[] s = Values.Split('=');
                    if (s.Length != 2) continue;

                    foreach (FieldInfo item in Fields)
                    {
                        if (item.Name == s[0])
                        {
                            Type t = item.FieldType;
                            if (t == typeof(int))
                            {
                                item.SetValue(this, int.Parse(s[1]));
                            }
                            else if (t == typeof(string))
                            {
                                item.SetValue(this, s[1]);
                            }
                            else if (t == typeof(bool))
                            {
                                item.SetValue(this, ReadBool(s[1]));
                            }
                            else
                            {
                                Loger.Warning(path, $"Не прочитаное значение:&{s[0]}&{s[1]}");
                            }
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Loger.Warning(path, "Ошибка чтения манифеста:" + e.Message);
            }
            return false;
        }

        private bool ReadBool(string s)
        {
            s = s.ToLower();
            return (s == "true" || s == "t" || s == "1" || s == "high" || s == "y" || s == "yes");
        }
    }
}