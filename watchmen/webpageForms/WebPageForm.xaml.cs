using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using watchmen.commonForms;
using watchmen.interfaces;
using wspd.commons.entity;
using wspd.interfaces;

namespace watchmen.webpageForms
{
    /// <summary>
    /// Interaction logic for WebPageForm.xaml
    /// </summary>
    public partial class WebPageForm : UserControl, INotifyPropertyChanged
    {
        private readonly int TABITEM_HEIGHT = 30;
        private readonly int MINUS_YEARS = 10;

        private String formTitle;
        private Color leftColor;
        private Color rightColor;
        private DateTime now;
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private int selectedPage;
        private ADDON_TYPES[] addonTypes;
        private PAGE_PARSER_TYPES parserType;
        private List<AddonEntity> addonList;
        private ProcessInterface process;
        private ProgressBarDialogWindow progressBarDialogWindow;
        private BackgroundWorker backgroundWorker; //http://www.codescratcher.com/wpf/progress-bar-in-wpf-backgroundworker/
        public event PropertyChangedEventHandler PropertyChanged;   //https://www.codeproject.com/Articles/301678/Step-by-Step-WPF-Data-Binding-with-Comboboxes   
        private AddonListForm addonListForm;
        private TabItem lastSelectedTabItem;
        private string createDateMarker;
        private string limitMaker;
        private string fileAddonStartMarker;
        private string fileAddonEndMarker;
        private bool isWebParsing;

        public WebPageForm(String webPageTitle, 
            Color gardientLeftColor, Color gardientRightColor, 
            ADDON_TYPES[] addonTypes, 
            ProcessInterface processObject, 
            PAGE_PARSER_TYPES parserType)
        {
            InitializeComponent();
            this.addonList = new List<AddonEntity>();
            this.addonTypes = addonTypes;
            this.process = processObject;
            this.now = DateTime.Now;
            init(webPageTitle, gardientLeftColor, gardientRightColor);
            isWebParsing = false;
            this.parserType = parserType;
            this.DataContext = this;
        }

        //https://stackoverflow.com/questions/5824600/databind-from-xaml-to-code-behind
        private void init(String webPageTitle, Color gardientLeftColor, Color gardientRightColor)
        {
            initUI(webPageTitle, gardientLeftColor, gardientRightColor);
            initYearComboBox();
            initMonthComboBox();
            initDayComboBox();
            initPageComboBox();
            initAddonTabs();
            initBackgroundWorker();
            initFileDates();
        }

        #region init components
        private void initUI(String webPageTitle, Color gardientLeftColor, Color gardientRightColor)
        {
            FormTitle = webPageTitle;
            if (gardientLeftColor == null || gardientRightColor == null)
            {
                leftColor = rightColor = Colors.White;
            }
            else
            {
                leftColor = gardientLeftColor;
                rightColor = gardientRightColor;
            }
        }

        private void initYearComboBox()
        {
            int currentYear = now.Year;
            for (int minusYear = 0; minusYear != MINUS_YEARS; minusYear++)
            {
                yearComboBox.Items.Add(currentYear - minusYear);
            }
            SelectedYear = yearComboBox.SelectedIndex = now.Year;
        }

        private void initMonthComboBox()
        {
            for (int monthIdx = 1; monthIdx <= 12; monthIdx++)
            {
                montComboBox.Items.Add(monthIdx);                
            }
            SelectedMonth = montComboBox.SelectedIndex = now.Month;
        }

        private void initDayComboBox()
        {            
            int monthDays = 31;
            for (int dayIdx = 1; dayIdx <= monthDays; dayIdx++)
            {
                dayComboBox.Items.Add(dayIdx);                
            }
            SelectedDay = dayComboBox.SelectedIndex = now.Day;
        }

        private void initPageComboBox()
        {
            for (int pageIdx = 1; pageIdx <= 9; pageIdx++)
            {
                pageComboBox.Items.Add(pageIdx);
            }
            for (int pageIdx = 10; pageIdx <= 99; pageIdx += 5)
            {
                pageComboBox.Items.Add(pageIdx);
            }
            for (int pageIdx = 100; pageIdx <= 500; pageIdx += 10)
            {
                pageComboBox.Items.Add(pageIdx);
            }
            SelectedPage = pageComboBox.SelectedIndex = 0;
        }

        private void initAddonTabs()
        {
            if(addonTypes.Length > 0)
            {
                foreach(ADDON_TYPES type in addonTypes)
                {
                    TabItem addonTabItem = new TabItem();
                    addonTabItem.Name = type.ToString();                    
                    addonTabItem.Header = type.ToString().ToUpper();
                    addonTabItem.Height = TABITEM_HEIGHT;
                    addonTabs.Items.Add(addonTabItem);
                }
            }
        }

        private void initBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
        }

        private void initFileDates()
        {
            createDateMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "create";
            limitMaker = Properties.Resources.FILE_META_LINE_CHARACTER + "limit";
            fileAddonStartMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "start";
            fileAddonEndMarker = Properties.Resources.FILE_META_LINE_CHARACTER + "end";
            createDate.Content = "-";
            limitParam.Content = "-";
        }

        #endregion;        

        #region ui component events (buttons, etc.)
        //weboldal felolvasása
        private void parserButton_Click(object sender, RoutedEventArgs e)
        {
            addonList.Clear();
            progressBarDialogWindow = new ProgressBarDialogWindow();
            progressBarDialogWindow.Show();
            backgroundWorker.RunWorkerAsync();
        }

        private void addonTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var tabControl = sender as TabControl;
                TabItem selectedTabItem = null;
                if (tabControl != null)
                {
                    selectedTabItem = (TabItem)tabControl.SelectedItem;
                }
                if (selectedTabItem != null && addonList != null && (addonList.Count > 0))
                {
                    this.addonListForm = new AddonListForm(addonList, selectedTabItem.Name);
                    selectedTabItem.Content = addonListForm;                    
                }
                if(selectedTabItem != null)
                {
                    lastSelectedTabItem = selectedTabItem;
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            saveToFile();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            loadFromFile();
        }
        #endregion        

        private void saveToFile()
        {
            if (isWebParsing && addonList != null && addonList.Count > 0)
            {
                using (StreamWriter outputFile = new StreamWriter(process.getFileName()))
                {
                    outputFile.WriteLine(createDateMarker + " " + wspd.utils.Utils.NowDate().ToString(wspd.utils.Utils.HungaryDateFormat()));
                    if(parserType == PAGE_PARSER_TYPES.DATE_BASED)
                    {
                        outputFile.WriteLine(limitMaker + " " + new DateTime(selectedYear, selectedMonth, selectedDay).ToString(wspd.utils.Utils.HungaryDateFormat()));
                    } else if(parserType == PAGE_PARSER_TYPES.PAGE_BASED)
                    {
                        outputFile.WriteLine(limitMaker + " " + SelectedPage);
                    }                    
                    outputFile.WriteLine(fileAddonStartMarker + " ");
                    foreach (AddonEntity addon in addonList)
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
                isWebParsing = false;
            }
        }

        private void loadFromFile()
        {
            addonList.Clear();
            string line = null;
            System.IO.StreamReader file = new System.IO.StreamReader(process.getFileName());
            while ((line = file.ReadLine()) != null)
            {
                if (line[0] == Properties.Resources.FILE_META_LINE_CHARACTER[0])
                {
                    int firstSpaceIndex = line.IndexOf(' ');
                    string marker = line.Substring(0, firstSpaceIndex);
                    if (marker.Equals(createDateMarker))
                    {
                        createDate.Content = line.Substring(firstSpaceIndex, line.Length - firstSpaceIndex).Trim();
                    }
                    else if (marker.Equals(limitMaker))
                    {
                        limitParam.Content = line.Substring(firstSpaceIndex, line.Length - firstSpaceIndex).Trim();
                    }
                }
                else
                {
                    String[] addonParams = line.Split(Properties.Resources.FILE_SEPARATOR_CHARACTER[0]);
                    AddonEntity addonItem = createAddonFromFileParams(addonParams);
                    addonList.Add(addonItem);
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
            addon.Type = AddonTypeConverter.GetType(addonParams[1]);                        
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

        #region backgroundWorker vezérlő metódusok
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarDialogWindow.prgTest.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Addig vár a task, ameddig le nem fut. Ekkor a programban nem lehet UI-n keresztül műveletet végezni.            
            var parserTask = Task.Run(() => webProcess());
            parserTask.Wait();
        }

        private void webProcess()
        {            
            if(IsParseDateBased)
            {
                process.startParsing(SelectedYear, SelectedMonth, SelectedDay);
            } else if(IsParsePageBased)
            {
                process.startParsing(SelectedPage);
            }
            
            isWebParsing = true;
            this.addonList = process.getAddonList();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarDialogWindow.Close();
            refreshAddonTabControl();
        }
        #endregion        

        
        //Programozottan a háttérből frissíti az addonTabControl-t, anélkül, hogy arra az UI-n rákattintottak volna.        
        private void refreshAddonTabControl()
        {        
            if(lastSelectedTabItem != null)
            {
                this.addonListForm = new AddonListForm(addonList, lastSelectedTabItem.Name);
                lastSelectedTabItem.Content = addonListForm;
                addonTabs.UpdateLayout();
            }                        
        }

        #region NotifyPropertyChanged
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region getters/setters
        public string FormTitle { get => formTitle; set => formTitle = value; }
        public Color LeftColor { get => leftColor; set => leftColor = value; }
        public Color RightColor { get => rightColor; set => rightColor = value; }
        public int SelectedYear { get => selectedYear;
            //set => selectedYear = value;
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    NotifyPropertyChanged("SelectedYear");
                }
            }
        }        
        public int SelectedMonth { get => selectedMonth;
            //set => selectedMonth = value; 
            set
            {
                if (selectedMonth != value)
                {
                    selectedMonth = value;
                    NotifyPropertyChanged("SelectedMonth");
                }
            }
        }
        public int SelectedPage { get => selectedPage;
            set
            {
                if (selectedPage != value)
                {
                    selectedPage = value;
                    NotifyPropertyChanged("SelectedPage");
                }
            }
        }
        public int SelectedDay { get => selectedDay;
            //set => selectedDay = value;
            set
            {
                if (selectedDay != value)
                {
                    selectedDay = value;
                    NotifyPropertyChanged("SelectedDay");
                }
            }
        }
        public bool IsParseDateBased
        {
            get => parserType == PAGE_PARSER_TYPES.DATE_BASED;
        }
        public bool IsParsePageBased
        {
            get => parserType == PAGE_PARSER_TYPES.PAGE_BASED;               
        }        
        #endregion

    }
}
