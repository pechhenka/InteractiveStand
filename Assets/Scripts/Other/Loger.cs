using System.IO;
using System;
using UnityEngine;

namespace Stand
{
    /* Нужен для введения логов
     * Просто вызовите нужный метод и он всё за вас красиво оформит
     */
    public static class Loger
    {
        private const string NameNotesLog = "LogNotes_Stand.txt";
        private const string NameWarningsLog = "LogWarnings_Stand.txt";
        private const string NameErrorsLog = "LogErrors_Stand.txt";

        public static void Log(string Name, string Body)
        {
            if (!Data.Instance.CurrentManifest.LogNotesRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameNotesLog;

            if (Name == null || Name == "")
                Name = "Untagged";
            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += Name + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        public static void Log<Name>(string Body)
        {
            if (!Data.Instance.CurrentManifest.LogNotesRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameNotesLog;

            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += typeof(Name).ToString() + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        public static void Warning(string Name, string Body)
        {
            if (!Data.Instance.CurrentManifest.LogWarningsRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameWarningsLog;

            if (Name == null || Name == "")
                Name = "Untagged";
            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += Name + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        public static void Warning<Name>(string Body)
        {
            if (!Data.Instance.CurrentManifest.LogWarningsRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameWarningsLog;

            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += typeof(Name).ToString() + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        public static void Error(string Name, string Body)
        {
            if (!Data.Instance.CurrentManifest.LogErrorsRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameErrorsLog;

            if (Name == null || Name == "")
                Name = "Untagged";
            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += Name + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        public static void Error<Name>(string Body)
        {
            if (!Data.Instance.CurrentManifest.LogErrorsRecording)
                return;

            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameErrorsLog;

            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += typeof(Name).ToString() + "|";
            line += Body + ";";

            try
            {
                if (!File.Exists(writePath))
                    CreateFile(writePath);

                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(line + '\n' + e.Message);
            }
        }

        private static void CreateFile(string writePath)
        {
            using (StreamWriter sw = new StreamWriter(writePath, true))
            {
                sw.WriteLine("date|time|key|value;");
            }
            Log("Лог файл", "не обнаржен&был создан");
        }

        public static void StartLoger()
        {
            Application.logMessageReceived += HandleLog;
        }
        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    if (!Data.Instance.CurrentManifest.LogNotesRecording) return;

                    Log("HookConsole", logString + "&" + stackTrace + "&" + "LogType:" + type.ToString());
                    break;
                case LogType.Warning:
                    if (!Data.Instance.CurrentManifest.LogWarningsRecording) return;

                    Warning("HookConsole", logString + "&" + stackTrace + "&" + "LogType:" + type.ToString());
                    break;
                default:
                    if (!Data.Instance.CurrentManifest.LogErrorsRecording) return;
                    Error("HookConsole", logString + "&" + stackTrace + "&" + "LogType:" + type.ToString());
                    break;
            }
        }
    }
}