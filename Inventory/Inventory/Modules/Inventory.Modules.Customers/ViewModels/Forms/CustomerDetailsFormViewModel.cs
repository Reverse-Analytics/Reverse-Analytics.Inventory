using Inventory.Core;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerDetailsFormViewModel : ViewModelBase
    {
        public DelegateCommand CancelCommand { get; }

        public CustomerDetailsFormViewModel()
        {
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnCancel()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
