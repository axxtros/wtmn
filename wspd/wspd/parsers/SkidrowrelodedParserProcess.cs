using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.parsers
{
    class SkidrowrelodedURL
    {
        public static String SKIDROW_RELODED_BASE_URL = @"https://www.skidrowreloaded.com/";
        public static String SKIDROW_RELODED_FILE_NAME = @"skidrowreloded_addons.txt";
    }

    public class SkidrowrelodedParserProcess : interfaces.ProcessInterface
    {
        private List<AddonEntity> addonList;
        private HtmlWeb webPage;
        private HtmlDocument htmlDoc;
        private String pageURL;
        private int addonCounter;
        private int currentPage;

        public SkidrowrelodedParserProcess()
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
            currentPage = 1;
            pageURL = SkidrowrelodedURL.SKIDROW_RELODED_BASE_URL;
            addonCounter = 0;

            while (pageURL != null && currentPage != (pageNumber + 1))
            {
                htmlDoc = webPage.Load(pageURL);
                pageURL = null;
                //Console.WriteLine(htmlDoc.DocumentNode.OuterHtml);
                getAddonAndDate(htmlDoc.DocumentNode.DescendantNodes(), currentPage);
                //next page                
                pageURL = this.getNextPageUrl(currentPage);
                ++currentPage;
            }
        }

        private void getAddonAndDate(IEnumerable<HtmlNode> pageNodes, int pageNumber)
        {
            try
            {
                AddonEntity addon = null;
                foreach (HtmlNode hnode in pageNodes)
                {
                    //addon title, download link, addon type                    
                    if (hnode.Name == "div" && hnode.Attributes["class"] != null && hnode.Attributes["class"].Value == "post")
                    {                        
                        if (hnode.FirstChild.NextSibling.Name == "h2" && hnode.FirstChild.NextSibling.FirstChild.Name == "a")
                        {
                            addon = new AddonEntity();
                            addon.ListIndex = ++addonCounter;
                            addon.Type = ADDON_TYPES.GAME;
                            addon.Name = hnode.FirstChild.NextSibling.InnerText;                            
                            addon.AddonURL = hnode.FirstChild.NextSibling.FirstChild.Attributes[0].Value;
                        }
                    }
                    //addon date
                    if (hnode.Name == "div" && hnode.Attributes["class"] != null && hnode.Attributes["class"].Value == "meta")
                    {
                        if(addon != null)
                        {
                            string dateString = hnode.FirstChild.InnerHtml.Substring(hnode.FirstChild.InnerHtml.IndexOf(' '), hnode.FirstChild.InnerHtml.IndexOf(' ') + 6).Trim();
                            string addonDateDay = dateString.Substring(0, 3).Trim();
                            if(addonDateDay[0] == '0')
                            {
                                addonDateDay = dateString.Substring(1, 2).Trim();
                            }
                            addon.Day = Int32.Parse(addonDateDay);
                            string addonDateMonth = dateString.Substring(3, 3).Trim();
                            addon.Month = utils.Utils.getMonthIndexFromName(addonDateMonth);
                            string addonDateYear = dateString.Substring(6, 5).Trim();
                            addon.Year = Int32.Parse(addonDateYear);
                            addon.Page = currentPage;
                            addonList.Add(addon);
                        }
                    }                    
                }                
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("SkidrowrelodedParserProcess getAddonAndDate NullReferenceException: " + ex.StackTrace, ex);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("SkidrowrelodedParserProcess getAddonAndDate Exception: " + ex.StackTrace, ex);
            }
        }

        private string getNextPageUrl(int nextPage)
        {
            //https://www.skidrowreloaded.com/page/100/
            return SkidrowrelodedURL.SKIDROW_RELODED_BASE_URL + "/page/" + ++nextPage + "/";

        }

        #region getters/setters
        public List<AddonEntity> getAddonList()
        {
            return addonList != null ? addonList : new List<AddonEntity>();
        }

        public string getFileName()
        {
            return SkidrowrelodedURL.SKIDROW_RELODED_FILE_NAME;
        }

        public string getParserName()
        {
            return SkidrowrelodedURL.SKIDROW_RELODED_BASE_URL;
        }
        #endregion
    }
}
