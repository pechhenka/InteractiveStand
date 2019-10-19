using System;
using System.Text;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

namespace Stand
{
    /* Выполняет подпись даты и номер билда создавая класс BuildInfo до начала сборки
     * получить дату можно следующим образом (возваращает строку):
     * Stand.BuildInfo.BUILD_DATE
     * Stand.BuildInfo.BUILD_NUMBER
     */
    public class PreprocessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            int NumberBuild;
            using (StreamReader inp = new StreamReader(@"Assets/Scripts/Editor/info.txt"))
            {
                NumberBuild = Convert.ToInt32(inp.ReadLine()) + 1;
            }
            using (StreamWriter outp = new StreamWriter(@"Assets/Scripts/Editor/info.txt"))
            {
                outp.Write(NumberBuild);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("public static class BuildInfo");
            sb.Append("{");

            sb.Append("public static string BUILD_DATE = \"");
            sb.Append(DateTime.Now.ToString("dd.MM.yyyy"));
            sb.Append("\";");

            sb.Append("public static string BUILD_NUMBER = \"");
            sb.Append(NumberBuild.ToString());
            sb.Append("\";");

            sb.Append("}");
            sb.Append(Environment.NewLine + "//Данный класс создаётся автоматически редактирование осуществляется в классе PreprocessBuild");
            using (StreamWriter file =
                new StreamWriter(@"Assets/Scripts/Other/BuildInfo.cs"))
            {
                file.WriteLine(sb.ToString());
            }
        }
    }
}