using Inventory.Core.Dialogs;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using Prism.Navigation;

namespace Inventory.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);

                if (value)
                {
                    var progressDialog = new ProgressDialog();

                    DialogHost.Show(progressDialog, RegionNames.DialogRegion).ConfigureAwait(false);
                }
                else
                {
                    DialogHost.Close(RegionNames.DialogRegion);
                }
            }
        }

        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }

        public static async void ShowSuccessDialog()
        {
            var successDialog = new SuccessDialog();

            await DialogHost.Show(successDialog, RegionNames.DialogRegion);
        }

        public static async void ShowSuccessDialog(string message)
        {
            var successDialog = new SuccessDialog(message: message);

            await DialogHost.Show(successDialog, RegionNames.DialogRegion);
        }

        public static async void ShowSuccessDialog(string title, string message)
        {
            var successDialog = new SuccessDialog(title: title, message: message);

            await DialogHost.Show(successDialog, RegionNames.DialogRegion);
        }

        public static async void ShowErrorDialog()
        {
            var errorDialog = new ErrorDialog();

            await DialogHost.Show(errorDialog, RegionNames.DialogRegion);
        }

        public static async void ShowErrorDialog(string message)
        {
            var errorDialog = new ErrorDialog(message: message);

            await DialogHost.Show(errorDialog, RegionNames.DialogRegion);
        }

        public static async void ShowErrorDialog(string title, string message)
        {
            var errorDialog = new ErrorDialog(title: title, message: message);

            await DialogHost.Show(errorDialog, RegionNames.DialogRegion);
        }
    }
}
