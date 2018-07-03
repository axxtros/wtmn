using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.parsers
{
    class Ets2ltAddonsURL
    {
        public static String ETS2LT_BASE_WEB_URL = @"https://ets2.lt/en/";
        public static String ETS2LT_FILE_NAME = @"ets2lt_addons.txt";
    }

    class Ets2ltParserProcess : interfaces.ProcessInterface
    {        
        private List<AddonEntity> addonList;
        private HtmlWeb webPage;
        private HtmlDocument htmlDoc;        

        public Ets2ltParserProcess()
        {
            //NOP
        }

        public void startParsing(int selectedYear, int selectedMonth, int selectedDay)
        {
            //NOP
        }

        public void startParsing(int pageNumber)
        {            
            addonList = new List<AddonEntity>();
            webPage = new HtmlWeb();
            int currentPage = 1;
            htmlDoc = webPage.Load(ArmaholicAddonsURL.ARMAHOLIC_BASE_WEB_URL);

            while (currentPage != (pageNumber + 1))
            {

                ++currentPage;
            }
        }

        #region getters/setters
        public List<AddonEntity> getAddonList()
        {
            return addonList != null ? addonList : new List<AddonEntity>();
        }

        public string getFileName()
        {
            return Ets2ltAddonsURL.ETS2LT_FILE_NAME;
        }

        public string getParserName()
        {
            return Ets2ltAddonsURL.ETS2LT_BASE_WEB_URL;
        }        
        #endregion

    }
}
