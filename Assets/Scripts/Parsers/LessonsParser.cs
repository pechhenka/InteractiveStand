using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using UnityEngine;

namespace Stand
{
    public class LessonsParser : Parser<LessonsParser>, IReceive<SignalLessonsMatrixChanged>, IReceive<SignalChangeLessonsMatrixChanged>
    {
        public List<Class> GetClassesWithoutChanges()
        {
            IRow ClassesRow = Data.Instance.LessonsMatrix?.GetRow(2);

            if (ClassesRow == null) return new List<Class>();

            List<Class> res = new List<Class>();
            int len;
            int number;
            foreach (ICell item in ClassesRow)
            {
                string s = item.Cell();
                if (s == "") continue;

                len = s.Length;

                if (int.TryParse(s.Substring(0, len - 1), out number))
                    if (number >= 5 && number <= 11)
                        res.Add(new Class(s));
            }

            return res;
        }

        public List<Class> GetClasses()
        {
            IRow ClassesRow = Data.Instance.ChangeLessonsMatrix?.GetRow(Data.Instance.CurrentManifest.ChangeLessonsMatrixOffsetY + 1);
            if (ClassesRow == null) return new List<Class>();

            List<Class> res = new List<Class>();
            int len;
            int number;
            foreach (ICell item in ClassesRow)
            {
                string s = item.Cell();
                if (s == "") continue;

                len = s.Length;

                if (int.TryParse(s.Substring(0, len - 1), out number))
                    if (number >= 5 && number <= 11)
                        res.Add(new Class(s));
            }

            return res;
        }

        public TableLessons GetTableLessonsWithoutChanges(Class c) => GetTableLessonsWithoutChanges(c, DateTime.Now.DayOfWeek);

        public TableLessons GetTableLessonsWithoutChanges(Class c, DayOfWeek d)
        {
            if (d == DayOfWeek.Sunday) return new TableLessons();
            IRow ClassesRow = Data.Instance.LessonsMatrix?.GetRow(Data.Instance.CurrentManifest.LessonsMatrixOffsetY);
            if (ClassesRow == null) return new TableLessons();

            int len = ClassesRow.LastCellNum;
            string s = c.ToString();

            List<string> Lesson = new List<string>();
            List<string> Cabinet = new List<string>();

            for (int i = 0; i <= len; i++)
            {
                if (!ClassesRow.Cell(i).Contains(s)) continue;

                int d_Norm = d.Normalising();
                int StartLine = 0, EndLine = 0;
                int LastRow = Data.Instance.LessonsMatrix.LastRowNum;

                int NumberCounter = 0;
                int CurNum = 0;
                int LastNum = 0;
                int j = 0;
                bool LineSwitch = false;
                if (d_Norm == 0) { LineSwitch = true; StartLine = Data.Instance.CurrentManifest.LessonsMatrixOffsetY + 1; }
                if (d_Norm == 5) { EndLine = LastRow; }

                while (NumberCounter < d_Norm + 1 && (j <= LastRow))
                {
                    string val = Data.Instance.LessonsMatrix.GetCell(
                        Data.Instance.CurrentManifest.LessonsMatrixOffsetY + j + 1,
                        Data.Instance.CurrentManifest.LessonsMatrixOffsetX
                        ) ?? "";
                    if (val == "") { j++; continue; }

                    if (!int.TryParse(val, out CurNum)) { j++; continue; }

                    if (CurNum < LastNum)
                    {
                        NumberCounter++;
                        if (NumberCounter > d_Norm - 1)
                            if (!LineSwitch)
                            {
                                LineSwitch = true;
                                StartLine = Data.Instance.CurrentManifest.LessonsMatrixOffsetY + j + 1;
                            }
                            else
                            {
                                EndLine = Data.Instance.CurrentManifest.LessonsMatrixOffsetY + j;
                                break;
                            }
                    }
                    LastNum = CurNum;
                    j++;
                }

                int MissCounter = 0;
                bool LessonFound = false;
                j = 0;
                while (true)
                {
                    if (MissCounter > 16 || (LessonFound && MissCounter > 2) || (StartLine + j > EndLine)) break;

                    if (Data.Instance.LessonsMatrix.GetCell(StartLine + j, i) == "")
                    {
                        MissCounter++;
                        if (!LessonFound)
                        {
                            Lesson.Add("--");
                            Cabinet.Add("--");
                        }
                        j++;
                        continue;
                    }
                    else
                        LessonFound = true;
                    MissCounter = 0;

                    Lesson.Add(Data.Instance.LessonsMatrix.GetCell(StartLine + j, i));

                    string Classrooms = Data.Instance.LessonsMatrix.GetCell(StartLine + j + 1, i + 1)
                        + Data.Instance.LessonsMatrix.GetCell(StartLine + j, i + 2)
                        + Data.Instance.LessonsMatrix.GetCell(StartLine + j + 1, i + 3);
                    Cabinet.Add(Classrooms);
                    j++;
                }

                break;
            }

            return new TableLessons(Lesson, Cabinet);
        }

        public TableLessons GetTableLessons(Class c)
        {
            throw new System.NotImplementedException();
        }

        void IReceive<SignalLessonsMatrixChanged>.HandleSignal(SignalLessonsMatrixChanged arg)
        {
            throw new System.NotImplementedException();
        }

        void IReceive<SignalChangeLessonsMatrixChanged>.HandleSignal(SignalChangeLessonsMatrixChanged arg)
        {
            throw new System.NotImplementedException();
        }
    }
}