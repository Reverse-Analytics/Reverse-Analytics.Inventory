using Syncfusion.UI.Xaml.Grid;

namespace Inventory.Helpers.Extensions
{
    public class SearchHelperExtension : SearchHelper
    {
        public SearchHelperExtension(SfDataGrid datagrid) : base(datagrid)
        {
        }

        protected override bool SearchCell(DataColumnBase column, object record, bool ApplySearchHighlightBrush)
        {
            // to get the columnIndex of selected row of the SfDataGrid
            var colIndex = DataGrid.SelectionController?.CurrentCellManager?.CurrentCell?.ColumnIndex ?? -1;

            if (colIndex == -1) { return false; }

            // to get the mapping name of based on the column index 
            var mapName = DataGrid.Columns[colIndex].MappingName;
            // based on the mapping name, the particular searching will be enabled other columns the searching will be excluded. 
            if (column.GridColumn.MappingName == mapName)
                return base.SearchCell(column, record, ApplySearchHighlightBrush);
            return false;
        }
    }
}
