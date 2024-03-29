﻿using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Cells;
using System.Windows;

namespace Inventory.Helpers.Extensions
{
    public class MoreOptionsTemplateColumnExtension : GridTemplateColumn
    {
        public MoreOptionsTemplateColumnExtension()
        {
            SetCellType("TemplateExt");
        }
    }

    public class GridCellTemplateExtension : GridCellTemplateRenderer
    {
        // This method will set the focus to the clicked cell
        // instead of row itself
        protected override void SetFocus(FrameworkElement uiElement, bool needToFocus)
        {
            if (!needToFocus)
                DataGrid.Focus();
        }
    }
}
