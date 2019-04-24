using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInitializer.Library
{
    public static class RandomExtensions
    {
        public static string NextString(this Random rd, int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string NextDateTime(this Random rd)
        {
            return $"{rd.Next(1990, 2019)}-{rd.Next(0, 13)}-{rd.Next(0, 28)}";
        }

        public static Byte NextByte(this Random rd)
        {
            Byte b = (Byte)rd.Next(0, 256);

            return b;
        }

        public static Char NextChar(this Random rd)
        {
            int num = rd.Next(0, 26);
            char ch = (char)('a' + num);
            return ch;
        }

        public static Decimal NextDecimal(this Random rd)
        {
            var s = GetDecimalScale(rd);
            var a = (int)(uint.MaxValue * rd.NextDouble());
            var b = (int)(uint.MaxValue * rd.NextDouble());
            var c = (int)(uint.MaxValue * rd.NextDouble());
            var n = rd.NextDouble() >= 0.5;
            return new Decimal(a, b, c, n, (Byte)s);
        }

        public static TimeSpan NextTimeSpan(this Random rd)
        {
            return new TimeSpan(0, 0, 0, rd.Next(86400));
        }

        private static int GetDecimalScale(Random r)
        {
            for (int i = 0; i <= 28; i++)
            {
                if (r.NextDouble() >= 0.1)
                    return i;
            }
            return 0;
        }
    }
}
