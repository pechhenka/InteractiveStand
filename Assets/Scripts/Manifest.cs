using System;
using System.IO;

namespace Stand
{
    public class Manifest
    {
        public string NameCallsMatrix = "Calls.csv"; // Имя файла со звонками
        public string NameLessonsMatrix = "Lessons.csv"; // Имя файла с уроками
        public string NameExtraMatrix = "Extra.csv"; // Имя файла с дополнительными занятиями

        public string PathOutsideData = ""; // Папка с внешнеми данными
        public int DownTime = 120; // Времячерез которое стенд вернётся в начальное состояние

        public bool LogNotesRecording = false; // Запись логов с заметками
        public bool LogWarningsRecording = true; // Запись логов с предупреждениями
        public bool LogErrorsRecording = true; // Запись логов с ошибками
        public bool SendLogsToOutsideData = false; // Отправка логов в папку с внемнеми данными

        public bool SupportChangesSchedules = false; // Поддержка меню с кнопками с изменениями в расписаниях
        public bool SupportAutomaticCalling = false; // Включить автоматическую подачу звонков

        public bool HideChangeButtonsIfOutdated = true; // Если изменения устарели скрыть соответствующие кнопки

        public bool Load(string path)
        {
            try
            {
                string ReadText = (String.Join("", File.ReadAllLines(path))).Replace(" ", "");

                foreach (string Values in ReadText.Split(';'))
                {
                    string[] s = Values.Split('=');

                    switch (s[0])
                    {
                        case "NameCallsMatrix":
                            NameCallsMatrix = s[1];
                            break;
                        case "NameLessonsMatrix":
                            NameLessonsMatrix = s[1];
                            break;
                        case "NameExtraMatrix":
                            NameExtraMatrix = s[1];
                            break;
                        case "PathOutsideData":
                            PathOutsideData = s[1];
                            break;

                        case "LogNotesRecording":
                            LogNotesRecording = ReadBool(s[1]);
                            break;
                        case "LogWarningsRecording":
                            LogWarningsRecording = ReadBool(s[1]);
                            break;
                        case "LogErrorsRecording":
                            LogErrorsRecording = ReadBool(s[1]);
                            break;
                        case "SendLogsToOutsideData":
                            SendLogsToOutsideData = ReadBool(s[1]);
                            break;

                        case "SupportChangesSchedules":
                            SupportChangesSchedules = ReadBool(s[1]);
                            break;
                        case "SupportAutomaticCalling":
                            SupportAutomaticCalling = ReadBool(s[1]);
                            break;
                        case "HideChangeButtonsIfOutdated":
                            HideChangeButtonsIfOutdated = ReadBool(s[1]);
                            break;

                        case "DownTime":
                            DownTime = int.Parse(s[1]);
                            break;

                        default:
                            Loger.Warning(path,$"Не прочитаное значение:&{s[0]}&{s[1]}");
                            break;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Loger.Log(path,"Ошибка чтения манифеста:&" + e.Message);
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