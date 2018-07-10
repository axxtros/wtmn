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

    public class Ets2ltParserProcess : interfaces.ProcessInterface
    {        
        private List<AddonEntity> addonList;
        private HtmlWeb webPage;
        private HtmlDocument htmlDoc;
        private String pageURL;
        private int addonCounter;

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
            pageURL = Ets2ltAddonsURL.ETS2LT_BASE_WEB_URL;
            addonCounter = 0;            

            while (pageURL != null && currentPage != (pageNumber + 1))
            {
                htmlDoc = webPage.Load(pageURL);
                pageURL = null;
                //Console.WriteLine(htmlDoc.DocumentNode.OuterHtml);
                getAddonAndDate(htmlDoc.DocumentNode.DescendantNodes(), currentPage);
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
                    //addon title                    
                    if (hnode.Name == "h2")
                    {
                        var classVal = hnode.Attributes["class"];
                        if(classVal.Value == "title")
                        {                            
                            addon = new AddonEntity(ADDON_TYPES.NONE, pageNumber);                            
                            String addonTitle = hnode.InnerText.
                                Replace("&#8211;", "-").
                                Replace("&#038;", "&").
                                Replace("&#8220;", "'").
                                Replace("&#8221;", "'").
                                Replace("&#160;", "@").
                                Replace("&#8216;", "'").
                                Replace("&#8217;", "'").
                                Replace("&#215;", "x");
                            addon.Name = addonTitle;
                            addon.ListIndex = addonList.Count + 1;
                            //Console.WriteLine(addonTitle);
                        }
                        //download link
                        var links = hnode.Descendants("a").Select(node => node.GetAttributeValue("href", "")).ToList();
                        addon.AddonURL = links[0];
                    }
                    //addon date
                    var classValue = hnode.Attributes["class"];
                    if (classValue != null && classValue.Value == "meta_date")
                    {
                        String addonDate = hnode.InnerText;
                        addon.Year = Int32.Parse(addonDate.Substring(0, 4));
                        addon.Month = Int32.Parse(addonDate.Substring(5, 2));
                        addon.Day = Int32.Parse(addonDate.Substring(8, 2));
                        //Console.WriteLine("year: " + addon.Year + " month: " + addon.Month + " day: " + addon.Day);
                    }
                    //addon type
                    if (classValue != null && classValue.Value == "meta_categories")
                    {
                        String addonType = hnode.InnerText;
                        if (addonType != null && addonType.Any())
                        {
                            switch (addonType.ToLower())
                            {
                                case "trucks": addon.Type = ADDON_TYPES.TRUCK_MOD; break;
                                case "trailers": addon.Type = ADDON_TYPES.TRAILER_MOD; break;
                                case "interiors": addon.Type = ADDON_TYPES.INTERIOR_MOD; break;
                                case "interior addons": addon.Type = ADDON_TYPES.INTERIOR_ADDON; break;
                                case "parts/tuning": addon.Type = ADDON_TYPES.PARTS_TUNING_MOD; break;
                                case "ai traffic": addon.Type = ADDON_TYPES.AI_TRAFFIC; break;
                                case "sounds": addon.Type = ADDON_TYPES.SOUND_MOD; break;
                                case "truck skins": addon.Type = ADDON_TYPES.TRUCK_SKIN; break;
                                case "combo skin packs": addon.Type = ADDON_TYPES.COMBO_SKIN_PACKS; break;
                                case "maps": addon.Type = ADDON_TYPES.MAPS; break;
                                case "cars": addon.Type = ADDON_TYPES.CARS; break;
                                case "others": addon.Type = ADDON_TYPES.OTHERS; break;
                                default: addon.Type = ADDON_TYPES.NOT_DEFINED; break;
                            }
                        }
                        else
                        {
                            addon.Type = ADDON_TYPES.NOT_DEFINED;
                        }
                        addon.ListIndex = ++addonCounter;
                        addonList.Add(addon);
                        //Console.WriteLine(addon.Type);
                    }
                    //next page                    
                    if (classValue != null && classValue.Value == "next")
                    {
                        pageURL = hnode.Attributes["href"].Value;
                        //Console.WriteLine("next page: " + pageURL);
                        break;
                    }
                }
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine("getAddonAndDate NullReferenceException: " + ex.StackTrace);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("getAddonAndDate Exception: " + ex.StackTrace);
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
