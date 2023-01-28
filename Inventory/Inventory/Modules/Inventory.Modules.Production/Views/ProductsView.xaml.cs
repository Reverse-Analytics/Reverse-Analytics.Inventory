using MaterialDesignThemes.Wpf;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.Modules.Production.Views
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        PopupBox selectedPopupBox;
        public ProductsView()
        {
            InitializeComponent();
        }

        private void PopupBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var popupBox = sender as PopupBox;

            selectedPopupBox = popupBox;

            popupBox.IsPopupOpen = true;
        }

        private void ProductsDataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (selectedPopupBox != null)
            {
                selectedPopupBox.Focus();
                selectedPopupBox.IsPopupOpen = true;
            }
        }
    }
}
