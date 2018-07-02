using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.ets2lt
{
    class Ets2LtParser
    {
        const String ETS2LT_BASE_WEB_URL = @"https://ets2.lt/en/";

        private HtmlWeb ets2LtWeb;
        private HtmlDocument ets2LtDoc;        
        private int maxPageNumber;
        private String pageURL;        

        public Ets2LtParser(int maxPageNumber, List<AddonEntity> addonList)
        {
            ets2LtWeb = new HtmlWeb();
            this.maxPageNumber = maxPageNumber;
            processParsingPage(addonList);
        }

        private void processParsingPage(List<AddonEntity> addonList)
        {
            int currentPage = 1;            
            pageURL = ETS2LT_BASE_WEB_URL;            
            while(pageURL != null && currentPage != (maxPageNumber + 1))
            {
                Console.WriteLine("Processing: " + currentPage + ". page");
                ets2LtDoc = ets2LtWeb.Load(pageURL);
                pageURL = null;
                getAddonAndDate(ets2LtDoc.DocumentNode.DescendantNodes(), currentPage, addonList);
                ++currentPage;
            }
        }

        private void getAddonAndDate(IEnumerable<HtmlNode> pageNodes, int pageNumber, List<AddonEntity> addonList)
        {
            try
            {
                AddonEntity addon = null;
                foreach (HtmlNode hnode in pageNodes)
                {
                    //addon title
                    if(hnode.Name == "h2")
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
                        //Console.WriteLine(addonTitle);
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
                        if(addonType != null && addonType.Any())
                        {
                            switch(addonType.ToLower())
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
                        } else
                        {
                            addon.Type = ADDON_TYPES.NOT_DEFINED;
                        }
                        addonList.Add(addon);
                        //Console.WriteLine(addon.Type);
                    }
                    //next page                    
                    if (classValue != null && classValue.Value == "next")
                    {
                        pageURL = hnode.Attributes["href"].Value;                        
                        //Console.WriteLine(pageURL);
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

    }
}
