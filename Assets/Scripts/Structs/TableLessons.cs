using System.Collections.Generic;

namespace Stand
{
    public struct TableLessons
    {
        public List<string> Lesson;
        public List<string> Cabinet;

        public TableLessons(in List<string> Lesson = null, in List<string> Cabinet = null)
        {
            this.Lesson = Lesson ?? new List<string>();
            this.Cabinet = Cabinet ?? new List<string>();
        }
    }
}
