using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wspd.commons.entity;

namespace wspd.ets2lt
{
    class Ets2LtProcess : interfaces.ProcessInterface
    {
        private int limitYear;
        private int limitMonth;
        private int limitDay;
        private List<AddonEntity> addonList;

        public Ets2LtProcess()
        {
            this.limitYear = 2018;
            this.limitMonth = 4;
            this.limitDay = 12;
        }

        public Ets2LtProcess(int limitYear, int limitMonth, int limitDay)
        {
            this.limitYear = limitYear;
            this.limitMonth = limitMonth;
            this.limitDay = limitDay;
        }

        public List<AddonEntity> getAddonList()
        {
            return addonList;
        }

        public void startParsing(int limitYear, int limitMonth, int limitDay)
        {
            Console.WriteLine("ets2.lt process start!");

            addonList = new List<AddonEntity>();
            Ets2LtParser ets2LtParser = new Ets2LtParser(3, addonList);

            if(addonList.Any())
            {
                String monthStr = ((limitMonth <= 9) ? "0" + limitMonth : "" + limitMonth);
                String dayStr = ((limitDay <= 9) ? "0" + limitDay : "" + limitDay);
                string filePathAndName = @"c:\dev\html5_dev\ets2_addons_list_" + limitYear + monthStr + dayStr + ".txt";

                using (StreamWriter outputFile = new StreamWriter(filePathAndName))
                {
                    outputFile.WriteLine("Create date: " + utils.Utils.NowDate().ToString(utils.Utils.HungaryDateFormat()));
                    outputFile.WriteLine("Limit date:  " + new DateTime(limitYear, limitMonth, limitDay).ToString(utils.Utils.HungaryDateFormat()) + "\n");
                    foreach (AddonEntity addon in addonList)
                    {
                        if(addon.Year >= limitYear && addon.Month >= limitMonth && addon.Day >= limitDay)
                        {
                            outputFile.WriteLine(addon.Name + " \nType: " + addon.Type + " Page: " + addon.Page + ". " + addon.Year + "." + addon.Month + "." + addon.Day + "\n");
                        }                        
                    }
                }
                Console.WriteLine("\nFile created:\n" + filePathAndName + "\n");
            }
            Console.WriteLine("\nets2.lt process ending! Press any key to continue...\n");
            Console.ReadKey();
        }
    }
}
