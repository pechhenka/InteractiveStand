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
                string sourcePath = Application.dataPath;//Папка проекта Unity
                string dataPath = report.summary.outputPath;//Папка Data построенного проекта
                string buildPath = dataPath.Replace(Application.productName + ".exe", "");//Папка построенного проекта

                sourcePath += @"/Data/";
                dataPath = buildPath + Application.productName + "_Data/Data/";

                //Создаёт папку Data
                if (!System.IO.Directory.Exists(dataPath))
                {
                    System.IO.Directory.CreateDirectory(dataPath);
                }

                //Копирует файлы из sourcePath в dataPath
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach (string s in files)
                {
                    if (s.Substring(s.Length - 5) == ".meta")
                        continue;

                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(dataPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

                //Удаляет ненужную папку
                if (System.IO.Directory.Exists(buildPath + Application.productName + "_BackUpThisFolder_ButDontShipItWithYourGame"))
                {
                    System.IO.Directory.Delete(buildPath + Application.productName + "_BackUpThisFolder_ButDontShipItWithYourGame", true);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Ошибка дополнительного PostprocessBuild:" + e.Message);
            }
        }
    }
}