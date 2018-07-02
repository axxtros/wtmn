using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

/// <summary>
/// Armaholic (http://www.armaholic.com) addons page parser class.
/// 11/04/2018
/// </summary>

namespace wspd.armaholic
{
   class ArmaholicParser
   {
        const String ARMAHOLIC_BASE_WEB_URL = @"http://www.armaholic.com/";
        const bool ALL_ADDON_GET_TO_LIST = false;   //ha minden addon-t le akarunk kérdezni, akkor nincs válogatás

        private HtmlWeb armaholicWeb;
        private HtmlDocument armaholicDoc;
        private List<AddonEntity> addonList;

        private int yearLimit;
        private int monthLimit;
        private int dayLimit;

        public ArmaholicParser(String startingUrl, ADDON_TYPES currentAddonType, List<AddonEntity> resultAddonList, int yearLimit, int monthLimit, int dayLimit)
        {
            armaholicWeb = new HtmlWeb();
            this.yearLimit = yearLimit;
            this.monthLimit = monthLimit;
            this.dayLimit = dayLimit;
            AddonList = new List<AddonEntity>();
            processParsingPage(startingUrl, currentAddonType, resultAddonList);
        }

        private void processParsingPage(String startingPageURL, ADDON_TYPES currentAddonType, List<AddonEntity> resultAddonList)
        {
            String pageURL = startingPageURL;
            int currentPageNumber = 1;
            armaholicDoc = armaholicWeb.Load(pageURL);            
            while (armaholicDoc != null)
            {
                Console.WriteLine("Parsing! Page: " + currentPageNumber + " Page url: " + pageURL);
                getAddonAndDate(armaholicDoc.DocumentNode.Descendants(), currentAddonType, currentPageNumber);
                                
                try
                {
                    pageURL = getNextPageUrl(armaholicDoc.DocumentNode.Descendants()).Replace("amp;", "");
                } catch (System.NullReferenceException)
                {
                    pageURL = null;
                    Console.WriteLine("End parsing!");
                }                
                if (pageURL != null)
                {
                    armaholicDoc = armaholicWeb.Load(pageURL);
                }
                else
                {
                    armaholicDoc = null;
                    break;
                }
                ++currentPageNumber;
            }
            addonFilter(resultAddonList);
        }

        private void getAddonAndDate(IEnumerable<HtmlNode> pageNodes, ADDON_TYPES currentAddonType, int currentPage)
        {
            if (pageNodes != null && pageNodes.Any())
            {
                AddonEntity addonItem = null;
                foreach (HtmlNode hnode in pageNodes)
                {
                    //addon name
                    if (hnode.Name == "strong")
                    {
                        HtmlNode nextNode = hnode.FirstChild;
                        if(nextNode != null && nextNode.Name == "a")
                        {                            
                            HtmlNode fontNode = nextNode.FirstChild;
                            String addonName = fontNode.InnerHtml.Remove(0, 6);
                            String addonLinkUrl = ARMAHOLIC_BASE_WEB_URL + "/" + nextNode.OuterHtml.Substring(nextNode.OuterHtml.IndexOf('=') + 2, nextNode.OuterHtml.IndexOf('>') - 10);
                            addonItem = new AddonEntity(currentAddonType, currentPage);
                            addonItem.Name = addonName;
                            addonItem.AddonURL = addonLinkUrl;
                            Console.WriteLine(addonName + " " + addonLinkUrl);
                        }
                    }
                    //addon date
                    if (hnode.Name == "font")
                    {
                        var classValue = hnode.Attributes["class"];
                        if (classValue != null && classValue.Value == "fontcolordark" && hnode.InnerHtml == "Date:")
                        {
                            String addonDate = hnode.NextSibling.OuterHtml.Remove(0, 6);
                            String addonYear = addonDate.Substring(0, 4);
                            String addonMonth = addonDate.Substring(5, 2);
                            String addonDay = addonDate.Substring(8, 2);
                            addonItem.Year = Int32.Parse(addonYear);
                            addonItem.Month = Int32.Parse(addonMonth);
                            addonItem.Day = Int32.Parse(addonDay);
                            AddonList.Add(addonItem);
                            Console.WriteLine(addonDate + " year: " + addonYear + " month: " + addonMonth + " day: " + addonDay);
                        }                            
                    }
                }
            }
        }        

        private String getNextPageUrl(IEnumerable<HtmlNode> pageNodes)
        {
            String resultURL = null;
            if(pageNodes != null && pageNodes.Any())
            {
                foreach (HtmlNode hnode in pageNodes)
                {
                    if (hnode.Name == "span")
                    {
                        var classValue = hnode.Attributes["class"];
                        if (classValue != null && classValue.Value == "pagenav_next")
                        {                            
                            resultURL = ARMAHOLIC_BASE_WEB_URL + hnode.ChildNodes[0].Attributes["href"].Value;
                            //Console.WriteLine(resultURL);
                            break;
                        }
                    }                    
                }
            }            
            return resultURL;
        }

        //a megadott év, hónap, és napnak megelelő addon-okat teszi bele a resultAddonList-be
        private void addonFilter(List<AddonEntity> resultAddonList)
        {            
            if (AddonList != null && AddonList.Any())
            {                
                foreach (AddonEntity addon in AddonList)
                {                    
                    if(ALL_ADDON_GET_TO_LIST || (addon.Year >= yearLimit && addon.Month >= monthLimit && addon.Day >= dayLimit) )
                    {
                        resultAddonList.Add(addon);
                        //Console.WriteLine("Type: " + addon.Type + ", Page: " + addon.Page + ". Name: " + addon.Name + " Date: " + addon.Year + "." + addon.Month + "." + addon.Day + ".");
                    }                    
                }
            }
        }

        internal List<AddonEntity> AddonList { get => addonList; set => addonList = value; }

    }
}
