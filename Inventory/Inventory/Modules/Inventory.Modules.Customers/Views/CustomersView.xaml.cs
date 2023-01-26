using MaterialDesignThemes.Wpf;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Cells;
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

namespace Inventory.Modules.Customers.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        PopupBox selectedPopupBox;
        public CustomersView()
        {
            InitializeComponent();
        }

        private void PopupBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var popupBox = sender as PopupBox;

            selectedPopupBox = popupBox;

            popupBox.IsPopupOpen = true;
        }

        private void customersDataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            if (selectedPopupBox != null)
            {
                selectedPopupBox.Focus();
                selectedPopupBox.IsPopupOpen= true;
            }
        }
    }
}
