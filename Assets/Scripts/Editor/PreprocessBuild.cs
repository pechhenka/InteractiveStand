using System;
using System.Text;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Stand
{
    /* Выполняет подпись даты билда создавая класс BuildInfo до начала сборки
     * получить дату можно следующим образом (возваращает строку):
     * Stand.BuildInfo.BUILD_DATE
     */
    public class PreprocessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public static class BuildInfo");
            sb.Append("{");
            sb.Append("public static string BUILD_DATE = \"");
            sb.Append(DateTime.Now.ToString("dd.MM.yyyy"));
            sb.Append("\";");
            sb.Append("}");
            sb.Append('\n' + "//Данный класс создаётся автоматически редактирование осуществляется в классе PreprocessBuild");
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"Assets/Scripts/Other/BuildInfo.cs"))
            {
                file.WriteLine(sb.ToString());
            }
        }
    }
}