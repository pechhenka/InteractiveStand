namespace Stand
{
    public struct Class
    {
        public int Number;
        public char Sign;
        public Class(int Number = 0, char Sign = ' ')
        {
            this.Number = Number;
            this.Sign = Sign;
        }

        public Class(in string s)
        {
            int n = 0;
            char b = ' ';
            foreach (char c in s.Clear())
            {
                if (c >= '0' && c <= '9')
                    n = n * 10 + (c - '0');
                else
                {
                    b = c;
                    break;
                }
            }
            this.Number = n;
            this.Sign = b;
        }

        public new string ToString() => Number.ToString() + Sign;
    }
}