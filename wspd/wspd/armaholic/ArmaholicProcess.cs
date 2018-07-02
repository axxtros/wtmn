
using System.Collections.Generic;
using wspd.commons.entity;

namespace wspd.armaholic
{
    public class ArmaholicProcess : interfaces.ProcessInterface
    {        
        private armaholic.ArmaholicParser armaholicParser;
        private List<AddonEntity> armaholicAddonList = new List<AddonEntity>();
        private int limitYear;
        private int limitMonth;
        private int limitDay;        

        public ArmaholicProcess()
        {
            this.limitYear = 2013;
            this.limitMonth = 1;
            this.limitDay = 1;
        }

        public ArmaholicProcess(int limitYear, int limitMonth, int limitDay)
        {
            this.limitYear = limitYear;
            this.limitMonth = limitMonth;
            this.limitDay = limitDay;
        }

        public void startParsing(int limitYear, int limitMonth, int limitDay)
        {            
            foreach (armaholic.ArmaholicResource addonRes in armaholic.ArmaholicAddonsURL.ARMAHOLIC_RESOURCES)
            {
                armaholicParser = new armaholic.ArmaholicParser(addonRes.Url, addonRes.AddonType, ArmaholicAddonList, limitYear, limitMonth, limitDay);
                
                //https://www.c-sharpcorner.com/UploadFile/mahesh/datagrid-in-wpf/
                //armaholicParser.AddonList
            }            
        }        

        public List<AddonEntity> getArmaholicAddonList()
        {
            if (armaholicParser != null)
            {
                return armaholicParser.AddonList;
            }
            return new List<AddonEntity>();
        }

        public List<AddonEntity> getAddonList()
        {
            return armaholicParser.AddonList;
        }

        internal List<AddonEntity> ArmaholicAddonList { get => armaholicAddonList; set => armaholicAddonList = value; }

    }

}
