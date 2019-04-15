using System.IO;
using System;
using UnityEngine;

namespace Stand
{
    /* Нужен для введения логов
     * Просто вызовите метод add и он всё за вас красиво оформит
     */
    public static class Loger
    {
        public static void add(string Name, string Body)
        {
            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Log_Stand.txt";

            if (Name == null || Name == "")
                Name = "Untagged";
            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += Name + "|";
            line += Body;

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

        public static void add<Name>(string Body)
        {
            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Log_Stand.txt";

            if (Body == null || Body == "")
                Body = "Untagged";

            string line = DateTime.Now.ToString("yyyy.MM.dd|HH:mm:ss|");
            line += typeof(Name).ToString() + "|";
            line += Body;

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

        public static void CompleteInitialized<T>()
        {
            add(typeof(T).ToString(), "успешно инициализирован");
        }

        public static void ErrorInitialized<T>(Exception e)
        {
            add(typeof(T).ToString(), "ошибка инициализации:" + e.Message);
        }

        private static void CreateFile(string writePath)
        {
            using (StreamWriter sw = new StreamWriter(writePath, true))
            {
                sw.WriteLine("date|time|key|value");
            }
            add("Лог файл", "не обнаржен&был создан");
        }
    }
}