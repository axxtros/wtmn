using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
using wspd;

namespace watchmen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            //System.Threading.Thread.Sleep(3000);        //splashscreen delay!
            InitializeComponent();
            this.Title = utils.StaticResources.PROGRAM_FULL_TITLE();
            initToolbar();
            loadWebpageList();               
        }

        private void initToolbar()
        {
            //exitButton.Content = Properties.Resources.exit;            
        }

        private void loadWebpageList()
        {
            webPagesListPanel.Children.Add(new WebPagesList(this));
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
