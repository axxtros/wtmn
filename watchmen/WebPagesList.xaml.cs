using System;
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
using watchmen.processconfigs;
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
            interfaces.ConfigInterface config = null;
            UserControl currentUserCtrl = null;            
            string buttonID = sideMenuButton.Name;
            switch (buttonID)
            {
                case "armaholic":
                    config = new ArmaholicConfig();                    
                    break;
                case "ets2lt":
                    config = new Ets2LtConfig();                    
                    break;
                case "skidrowreloded":
                    config = new SkidrowrelodedConfig();
                    break;
            }
            if(config != null)
            {
                currentUserCtrl = new webpageForms.WebPageForm(
                        config.getProcess().getParserName(),
                        config.getFirstColor(),
                        config.getSecondColor(),
                        config.getAddonTypes(),
                        config.getProcess(),
                        config.getParsingType());
                mainWindow.contentPanel.Children.Add(currentUserCtrl);
            }
        }
    }
}
