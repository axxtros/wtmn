﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.parsers
{
    class ArmaholicAddonsURL
    {
        public static String ARMAHOLIC_BASE_WEB_URL = @"http://www.armaholic.com/";

        public static List<ArmaholicResource> ARMAHOLIC_RESOURCES = new List<ArmaholicResource> {
            //addons
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_gear&s=title&w=asc&d=0", ADDON_TYPES.GEAR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_packs&s=title&w=asc&d=0", ADDON_TYPES.PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_misc&s=title&w=asc&d=0", ADDON_TYPES.MISCELLANEOUS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_objects&s=title&w=asc&d=0", ADDON_TYPES.OBJECTS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_replacements&s=title&w=asc&d=0", ADDON_TYPES.REPLACEMENT_PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_soundmods&s=title&w=asc&d=0", ADDON_TYPES.SOUNDS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_islands&s=title&w=asc&d=0", ADDON_TYPES.TERRAIN),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_units&s=title&w=asc&d=0", ADDON_TYPES.UNITS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_weapons&s=title&w=asc&d=0", ADDON_TYPES.WEAPONS),
            ////vehicles
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_chopper&s=title&w=asc&d=0", ADDON_TYPES.CHOPPERS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_heavyarmor&s=title&w=asc&d=0", ADDON_TYPES.HEAVY_ARMOR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_lightarmor&s=title&w=asc&d=0", ADDON_TYPES.LIGHT_ARMOR),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_packs&s=title&w=asc&d=0", ADDON_TYPES.VEHICLE_PACKS),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_planes&s=title&w=asc&d=0", ADDON_TYPES.PLANES),
            //new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_sea&s=title&w=asc&d=0", ADDON_TYPES.SEA),
            new ArmaholicResource(@"http://www.armaholic.com/list.php?c=arma3_files_addons_vehicles_wheeled&s=title&w=asc&d=0", ADDON_TYPES.WHEELED)
        };
    }

    class ArmaholicResource
    {
        private String url;
        private wspd.commons.entity.ADDON_TYPES addonType;

        public ArmaholicResource(string url, ADDON_TYPES addonType)
        {
            this.Url = url;
            this.AddonType = addonType;
        }

        public string Url { get => url; set => url = value; }
        internal ADDON_TYPES AddonType { get => addonType; set => addonType = value; }
    }

    public class ArmaholicParserProcess : interfaces.ProcessInterface
    {
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private List<AddonEntity> armaholicAddonList;
        private HtmlWeb armaholicWeb;
        private HtmlDocument armaholicDoc;        

        public ArmaholicParserProcess()
        {
            //NOP
        }        

        public void startParsing(int selectedYear, int selectedMonth, int selectedDay)
        {
            this.selectedYear = selectedYear;
            this.selectedMonth = selectedMonth;
            this.selectedDay = selectedDay;

            armaholicAddonList = new List<AddonEntity>();
            armaholicWeb = new HtmlWeb();
            foreach (ArmaholicResource addonRes in ArmaholicAddonsURL.ARMAHOLIC_RESOURCES)
            {
                processParsingPage(addonRes.Url, addonRes.AddonType);
            }
        }

        #region armaholic parsers functions
        private void processParsingPage(String startingPageURL, ADDON_TYPES currentAddonType)
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
                }
                catch (System.NullReferenceException)
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
                        if (nextNode != null && nextNode.Name == "a")
                        {
                            HtmlNode fontNode = nextNode.FirstChild;
                            String addonName = fontNode.InnerHtml.Remove(0, 6);
                            String addonLinkUrl = ArmaholicAddonsURL.ARMAHOLIC_BASE_WEB_URL + "/" + nextNode.OuterHtml.Substring(nextNode.OuterHtml.IndexOf('=') + 2, nextNode.OuterHtml.IndexOf('>') - 10);
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
                            addonItem.ListIndex = 0;                            
                            addAddonItemToFilteredList(addonItem);
                            Console.WriteLine(addonDate + " year: " + addonYear + " month: " + addonMonth + " day: " + addonDay);
                        }
                    }
                }
            }
        }

        private void addAddonItemToFilteredList(AddonEntity addonItem)
        {
            if(addonItem.Year >= this.selectedYear && addonItem.Month >= this.selectedMonth && addonItem.Day >= this.selectedDay)
            {
                armaholicAddonList.Add(addonItem);
            }
        }

        private String getNextPageUrl(IEnumerable<HtmlNode> pageNodes)
        {
            String resultURL = null;
            if (pageNodes != null && pageNodes.Any())
            {
                foreach (HtmlNode hnode in pageNodes)
                {
                    if (hnode.Name == "span")
                    {
                        var classValue = hnode.Attributes["class"];
                        if (classValue != null && classValue.Value == "pagenav_next")
                        {
                            resultURL = ArmaholicAddonsURL.ARMAHOLIC_BASE_WEB_URL + hnode.ChildNodes[0].Attributes["href"].Value;                            
                            break;
                        }
                    }
                }
            }
            return resultURL;
        }
        #endregion

        #region getters/setters
        public List<AddonEntity> getAddonList()
        {
            return armaholicAddonList != null ? armaholicAddonList : new List<AddonEntity>();
        }
        #endregion
    }
}
