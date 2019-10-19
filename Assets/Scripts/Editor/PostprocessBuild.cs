using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;

namespace Stand
{
    /* Копирует все файлы из папки Data (кроме *.meta)
     * Нужно для класса Data (или вы можете сами закинуть файлы)
     * 
     * Также удаляет папку _BackUpThisFolder_ButDontShipItWithYourGame
     * Потому что она много весит и нужна она только для отладки (можно также ручками это сделать)
     * 
     * Вы конечно могли все эти действия сделать вручную
     * Но я решил облегчить вам жизнь и автоматизировал это :)
     * 
     * Если при сборке проекта этот класс жалуется, то вы можете игнорировать его ошибку и сделать его работу вручную
     */
    public class PostprocessBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            try
            {
                string source_Path = Application.dataPath + "/Editor/"; // ../Unity projects/InteractiveStand/Assets/Editor/
                string build_Path = report.summary.outputPath // ../Interactive Stand/Interactive Stand.exe
                    .Replace(Application.productName + ".exe", ""); // ../Interactive Stand/

                string build_LocalDataPath = build_Path + Application.productName + "_Data/LocalData/";
                string build_TaskSchedulerPath = build_Path + "Task scheduler/";

                if (!System.IO.Directory.Exists(build_LocalDataPath))
                    System.IO.Directory.CreateDirectory(build_LocalDataPath);
                if (!System.IO.Directory.Exists(build_TaskSchedulerPath))
                    System.IO.Directory.CreateDirectory(build_TaskSchedulerPath);

                // Копирует таблицы и манифест
                string[] files = System.IO.Directory.GetFiles(source_Path + "LocalData/");
                foreach (string s in files)
                {
                    if (s.Substring(s.Length - 5) == ".meta")
                        continue;

                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(build_LocalDataPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

                // Копирует Task scheduler
                files = System.IO.Directory.GetFiles(source_Path + "Task scheduler/");
                foreach (string s in files)
                {
                    if (s.Substring(s.Length - 5) == ".meta")
                        continue;

                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(build_TaskSchedulerPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

                // Копирует руководства обслуживания
                files = System.IO.Directory.GetFiles(source_Path + "Guides/");
                foreach (string s in files)
                {
                    if (s.Substring(s.Length - 5) == ".meta")
                        continue;

                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(build_Path, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

                //При сборке IL2CPP
                if (System.IO.Directory.Exists(build_Path + Application.productName + "_BackUpThisFolder_ButDontShipItWithYourGame"))
                    System.IO.Directory.Delete(build_Path + Application.productName + "_BackUpThisFolder_ButDontShipItWithYourGame", true);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Ошибка PostprocessBuild:" + e.Message);
            }
        }
    }
}