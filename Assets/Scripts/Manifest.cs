using System;
using System.IO;

namespace Stand
{
    public class Manifest
    {
        public string NameCallsMatrix = "Calls.xls"; // Имя файла со звонками
        public string NameLessonsMatrix = "Lessons.xls"; // Имя файла с уроками
        public string NameExtraMatrix = "Extra.xls"; // Имя файла с дополнительными занятиями

        public string NameChangeCallsMatrix = "ChangeCalls.xls"; // Имя файла с изменёнными звонками
        public string NameChangeLessonsMatrix = "ChangeLessons.xls"; // Имя файла с измененными уроками

        public string PathOutsideData = null; // Папка с внешнеми данными
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

                foreach (string Values in ReadText.Split(';'))
                {
                    string[] s = Values.Split('=');
                    if (s.Length < 2) continue;

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
                        case "NameChangeCallsMatrix":
                            NameChangeCallsMatrix = s[1];
                            break;
                        case "NameChangeLessonsMatrix":
                            NameChangeLessonsMatrix = s[1];
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

                        case "WebsiteAddress":
                            WebsiteAddress = s[1];
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
                Loger.Warning(path,"Ошибка чтения манифеста:" + e.Message);
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