using Inventory.Modules.Customers.ViewModels.Forms;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory.Modules.Customers.Views.Forms
{
    /// <summary>
    /// Interaction logic for CustomerForm.xaml
    /// </summary>
    public partial class CustomerForm : UserControl
    {
        public CustomerForm()
        {
            InitializeComponent();

            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var vm = DataContext as CustomerFormViewModel;

                vm?.CancelCommand?.Execute();
            }
        }
    }
}
