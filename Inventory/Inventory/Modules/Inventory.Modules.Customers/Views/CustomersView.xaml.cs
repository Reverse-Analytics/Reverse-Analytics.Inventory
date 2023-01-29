using Inventory.Controls;
using System.Windows.Controls;

namespace Inventory.Modules.Customers.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();

            customersDataGrid.CellRenderers.Remove("TemplateExt");
            customersDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }
    }
}
