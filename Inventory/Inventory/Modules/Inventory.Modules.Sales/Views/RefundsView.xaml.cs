using Inventory.Helpers.Extensions;
using System.Windows.Controls;

namespace Inventory.Modules.Sales.Views
{
    /// <summary>
    /// Interaction logic for RefundsView.xaml
    /// </summary>
    public partial class RefundsView : UserControl
    {
        public RefundsView()
        {
            InitializeComponent();

            refundsDataGrid.CellRenderers.Remove("TemplateExt");
            refundsDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }
    }
}
