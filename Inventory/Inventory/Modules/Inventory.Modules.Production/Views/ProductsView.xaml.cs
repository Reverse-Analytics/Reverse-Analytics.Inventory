using Inventory.Helpers.Extensions;
using Inventory.Helpers.Extensions.SearchHelpers;
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
        public ProductsView()
        {
            InitializeComponent();

            productsDataGrid.SearchHelper = new ProductSearchHelper(productsDataGrid);
            productsDataGrid.SearchHelper.AllowCaseSensitiveSearch = true;
            productsDataGrid.SearchHelper.SearchType = SearchType.Contains;
            productsDataGrid.SearchHelper.AllowFiltering = false;

            searchTextBox.TextChanged += TextBox_TextChanged;

            productsDataGrid.CellRenderers.Remove("TemplateExt");
            productsDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            productsDataGrid.SearchHelper.Search(searchTextBox.Text);
        }
    }
}
