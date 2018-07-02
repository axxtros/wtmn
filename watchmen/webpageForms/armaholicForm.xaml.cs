using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using watchmen.commonForms;
using wspd;
using wspd.commons.entity;

namespace watchmen.webpageForms
{
    /// <summary>
    /// Interaction logic for armaholicForm.xaml
    /// </summary>
    public partial class ArmaholicForm : UserControl
    {
        private const string ARMAHOLIC_FILE_NAME = @"armaholic_addons.txt";

        private string createDateMarker;
        private string limitDateMarker;
        private string fileAddonStartMarker;
        private string fileAddonEndMarker;

        private WspdEngine wspdEngine;
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private List<AddonEntity> armaholicAddonList;
        private ProgressBarDialogWindow progressBarDialogWindow;
        private BackgroundWorker armaholicBackgroundWorker;                                      //http://www.codescratcher.com/wpf/progress-bar-in-wpf-backgroundworker/
        
        public ArmaholicForm(WspdEngine wspdEngine)
        {
            InitializeComponent();
            this.wspdEngine = wspdEngine;
            init();
        }

        private void init()
        {
            initYearComboBox();
            yearComboBox.SelectionChanged += (s, e) => { datetimeComboBox_SelectionChanged(s, e); };
            initMonthComboBox();
            montComboBox.SelectionChanged += (s, e) => { datetimeComboBox_SelectionChanged(s, e); };
            initDayComboBox();
            dayComboBox.SelectionChanged += (s, e) => { datetimeComboBox_SelectionChanged(s, e); };
            armaholicAddonList = new List<AddonEntity>();
            //progressBarDialogWindow = new ProgressBarDialogWindow();
            armaholicBackgroundWorker = new BackgroundWorker();            
            armaholicBackgroundWorker.DoWork += ArmaholicBackgroundWorker_DoWork;
            armaholicBackgroundWorker.RunWorkerCompleted += ArmaholicBackgroundWorker_RunWorkerCompleted;
            armaholicBackgroundWorker.ProgressChanged += ArmaholicBackgroundWorker_ProgressChanged;

            createDateMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "create";
            limitDateMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "limit";
            fileAddonStartMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "start";
            fileAddonEndMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "end";
            
            createDate.Content = "-";
            limitDate.Content = "-";
        }

        private void initYearComboBox()
        {
            selectedYear = DateTime.Now.Year;
            yearComboBox.Items.Add(DateTime.Now.Year + 1);
            yearComboBox.Items.Add(DateTime.Now.Year);
            yearComboBox.SelectedIndex = 1;
            yearComboBox.Items.Add(DateTime.Now.Year - 1);
            yearComboBox.Items.Add(DateTime.Now.Year - 2);
            yearComboBox.Items.Add(DateTime.Now.Year - 3);
            yearComboBox.Items.Add(DateTime.Now.Year - 4);
            yearComboBox.Items.Add(DateTime.Now.Year - 5);            
        }

        private void initMonthComboBox()
        {            
            for (int monthIdx = 1; monthIdx <= 12; monthIdx++)
            {
                montComboBox.Items.Add(monthIdx);
                if (monthIdx == DateTime.Now.Month)
                {
                    selectedMonth = (monthIdx - 1);
                    montComboBox.SelectedIndex = selectedMonth;
                }
            }            
        }

        private void initDayComboBox()
        {
            if(!dayComboBox.Items.IsEmpty)
            {
                dayComboBox.SelectedIndex = -1;
                dayComboBox.Items.Clear();
            }
            int monthDays = 31; //DateTime.DaysInMonth(selectedYear, selectedMonth);
            for (int dayIdx = 1; dayIdx <= monthDays; dayIdx++)
            {                
                dayComboBox.Items.Add(dayIdx);
                if (dayIdx == DateTime.Now.Day)
                {
                    selectedDay = (dayIdx - 1);
                    dayComboBox.SelectedIndex = selectedDay;
                }
            }            
        }

        private void datetimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox selectedComboBox = (ComboBox) sender;
            if(selectedComboBox != null)
            {
                string comboboxName = selectedComboBox.Name;
                switch(comboboxName)
                {
                    case "yearComboBox": selectedYear = (int) yearComboBox.SelectedItem; break;
                    case "montComboBox": selectedMonth = (int) montComboBox.SelectedItem; break;
                    case "dayComboBox": selectedDay = (int) dayComboBox.SelectedItem; break;
                }
            }
        }

        ///generált eseménykezelője a TabControl-nak
        private void armaholicTabControl_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {            
            if (e.Source is TabControl)
            {
                var tabControl = sender as TabControl;
                TabItem selectedTabItem = null;
                if (tabControl != null)
                {
                    selectedTabItem = (TabItem)tabControl.SelectedItem;
                }
                if (selectedTabItem != null && armaholicAddonList != null && armaholicAddonList.Any())
                {
                    AddonListForm addonListForm = new AddonListForm(armaholicAddonList, selectedTabItem.Name);
                    selectedTabItem.Content = addonListForm;
                }
            }
        }

        /// <summary>
        /// Programozottan a háttérből frissíti az addonTabControl-t, anélkül, hogy arra az UI-n rákattintottak volna.
        /// </summary>
        private void refreshAddonTabControl()
        {
            //az első tabItem a WHEELED típus, ez fog kiválasztódni a frissítés után
            AddonListForm addonListForm = new AddonListForm(armaholicAddonList, ADDON_TYPES.WHEELED.ToString());
            TabItem selectedTabItem = null;
            foreach (TabItem tabItem in addonTabControl.Items)
            {
                if (tabItem.Name == ADDON_TYPES.WHEELED.ToString())
                {
                    selectedTabItem = tabItem;
                    selectedTabItem.Content = addonListForm;
                    break;
                }
            }
            addonTabControl.SelectedItem = selectedTabItem;
            addonTabControl.UpdateLayout();
        }

        private void startArmaholicParserButton_Click(object sender, RoutedEventArgs e)
        {
            armaholicAddonList.Clear();
            progressBarDialogWindow = new ProgressBarDialogWindow();
            progressBarDialogWindow.Show();
            armaholicBackgroundWorker.RunWorkerAsync();
        }

        private void saveAddons_Click(object sender, RoutedEventArgs e)
        {
            if(armaholicAddonList != null && armaholicAddonList.Any())
            {
                writeToFile();
            } else
            {                
                MessageBox.Show(Properties.Resources.empty_addon_list, Properties.Resources.error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void writeToFile()
        {            
            //String monthStr = ((selectedYear <= 9) ? "0" + selectedYear : "" + selectedYear);
            //String dayStr = ((selectedDay <= 9) ? "0" + selectedDay : "" + selectedDay);
            //string filePathAndName = @"c:\dev\html5_dev\armaholic_addons_list_" + selectedYear + monthStr + dayStr + ".txt";
            using (StreamWriter outputFile = new StreamWriter(ARMAHOLIC_FILE_NAME))
            {
                outputFile.WriteLine(createDateMarker + " " + wspd.utils.Utils.NowDate().ToString(wspd.utils.Utils.HungaryDateFormat()));
                outputFile.WriteLine(limitDateMarker + " " + new DateTime(selectedYear, selectedMonth, selectedDay).ToString(wspd.utils.Utils.HungaryDateFormat()));
                outputFile.WriteLine(fileAddonStartMarker + " ");
                foreach (AddonEntity addon in armaholicAddonList)
                {
                    outputFile.WriteLine(
                        addon.ListIndex + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Type + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Name + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.AddonURL + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Page + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Year + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Month + Properties.Resources.FILE_SEPARATOR_CHARACTER + 
                        addon.Day
                    );
                }
                outputFile.WriteLine(fileAddonEndMarker + " ");
            }
            //Console.WriteLine("\nFile created:\n" + ARMAHOLIC_FILE_NAME + "\n");
        }

        private void loadAddons_Click(object sender, RoutedEventArgs e)
        {
            armaholicAddonList.Clear();
            string line = null;
            System.IO.StreamReader file = new System.IO.StreamReader(ARMAHOLIC_FILE_NAME);
            while ((line = file.ReadLine()) != null)
            {
                if(line[0] == Properties.Resources.FILE_META_LINE_CHARACTER[0])
                {
                    int firstSpaceIndex = line.IndexOf(' ');
                    string marker = line.Substring(0, firstSpaceIndex);
                    if (marker.Equals(createDateMarker))
                    {
                        createDate.Content = line.Substring(firstSpaceIndex, line.Length - firstSpaceIndex).Trim();
                    } else if(marker.Equals(limitDateMarker))
                    {
                        limitDate.Content = line.Substring(firstSpaceIndex, line.Length - firstSpaceIndex).Trim();
                    }
                } else
                {
                    String[] addonParams = line.Split(Properties.Resources.FILE_SEPARATOR_CHARACTER[0]);
                    AddonEntity loadedAddon = createAddonFromFileParams(addonParams);
                    armaholicAddonList.Add(loadedAddon);
                }                
            }
            file.Close();
            refreshAddonTabControl();
        }

        private AddonEntity createAddonFromFileParams(String[] addonParams)
        {
            AddonEntity addon = new AddonEntity();
            //linenumber
            addon.ListIndex = Int32.Parse(addonParams[0]);
            //type
            switch (addonParams[1])
            {
                case "GEAR": addon.Type = ADDON_TYPES.GEAR; break;
                case "PACKS": addon.Type = ADDON_TYPES.PACKS; break;
                case "MISCELLANEOUS": addon.Type = ADDON_TYPES.MISCELLANEOUS; break;
                case "OBJECTS": addon.Type = ADDON_TYPES.OBJECTS; break;
                case "REPLACEMENT_PACKS": addon.Type = ADDON_TYPES.REPLACEMENT_PACKS; break;
                case "SOUNDS": addon.Type = ADDON_TYPES.SOUNDS; break;
                case "TERRAIN": addon.Type = ADDON_TYPES.TERRAIN; break;
                case "UNITS": addon.Type = ADDON_TYPES.UNITS; break;
                case "WEAPONS": addon.Type = ADDON_TYPES.WEAPONS; break;
                case "CHOPPERS": addon.Type = ADDON_TYPES.CHOPPERS; break;
                case "HEAVY_ARMOR": addon.Type = ADDON_TYPES.HEAVY_ARMOR; break;
                case "LIGHT_ARMOR": addon.Type = ADDON_TYPES.LIGHT_ARMOR; break;
                case "VEHICLE_PACKS": addon.Type = ADDON_TYPES.VEHICLE_PACKS; break;
                case "PLANES": addon.Type = ADDON_TYPES.PLANES; break;
                case "SEA": addon.Type = ADDON_TYPES.SEA; break;
                case "WHEELED": addon.Type = ADDON_TYPES.WHEELED; break;
            }
            //Name
            addon.Name = addonParams[2];
            //AddonURL
            addon.AddonURL = addonParams[3];
            //Page
            addon.Page = Int32.Parse(addonParams[4]);
            //Year
            addon.Year = Int32.Parse(addonParams[5]);
            //Month
            addon.Month = Int32.Parse(addonParams[6]);
            //Day
            addon.Day = Int32.Parse(addonParams[7]);
            return addon;
        }

        private void runAsyncArmaholicParser()
        {
            this.wspdEngine.webpageParser(WEBPAGE_ID.ARMAHOLIC, selectedYear, selectedMonth, selectedDay);            
            //Itt kell visszakapni a Program.ArmaholicAddonList-et, itt már benne vannak a beolvasott addon adatok a listában.
            armaholicAddonList = this.wspdEngine.ArmaholicAddonList;
            
        }

        #region ArmaholicBackgroundWorker vezérlő metódusok
        private void ArmaholicBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarDialogWindow.prgTest.Value = e.ProgressPercentage;
        }

        private void ArmaholicBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Addig vár a task, ameddig le nem fut. Ekkor a programban nem lehet UI-n keresztül műveletet végezni. Ez fontos, mert vissza kell kapni a this.wspdEngine.ArmaholicAddonList-et.
            var armaholicParserTask = Task.Run(() => runAsyncArmaholicParser());
            armaholicParserTask.Wait();
        }

        private void ArmaholicBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarDialogWindow.Close();
            refreshAddonTabControl();
        }
        #endregion        
    }
}
