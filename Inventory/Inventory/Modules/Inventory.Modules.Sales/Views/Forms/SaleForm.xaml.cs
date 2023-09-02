using Inventory.Modules.Sales.ViewModels.Forms;
using System.Windows.Controls;

namespace Inventory.Modules.Sales.Views.Forms
{
    /// <summary>
    /// Interaction logic for SaleForm.xaml
    /// </summary>
    public partial class SaleForm : UserControl
    {
        public SaleForm()
        {
            InitializeComponent();
        }

        private void ComboBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var vm = (SaleFormViewModel)DataContext;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                vm?.OnAddProduct();
            }
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var vm = (SaleFormViewModel)DataContext;

            if (e.Key == System.Windows.Input.Key.Escape)
            {
                vm.CancelCommand.Execute();
            }
        }

        private void DatePicker_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Up)
            {
                this.customersComboBox.Focus();
            }
        }

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var vm = (SaleFormViewModel)DataContext;

                (sender as Button).Command.Execute(null);
            }
        }
    }
}
