using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wspd.utils
{
    public class Utils
    {        

        public static DateTime NowDate()
        {
            return DateTime.Now;
        }

        public static String HungaryDateFormat()
        {
            return "yyyy.MM.dd";
        }

        public static int getMonthIndexFromName(string monthName)
        {
            switch(monthName)
            {
                case "Jan": return 1;
                case "Feb": return 2;
                case "Mar": return 3;
                case "Apr": return 4;
                case "May": return 5;
                case "Jun": return 6;
                case "Jul": return 7;
                case "Aug": return 8;
                case "Sep": return 9;
                case "Okt": return 10;
                case "Nov": return 11;
                case "Dec": return 12;
            }
            return 0;
        }

    }
}
