using Inventory.Controls;
using System.Windows.Controls;

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

            productsDataGrid.CellRenderers.Remove("TemplateExt");
            productsDataGrid.CellRenderers.Add("TemplateExt", new GridCellTemplateExtension());
        }
    }
}
