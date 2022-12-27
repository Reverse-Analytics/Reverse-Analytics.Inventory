using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Inventory.Modules.Regions.Views
{
    /// <summary>
    /// Interaction logic for SidebarView.xaml
    /// </summary>
    public partial class SidebarView : UserControl
    {
        private bool productionChildVisible = false;
        private bool customersChildVisible = false;
        private bool suppliersChildVisible = false;

        public SidebarView()
        {
            InitializeComponent();
        }

        private void ProductionButton_Click(object sender, RoutedEventArgs e)
        {
            if (productionChildVisible)
            {
                productionChildStackPanel.Visibility = Visibility.Collapsed;
                productionCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                productionChildStackPanel.Visibility = Visibility.Visible;
                productionCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            productionChildVisible = !productionChildVisible;
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            if (customersChildVisible)
            {
                customersChildStackPanel.Visibility = Visibility.Collapsed;
                customersCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                customersChildStackPanel.Visibility = Visibility.Visible;
                customersCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            customersChildVisible = !customersChildVisible;
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            if (suppliersChildVisible)
            {
                suppliersChildStackPanel.Visibility = Visibility.Collapsed;
                suppliersCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                suppliersChildStackPanel.Visibility = Visibility.Visible;
                suppliersCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }
            suppliersChildVisible = !suppliersChildVisible;
        }
    }
}
