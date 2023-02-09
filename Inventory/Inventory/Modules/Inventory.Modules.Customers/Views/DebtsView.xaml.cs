using Inventory.Helpers.Extensions;
using System.Windows.Controls;

namespace Inventory.Modules.Customers.Views
{
    /// <summary>
    /// Interaction logic for DebtsView.xaml
    /// </summary>
    public partial class DebtsView : UserControl
    {
        public DebtsView()
        {
            InitializeComponent();

            customersDataGrid.CellRenderers.Remove("TemplateExt");
            customersDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }
    }
}
