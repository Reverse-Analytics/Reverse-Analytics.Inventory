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
                    var session = DialogHost.GetDialogSession(RegionNames.ProgressRegion);
                    session?.Close();

                    var progressDialog = new ProgressDialog();

                    DialogHost.Show(progressDialog, RegionNames.ProgressRegion).ConfigureAwait(false);
                }
                else
                {
                    var session = DialogHost.GetDialogSession(RegionNames.ProgressRegion);
                    session?.Close();
                }
            }
        }

        protected ViewModelBase()
        {
        }

        public virtual void Destroy()
        {

        }
    }
}
