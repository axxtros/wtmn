﻿using System;
using System.Collections.Generic;
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
//using wspd.armaholic;
using wspd.commons.entity;
using wspd.parsers;

namespace watchmen
{
    /// <summary>
    /// Interaction logic for WebPagesList.xaml
    /// </summary>
    public partial class WebPagesList : UserControl
    {
        private MainWindow mainWindow;
        private wspd.WspdEngine wspdEngine;

        public WebPagesList(MainWindow mainWindow)
        {
            InitializeComponent();
            this.wspdEngine = new wspd.WspdEngine();
            this.mainWindow = mainWindow;
            initWebPages();
        }
        
        private void initWebPages()
        {
            foreach (KeyValuePair<string, string> menuItem in wspd.utils.StaticResources.WEB_PAGE_NAME_LIST)
            {                
                sideMenuPanel.Children.Add(createSideMenuButton(menuItem.Key, menuItem.Value));
            }            
        }

        private Button createSideMenuButton(string buttunID, String buttonText)
        {
            Button resultMenuButton = new Button();
            resultMenuButton.Name = buttunID;
            resultMenuButton.Content = buttonText;
            resultMenuButton.Height = 40;
            resultMenuButton.MinWidth = 200;
            resultMenuButton.Click += (s, e) => { Side_Menu_Button_Click_Event(s, e); };
            return resultMenuButton;
        }

        /// <summary>
        /// Oldalmenü gombok eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Side_Menu_Button_Click_Event(object sender, RoutedEventArgs e)
        {
            mainWindow.contentPanel.Children.Clear();
            Button sideMenuButton = (Button) sender;            
            UserControl currentUserCtrl = null;            
            string buttonID = sideMenuButton.Name;
            switch (buttonID)
            {
                case "armaholic":
                    ADDON_TYPES[] addonTypes = {
                        ADDON_TYPES.GEAR,
                        ADDON_TYPES.PACKS,
                        ADDON_TYPES.MISCELLANEOUS,
                        ADDON_TYPES.OBJECTS,
                        ADDON_TYPES.REPLACEMENT_PACKS,
                        ADDON_TYPES.SOUNDS,
                        ADDON_TYPES.TERRAIN,
                        ADDON_TYPES.UNITS,
                        ADDON_TYPES.WEAPONS,
                        ADDON_TYPES.CHOPPERS,
                        ADDON_TYPES.HEAVY_ARMOR,
                        ADDON_TYPES.LIGHT_ARMOR,
                        ADDON_TYPES.VEHICLE_PACKS,
                        ADDON_TYPES.PLANES,
                        ADDON_TYPES.SEA,
                        ADDON_TYPES.WHEELED
                    };
                    ArmaholicParserProcess process = new ArmaholicParserProcess();
                    currentUserCtrl = new webpageForms.WebPageForm(process.getParserName(), Color.FromArgb(255, 227, 225, 214), Color.FromArgb(255, 136, 119, 93), addonTypes, process);
                    break;
                case "ets2lt":
                    //wspdEngine.webpageParser(wspd.WEBPAGE_ID.ETS2_LT);
                    break;                
            }
            if(currentUserCtrl != null)
            {
                mainWindow.contentPanel.Children.Add(currentUserCtrl);
            }
        }
    }
}
