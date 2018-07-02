using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using wspd.commons.entity;

namespace watchmen.commonForms
{
    /// <summary>
    /// Interaction logic for AddonListForm.xaml
    /// </summary>
    public partial class AddonListForm : UserControl
    {        
        private List<AddonEntity> addonList;
        private string addonTypeName;
        private IEnumerable<AddonEntity> addonQuery;

        public AddonListForm(List<AddonEntity> addonList, string addonTypeName)
        {
            InitializeComponent();
            this.addonList = addonList;
            this.addonTypeName = addonTypeName;
            init();
        }

        private void init()
        {
            if(this.addonList != null && addonList.Any())
            {
                addonQuery = addonList.Where(list => list.Type.ToString() == addonTypeName);
                if (addonQuery != null)
                {
                    addonQuery = addonQuery.OrderByDescending(item => item.Year).ThenByDescending(item => item.Month).ThenByDescending(item => item.Day);
                    int index = 1;
                    foreach (AddonEntity item in addonQuery)
                    {
                        item.ListIndex = index++;
                    }
                    addonDataGrid.ItemsSource = addonQuery;
                }
            }            
        }

        private void addonHyperlinkClickEvent(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}
