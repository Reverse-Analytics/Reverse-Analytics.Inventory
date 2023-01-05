using Inventory.Core.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace Inventory.Modules.Production.ViewModels.CategoryDialogs
{
    public class AddCategoryDialogViewModel : ViewModelBase, IDialogAware
    {
        public string Title => "Add Category";

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand CloseDialogCommand { get; }

        public AddCategoryDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void CloseDialog()
        {

        }

        public bool CanCloseDialog()
        {
            return false;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
