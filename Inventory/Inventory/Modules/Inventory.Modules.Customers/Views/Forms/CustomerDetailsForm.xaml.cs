using Inventory.Modules.Customers.ViewModels.Forms;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory.Modules.Customers.Views.Forms
{
    /// <summary>
    /// Interaction logic for CustomerDetailsForm.xaml
    /// </summary>
    public partial class CustomerDetailsForm : UserControl
    {
        public CustomerDetailsForm()
        {
            InitializeComponent();

            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var vm = DataContext as CustomerDetailsFormViewModel;

                vm?.CancelCommand?.Execute();
            }
        }
    }
}
