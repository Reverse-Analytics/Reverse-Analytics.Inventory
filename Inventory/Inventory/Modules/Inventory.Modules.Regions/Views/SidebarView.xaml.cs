using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Inventory.Modules.Regions.Views
{
    /// <summary>
    /// Interaction logic for SidebarView.xaml
    /// </summary>
    public partial class SidebarView : UserControl
    {
        private bool productionChildVisible = false;
        private bool salesChildVisible = false;
        private bool suppliesChildVisible = false;

        Dictionary<string, List<Button>> navigationButtons = new Dictionary<string, List<Button>>();
        Dictionary<string, Path> navigationButtonIcons = new Dictionary<string, Path>();

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public SidebarView()
        {
            InitializeComponent();

            productionsChildContent.Height = 0;
            salesChildContent.Height = 0;
            suppliesChildContent.Height = 0;

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
            navigationButtons.Add(salesButton.Name, new List<Button>
            {
                salesButton,
                salesListButton,
                saleDebtsButton
            });
            navigationButtons.Add(customersButton.Name, new List<Button>
            {
                customersButton
            });
            navigationButtons.Add(suppliesButton.Name, new List<Button>
            {
                suppliesButton,
                suppliesListButton,
                supplyDebtsButton
            });
            navigationButtons.Add(suppliersButton.Name, new List<Button>
            {
                suppliersButton
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
            navigationButtonIcons.Add(salesButton.Name, salesIcon);
            navigationButtonIcons.Add(customersButton.Name, customersIcon);
            navigationButtonIcons.Add(suppliesButton.Name, suppliesIcon);
            navigationButtonIcons.Add(suppliersButton.Name, suppliersIcon);
            navigationButtonIcons.Add(inventoryButton.Name, inventoriesIcon);
            navigationButtonIcons.Add(reportsButton.Name, reportsIcon);
            navigationButtonIcons.Add(settingsButton.Name, settingsIcon);
        }

        private void ResetButtonsToDefaultStateExceptSelected(string selectedButtonName)
        {
            var allButtonsExceptSelected = navigationButtons.Where(nb => nb.Key != selectedButtonName).Select(nb => nb.Value).ToList();
            foreach (var buttonsList in allButtonsExceptSelected)
            {
                foreach (var button in buttonsList)
                {
                    button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
                }
            }

            var allButtonIconsExceptSelected = navigationButtonIcons.Where(nb => nb.Key != selectedButtonName).Select(nb => nb.Value).ToList();
            foreach (var icon in allButtonIconsExceptSelected)
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

            foreach (var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(productionButton.Name);
        }

        private void SalesInnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (sender as Button);

            salesButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            salesIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));


            var allButtonsExceptSelected = navigationButtons[salesButton.Name].Where(b => (b.Name != selectedButton.Name && b.Name != salesButton.Name));

            foreach (var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(salesButton.Name);
        }

        private void SuppliesInnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (sender as Button);

            suppliesButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            suppliesIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            var allButtonsExceptSelected = navigationButtons[suppliesButton.Name].Where(b => (b.Name != selectedButton.Name && b.Name != suppliesButton.Name));

            foreach (var button in allButtonsExceptSelected)
            {
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 46, 46));
            }

            ResetButtonsToDefaultStateExceptSelected(suppliesButton.Name);
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

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            if (salesChildVisible)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
                salesChildContent.BeginAnimation(HeightProperty, heightAnimation);

                salesCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                salesChildInnerContent.Measure(new Size(salesChildInnerContent.MaxWidth, salesChildInnerContent.MaxHeight));
                DoubleAnimation heightAnimation = new DoubleAnimation(0, salesChildInnerContent.DesiredSize.Height, _openCloseDuration);
                salesChildContent.BeginAnimation(HeightProperty, heightAnimation);

                salesCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            salesChildVisible = !salesChildVisible;
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            customersButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            customersIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(customersButton.Name);
        }

        private void SuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            if (suppliesChildVisible)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
                suppliesChildContent.BeginAnimation(HeightProperty, heightAnimation);

                suppliesCaret.Icon = FontAwesome.Sharp.IconChar.CaretRight;
            }
            else
            {
                suppliesChildInnerContent.Measure(new Size(suppliesChildInnerContent.MaxWidth, suppliesChildInnerContent.MaxHeight));
                DoubleAnimation heightAnimation = new DoubleAnimation(0, suppliesChildInnerContent.DesiredSize.Height, _openCloseDuration);
                suppliesChildContent.BeginAnimation(HeightProperty, heightAnimation);

                suppliesCaret.Icon = FontAwesome.Sharp.IconChar.CaretDown;
            }

            suppliesChildVisible = !suppliesChildVisible;
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            suppliersButton.Foreground = new SolidColorBrush(Color.FromRgb(27, 130, 224));
            suppliersIcon.Fill = new SolidColorBrush(Color.FromRgb(27, 130, 224));

            ResetButtonsToDefaultStateExceptSelected(suppliersButton.Name);
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
