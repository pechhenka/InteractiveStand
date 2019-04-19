using System;
using System.IO;

namespace Stand
{
    public class Manifest
    {
        public string NameCallsMatrix = "Calls.csv";
        public string NameLessonsMatrix = "Lessons.csv";
        public string NameExtraMatrix = "Extra.csv";

        public string PathOutsideData = "";

        public int DownTime = 120;

        public bool SupportChangesSchedules = false;
        public bool SupportAutomaticCalling = false;

        public bool HideChangeButtonsIfOutdated = true;

        public void Load(string path)
        {
            string ReadText = (String.Join("",File.ReadAllLines(path))).Replace(" ","");
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
                        break;
                }
            }
        }

        private bool ReadBool(string s)
        {
            s = s.ToLower();
            return (s == "true" || s == "t" || s == "1" || s == "high" || s == "y" || s == "yes");
        }
    }
}