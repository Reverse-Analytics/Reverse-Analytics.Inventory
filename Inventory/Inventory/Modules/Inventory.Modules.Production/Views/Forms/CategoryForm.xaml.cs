using Inventory.Modules.Production.ViewModels.Forms;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory.Modules.Production.Views.CategoryDialogs
{
    /// <summary>
    /// Interaction logic for AddCategoryDialogView.xaml
    /// </summary>
    public partial class CategoryForm : UserControl
    {
        public CategoryForm()
        {
            InitializeComponent();

            PreviewKeyDown += new KeyEventHandler(HandleKeyPress);
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            var vm = DataContext as CategoryFormViewModel;

            if (e.Key == Key.Escape)
            {
                vm?.CloseCommand?.Execute();
            }
            else if (e.Key == Key.Enter && (vm?.SaveCommand?.CanExecute() ?? false))
            {
                vm.SaveCommand.Execute();
            }
        }
    }
}
