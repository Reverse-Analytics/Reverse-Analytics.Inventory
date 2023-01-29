using Inventory.Controls;
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
            categoriesDataGrid.SearchHelper = new SearchHelperExt(this.categoriesDataGrid);
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

    public class SearchHelperExt : SearchHelper
    {
        public SearchHelperExt(SfDataGrid datagrid) : base(datagrid)
        {
        }

        protected override bool SearchCell(DataColumnBase column, object record, bool ApplySearchHighlightBrush)
        {
            if (column == null)
                return false;
            if (column.GridColumn.MappingName == "CategoryName")
            {
                return base.SearchCell(column, record, ApplySearchHighlightBrush);
            }
            else
                return false;
        }

        protected override bool FilterRecords(object dataRow)
        {
            if (string.IsNullOrEmpty(SearchText))
                return true;

            if (this.Provider == null)
                Provider = this.DataGrid.View.GetPropertyAccessProvider();


            if (MatchSearchTextOpimized("CategoryName", dataRow))
                return true;

            return false;
        }

        protected virtual bool MatchSearchTextOpimized(string mappingName, object record)
        {
            if (this.DataGrid.View == null || string.IsNullOrEmpty(SearchText))
                return false;

            var cellvalue = Provider.GetFormattedValue(record, mappingName);
            if (this.SearchType == SearchType.Contains)
            {
                if (!AllowCaseSensitiveSearch)
                    return cellvalue != null && cellvalue.ToString().ToLower().Contains(SearchText.ToString().ToLower());
                else
                    return cellvalue != null && cellvalue.ToString().Contains(SearchText.ToString());
            }
            else if (this.SearchType == SearchType.StartsWith)
            {
                if (!AllowCaseSensitiveSearch)
                    return cellvalue != null && cellvalue.ToString().ToLower().StartsWith(SearchText.ToString().ToLower());
                else
                    return cellvalue != null && cellvalue.ToString().StartsWith(SearchText.ToString());
            }
            else
            {
                if (!AllowCaseSensitiveSearch)
                    return cellvalue != null && cellvalue.ToString().ToLower().EndsWith(SearchText.ToString().ToLower());
                else
                    return cellvalue != null && cellvalue.ToString().EndsWith(SearchText.ToString());
            }
        }
    }
}
