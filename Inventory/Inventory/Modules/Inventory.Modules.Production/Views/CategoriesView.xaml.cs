using Inventory.Helpers.Extensions;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory.Modules.Production.Views
{
    /// <summary>
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();

            // Setup search helper
            categoriesDataGrid.SearchHelper = new SearchHelperExtension(categoriesDataGrid);
            searchTextBox.TextChanged += TextBox_TextChanged;
            searchTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

            categoriesDataGrid.CellRenderers.Remove("TemplateExt");
            categoriesDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
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
            if (categoriesDataGrid.SearchHelper.SearchText.Equals(searchTextBox.Text))
                return;

            var text = searchTextBox.Text;
            categoriesDataGrid.SearchHelper.AllowCaseSensitiveSearch = true;
            categoriesDataGrid.SearchHelper.SearchType = SearchType.StartsWith;
            categoriesDataGrid.SearchHelper.AllowFiltering = true;
            categoriesDataGrid.SearchHelper.Search(text);
        }
    }
}
