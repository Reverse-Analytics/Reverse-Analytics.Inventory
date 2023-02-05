using Inventory.Modules.Production.ViewModels.Forms;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory.Modules.Production.Views.Forms
{
    /// <summary>
    /// Interaction logic for ProductDetailsForm.xaml
    /// </summary>
    public partial class ProductDetailsForm : UserControl
    {
        public ProductDetailsForm()
        {
            InitializeComponent();

            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var vm = DataContext as ProductDetailsFormViewModel;

                vm?.CloseCommand?.Execute();
            }
        }
    }
}
