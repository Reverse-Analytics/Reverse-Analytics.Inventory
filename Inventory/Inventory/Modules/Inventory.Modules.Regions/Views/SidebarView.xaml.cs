using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        Dictionary<string, List<Button>> navigationButtons = new Dictionary<string, List<Button>>();
        Dictionary<string, Path> navigationButtonIcons = new Dictionary<string, Path>();

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public SidebarView()
        {
            InitializeComponent();

            productionsChildContent.Height = 0;
            customersChildContent.Height = 0;
            suppliersChildContent.Height = 0;

            InitializeNavigationButtonsDictionary();
            InitializeNavigationButtonIconsDictionary();
        }

        private void InitializeNavigationButtonsDictionary()
        {
            navigationButtons.Add(dashboardButton.Name, new List<Button>
            {
                dashboardButton
            });
            navigationButtons.Add(productionButton.Name, new List<Button>
            {
                productionButton,
                catalogButton,
                categoriesButton
            });
            navigationButtons.Add(customersButton.Name, new List<Button> 
            {
                customersButton,
                customersListButton,
                customerDebtsButton
            });
            navigationButtons.Add(salesButton.Name, new List<Button> 
            {
                salesButton
            });
            navigationButtons.Add(suppliersButton.Name, new List<Button> 
            {
                suppliersButton,
                suppliersListButton,
                supplierDebtsButton
            });
            navigationButtons.Add(suppliesButton.Name, new List<Button> 
            {
                suppliesButton
            });
            navigationButtons.Add(inventoryButton.Name, new List<Button> 
            {
                inventoryButton
            });
            navigationButtons.Add(reportsButton.Name, new List<Button> 
            {
                reportsButton
            });
            navigationButtons.Add(settingsButton.Name, new List<Button> 
            {
                settingsButton
            });
        }

        private void InitializeNavigationButtonIconsDictionary()
        {
            navigationButtonIcons.Add(dashboardButton.Name, dashboardIcon);
            navigationButtonIcons.Add(productionButton.Name, productionsIcon);
            navigationButtonIcons.Add(customersButton.Name, customersIcon);
            navigationButtonIcons.Add(salesButton.Name, salesIcon);
            navigationButtonIcons.Add(suppliersButton.Name, suppliersIcon);
            navigationButtonIcons.Add(suppliesButton.Name, suppliesIcon);
            navigationButtonIcons.Add(inventoryButton.Name, inventoriesIcon);
            navigationButtonIcons.Add(reportsButton.Name, reportsIcon);
            navigationButtonIcons.Add(settingsButton.Name, settingsIcon);
        }

        private void ResetButtonsToDefaultStateExceptSelected(string selectedButtonName)
        {
            var allButtonsExceptSelected = navigationButtons.Where(nb => nb.Key != selectedButtonName).Select(nb => nb.Value).ToList();
            foreach(var buttonsList in allButtonsExceptSelected) 
            {
                foreach(var button in buttonsList)
                {
                    button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
                }
            }

            var allButtonIconsExceptSelected = navigationButtonIcons.Where(nb => nb.Key != selectedButtonName).Select(nb => nb.Value).ToList();
            foreach(var icon in allButtonIconsExceptSelected)
            {
                icon.Fill = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            dashboardIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            dashboardButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(dashboardButton.Name);
        }

        private void ProductionInnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (sender as Button);

            productionButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            productionsIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            var allButtonsExceptSelected = navigationButtons[productionButton.Name].Where(b => (b.Name != selectedButton.Name && b.Name != productionButton.Name));

            foreach(var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(productionButton.Name);
        }

        private void CustomersInnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (sender as Button);

            customersButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            customersIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));


            var allButtonsExceptSelected = navigationButtons[customersButton.Name].Where(b => (b.Name != selectedButton.Name && b.Name != customersButton.Name));

            foreach (var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(customersButton.Name);
        }

        private void SuppliersInnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (sender as Button);

            suppliersButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            suppliersIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            var allButtonsExceptSelected = navigationButtons[suppliersButton.Name].Where(b => (b.Name != selectedButton.Name && b.Name != suppliersButton.Name));

            foreach (var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(suppliersButton.Name);
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

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            salesButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            salesIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(salesButton.Name);
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

        private void SuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            suppliesButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            suppliesIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(suppliesButton.Name);
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            inventoriesIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            inventoryButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(inventoryButton.Name);
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            reportsIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            reportsButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(reportsButton.Name);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            settingsButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(settingsButton.Name);
        }
    }
}
