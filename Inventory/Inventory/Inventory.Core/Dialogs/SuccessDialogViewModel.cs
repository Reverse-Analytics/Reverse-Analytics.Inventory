using Prism.Services.Dialogs;
using System;

namespace Inventory.Core.Dialogs
{
    public class SuccessDialogViewModel : IDialogAware
    {
        public string Title => "Success";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
