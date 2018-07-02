﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using watchmen.commonForms;
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
        private ADDON_TYPES[] addonTypes;
        private List<AddonEntity> addonList;
        private ProcessInterface process;
        private ProgressBarDialogWindow progressBarDialogWindow;
        private BackgroundWorker backgroundWorker; //http://www.codescratcher.com/wpf/progress-bar-in-wpf-backgroundworker/
        public event PropertyChangedEventHandler PropertyChanged;   //https://www.codeproject.com/Articles/301678/Step-by-Step-WPF-Data-Binding-with-Comboboxes   
        private AddonListForm addonListForm;
        private TabItem lastSelectedTabItem;

        public WebPageForm(String webPageTitle, Color gardientLeftColor, Color gardientRightColor, ADDON_TYPES[] addonTypes, ProcessInterface processObject)
        {
            InitializeComponent();
            this.addonList = new List<AddonEntity>();
            this.addonTypes = addonTypes;
            this.process = processObject;
            this.now = DateTime.Now;
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
            process.startParsing(SelectedYear, SelectedMonth, SelectedDay);
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
        #endregion

        #region NotifyPropertyChanged
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        
    }
}
