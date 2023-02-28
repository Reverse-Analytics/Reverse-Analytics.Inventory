using Inventory.Helpers.Extensions;
using System.Windows.Controls;

namespace Inventory.Modules.Sales.Views
{
    /// <summary>
    /// Interaction logic for SaleView.xaml
    /// </summary>
    public partial class SalesView : UserControl
    {
        public SalesView()
        {
            InitializeComponent();

            salesDataGrid.CellRenderers.Remove("TemplateExt");
            salesDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }
    }
}
