using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd
{
    class Program
    {

        static void Main(string[] args)
        {
            
        }
    }

    public enum WEBPAGE_ID
    {
        ARMAHOLIC = 1,
        ETS2_LT = 2
    }

    public class WspdEngine
    {
        private List<AddonEntity> armaholicAddonList;

        public WspdEngine()
        {
            //NOP
        }        

        /// <summary>
        /// Web parsing start.
        /// </summary>
        public void webpageParser(WEBPAGE_ID webpageID, int selectedYear, int selectedMonth, int selectedDay)
        {
            switch(webpageID)
            {
                case WEBPAGE_ID.ARMAHOLIC:
                    //armaholic.com
                    armaholic.ArmaholicProcess armaholicProcess = new armaholic.ArmaholicProcess(selectedYear, selectedMonth, selectedDay);
                    armaholicProcess.startParsing(selectedYear, selectedMonth, selectedDay);
                    this.armaholicAddonList = armaholicProcess.ArmaholicAddonList;
                    break;
                case WEBPAGE_ID.ETS2_LT:
                    //ets2.lt
                    ets2lt.Ets2LtProcess ets2LtProcess = new ets2lt.Ets2LtProcess();
                    ets2LtProcess.startParsing(selectedYear, selectedMonth, selectedDay);
                    break;
                default: break;
            }
        }

        #region getter/setters
        public List<AddonEntity> ArmaholicAddonList { get => armaholicAddonList; set => armaholicAddonList = value; }

        #endregion

    }

}
