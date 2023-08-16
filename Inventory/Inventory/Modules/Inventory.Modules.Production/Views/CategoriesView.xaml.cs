using Inventory.Helpers.Extensions;
using System.Windows;
using System.Windows.Controls;

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
            categoriesDataGrid.SearchHelper.AllowFiltering = true;
            categoriesDataGrid.SearchHelper.CanHighlightSearchText = true;
            categoriesDataGrid.SearchHelper.AllowCaseSensitiveSearch = false;
            categoriesDataGrid.SearchHelper.SearchType = Syncfusion.UI.Xaml.Grid.SearchType.Contains;

            searchTextBox.TextChanged += TextBox_TextChanged;

            categoriesDataGrid.CellRenderers.Remove("TemplateExt");
            categoriesDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            categoriesDataGrid.SearchHelper.Search(searchTextBox.Text);
        }
    }
}
