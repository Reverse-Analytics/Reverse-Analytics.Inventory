using Inventory.Helpers.Extensions;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            productsDataGrid.SearchHelper = new SearchHelperExtension(productsDataGrid);
            searchTextBox.TextChanged += TextBox_TextChanged;
            searchTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

            productsDataGrid.CellRenderers.Remove("TemplateExt");
            productsDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PerformSearch();
        }

        private void PerformSearch()
        {
            if (productsDataGrid.SearchHelper.SearchText.Equals(searchTextBox.Text))
                return;

            var text = searchTextBox.Text;
            productsDataGrid.SearchHelper.AllowCaseSensitiveSearch = false;
            productsDataGrid.SearchHelper.SearchType = SearchType.Contains;
            productsDataGrid.SearchHelper.AllowFiltering = true;
            productsDataGrid.SearchHelper.Search(text);
        }
    }
}
