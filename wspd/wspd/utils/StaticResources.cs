using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wspd.utils
{  
    public class StaticResources
    {         

        public enum WEBPAGE_KEYS
        {
            ETS2LT,
            ARMAHOLIC,
            SKIDROWRELODED
        }

        public static Dictionary<WEBPAGE_KEYS, string> WEB_PAGE_NAME_LIST = new Dictionary<WEBPAGE_KEYS, string> {            
            { WEBPAGE_KEYS.ETS2LT, "www.ets2.lt" },
            { WEBPAGE_KEYS.ARMAHOLIC, "www.armaholic.com" },
            { WEBPAGE_KEYS.SKIDROWRELODED, "www.skidrowreloded.com" }
        };
    }
}
