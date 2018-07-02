using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchmen.utils
{
    class StaticResources
    {
        public static String PROGRAM_NAME = "Watchman";
        private static String PROGRAM_VER = "1.0";

        public static string PROGRAM_FULL_TITLE()
        {
            return PROGRAM_NAME + " " + PROGRAM_VERSION;
        }

        public static string PROGRAM_VERSION { get => "v." + PROGRAM_VER; set => PROGRAM_VER = value; }        

    }
}
