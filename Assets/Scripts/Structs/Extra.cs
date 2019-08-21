using System.Collections.Generic;

namespace Stand
{
    public struct Extra
    {
        public List<string> CourseName;
        public List<string> Classes;
        public List<string> Time;
        public List<string> Сlassroom;
        public int Amount;

        public Extra(in List<string> CourseName = null,
            in List<string> Classes = null,
            in List<string> Time = null,
            in List<string> Сlassroom = null,
            in int Amount = 0)
        {
            this.CourseName = CourseName ?? new List<string>();
            this.Classes = Classes ?? new List<string>();
            this.Time = Time ?? new List<string>();
            this.Сlassroom = Сlassroom ?? new List<string>();
            this.Amount = Amount;
        }
    }
}
