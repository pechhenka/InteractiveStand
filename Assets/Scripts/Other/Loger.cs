using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

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

        private static void WriteLog(in bool recording, in string writePath, in string Name, in string Body)
        {
            if (!recording) return;

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += (string.IsNullOrEmpty(Name) ? "Untagged" : Name) + "|";
            line += (string.IsNullOrEmpty(Body) ? "Untagged" : Body) + ";";

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
#if UNITY_EDITOR
                Debug.LogWarning(line + '\n' + e.Message);
#endif
            }
        }

        #region Log
        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Log(in string Name, in string Body)
        {
            WriteLog(Data.Instance.CurrentManifest.LogNotesRecording,
#if UNITY_EDITOR
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameNotesLog,
#else
                Application.dataPath + "/Logs/" + NameNotesLog,
#endif
                Name, Body);
        }

        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Log<Name>(in string Body) => Log(typeof(Name).ToString(), Body);

        public static void Log(in string Name, in List<KeyValuePair<string, string>> args)
        {
#pragma warning disable CS0618 // Тип или член устарел
            Log(Name, args.ArgsToString());
#pragma warning restore CS0618 // Тип или член устарел
        }

        public static void Log<Name>(in List<KeyValuePair<string, string>> args) => Log(typeof(Name).ToString(), args);
        #endregion

        #region Warning
        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Warning(string Name, string Body)
        {
            WriteLog(Data.Instance.CurrentManifest.LogWarningsRecording,
#if UNITY_EDITOR
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameWarningsLog,
#else
                Application.dataPath + "/Logs/" + NameWarningsLog,
#endif
            Name, Body);
        }

        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Warning<Name>(string Body) => Warning(typeof(Name).ToString(), Body);
        public static void Warning(in string Name, in List<KeyValuePair<string, string>> args)
        {
#pragma warning disable CS0618 // Тип или член устарел
            Warning(Name, args.ArgsToString());
#pragma warning restore CS0618 // Тип или член устарел
        }

        public static void Warning<Name>(in List<KeyValuePair<string, string>> args) => Warning(typeof(Name).ToString(), args);
        #endregion

        #region Error
        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Error(string Name, string Body)
        {
            WriteLog(Data.Instance.CurrentManifest.LogErrorsRecording,
#if UNITY_EDITOR
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + NameErrorsLog,
#else
                Application.dataPath + "/Logs/" + NameErrorsLog,
#endif
            Name, Body);
        }

        [Obsolete("This method will soon be deprecated. Use new Log method with List<KeyValuePair<string, string>> instead.")]
        public static void Error<Name>(string Body) => Error(typeof(Name).ToString(), Body);

        public static void Error(in string Name, in List<KeyValuePair<string, string>> args)
        {
#pragma warning disable CS0618 // Тип или член устарел
            Error(Name, args.ArgsToString());
#pragma warning restore CS0618 // Тип или член устарел
        }

        public static void Error<Name>(in List<KeyValuePair<string, string>> args) => Error(typeof(Name).ToString(), args);

        public static void Error(in string Name, in Exception e)
        {
            Error(Name, new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Message",e.Message),
                new KeyValuePair<string, string>("StackTrace",e.StackTrace),
                new KeyValuePair<string, string>("Source",e.Source)
            });
        }

        public static void Error<Name>(in Exception e) => Error(typeof(Name).ToString(), e);
        #endregion

        private static void CreateFile(string writePath)
        {
            using (StreamWriter sw = new StreamWriter(writePath, true))
            {
                sw.WriteLine("date|time|topic|arguments;");
            }
            Log("Loger", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Создан лог",writePath)
            });
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

                    Log("HookConsole", new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("logString",logString),
                        new KeyValuePair<string, string>("stackTrace",stackTrace),
                        new KeyValuePair<string, string>("LogType",type.ToString())
                    });
                    break;
                case LogType.Warning:
                    if (!Data.Instance.CurrentManifest.LogWarningsRecording) return;

                    Warning("HookConsole", new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("logString",logString),
                        new KeyValuePair<string, string>("stackTrace",stackTrace),
                        new KeyValuePair<string, string>("LogType",type.ToString())
                    });
                    break;
                default:
                    if (!Data.Instance.CurrentManifest.LogErrorsRecording) return;

                    Error("HookConsole", new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("logString",logString),
                        new KeyValuePair<string, string>("stackTrace",stackTrace),
                        new KeyValuePair<string, string>("LogType",type.ToString())
                    });
                    break;
            }
        }

        private static string ArgsToString(this List<KeyValuePair<string, string>> args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            string body = "";
            foreach (var item in args)
            {
                if (body != "") body += '&';
                if (!string.IsNullOrEmpty(item.Key))
                {
                    body += item.Key;
                    if (!string.IsNullOrEmpty(item.Value))
                        body += '=' + item.Value;
                }
            }
            return body;
        }
    }
}