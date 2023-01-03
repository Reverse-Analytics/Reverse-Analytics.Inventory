using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public SidebarView()
        {
            InitializeComponent();

            productionsChildContent.Height = 0;
            customersChildContent.Height = 0;
            suppliersChildContent.Height = 0;
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductionButton_Click(object sender, RoutedEventArgs e)
        {
            if (productionChildVisible)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
                productionsChildContent.BeginAnimation(HeightProperty, heightAnimation);
                productionCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
                // productionsIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            }
            else
            {
                productionsChildInnerContent.Measure(new Size(productionsChildInnerContent.MaxWidth, productionsChildInnerContent.MaxHeight));
                DoubleAnimation heightAnimation = new DoubleAnimation(0, productionsChildInnerContent.DesiredSize.Height, _openCloseDuration);
                productionsChildContent.BeginAnimation(HeightProperty, heightAnimation);
                productionCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
                // productionsIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            }

            productionChildVisible = !productionChildVisible;
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            if (customersChildVisible)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
                customersChildContent.BeginAnimation(HeightProperty, heightAnimation);
                
                customersCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                customersChildInnerContent.Measure(new Size(customersChildInnerContent.MaxWidth, customersChildInnerContent.MaxHeight));
                DoubleAnimation heightAnimation = new DoubleAnimation(0, customersChildInnerContent.DesiredSize.Height, _openCloseDuration);
                customersChildContent.BeginAnimation(HeightProperty, heightAnimation);
                
                customersCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            customersChildVisible = !customersChildVisible;
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            if (suppliersChildVisible)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
                suppliersChildContent.BeginAnimation(HeightProperty, heightAnimation);

                suppliersCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                suppliersChildInnerContent.Measure(new Size(suppliersChildInnerContent.MaxWidth, suppliersChildInnerContent.MaxHeight));
                DoubleAnimation heightAnimation = new DoubleAnimation(0, suppliersChildInnerContent.DesiredSize.Height, _openCloseDuration);
                suppliersChildContent.BeginAnimation(HeightProperty, heightAnimation);

                suppliersCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            suppliersChildVisible = !suppliersChildVisible;
        }

        private void CatalogButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SuppliesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SupplierDebtsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
