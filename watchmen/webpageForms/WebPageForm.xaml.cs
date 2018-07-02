using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using wspd.commons.entity;
using wspd.interfaces;

namespace watchmen.webpageForms
{
    /// <summary>
    /// Interaction logic for WebPageForm.xaml
    /// </summary>
    public partial class WebPageForm : UserControl
    {
        private readonly int TABITEM_HEIGHT = 30;

        private String formTitle;
        private Color leftColor;
        private Color rightColor;
        private int selectedYear;
        private int selectedMonth;
        private int selectedDay;
        private ADDON_TYPES[] addonTypes;
        private List<AddonEntity> addonList;
        private ProcessInterface process;
        private ProgressBarDialogWindow progressBarDialogWindow;
        private BackgroundWorker backgroundWorker; //http://www.codescratcher.com/wpf/progress-bar-in-wpf-backgroundworker/
        public event PropertyChangedEventHandler PropertyChanged;        

        public WebPageForm(String webPageTitle, Color gardientLeftColor, Color gardientRightColor, ADDON_TYPES[] addonTypes, ProcessInterface processObject)
        {
            InitializeComponent();
            this.addonList = new List<AddonEntity>();
            this.addonTypes = addonTypes;
            this.process = processObject;
            init(webPageTitle, gardientLeftColor, gardientRightColor);
            this.DataContext = this;
        }

        //https://stackoverflow.com/questions/5824600/databind-from-xaml-to-code-behind
        private void init(String webPageTitle, Color gardientLeftColor, Color gardientRightColor)
        {
            initUI(webPageTitle, gardientLeftColor, gardientRightColor);
            initYearComboBox();
            initMonthComboBox();
            initDayComboBox();
            initAddonTabs();
            initBackgroundWorker();             
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
            int currentYear = DateTime.Now.Year;
            for (int minusYear = 0; minusYear != 10; minusYear++)
            {
                yearComboBox.Items.Add(currentYear - minusYear);
            }
            yearComboBox.SelectedIndex = 0;            
            SelectedYear = (int)yearComboBox.SelectedItem;            
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
            if (!dayComboBox.Items.IsEmpty)
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
        #endregion        

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
            process.startParsing(SelectedYear, selectedMonth, selectedDay);
            this.addonList = process.getAddonList();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarDialogWindow.Close();
            //refreshAddonTabControl();
        }
        #endregion


        #region getters/setters
        public string FormTitle { get => formTitle; set => formTitle = value; }
        public Color LeftColor { get => leftColor; set => leftColor = value; }
        public Color RightColor { get => rightColor; set => rightColor = value; }
        public int SelectedYear { get => selectedYear; set => selectedYear = value; }        
        public int SelectedMonth { get => selectedMonth; set => selectedMonth = value; }
        public int SelectedDay { get => selectedDay; set => selectedDay = value; }
        #endregion
        

    }
}
