using MaterialDesignThemes.Wpf;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;

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

        private void CustomersDataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (selectedPopupBox != null)
            {
                selectedPopupBox.Focus();
                selectedPopupBox.IsPopupOpen = true;
            }
        }
    }
}
