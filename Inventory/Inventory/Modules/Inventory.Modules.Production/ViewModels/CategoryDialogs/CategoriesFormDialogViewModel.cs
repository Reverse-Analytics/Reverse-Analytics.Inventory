using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System.Diagnostics;

namespace Inventory.Modules.Production.ViewModels.CategoryDialogs
{
    public class CategoriesFormDialogViewModel : ViewModelBase
    {
        private string _title = "Add Category";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _categoryName;
        public string CategoryName 
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsCategoryNameInvalid => CanSave();

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DialogClosingEventHandler ClosingCommand { get; }

        public CategoriesFormDialogViewModel()
        {
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CancelCommand = new DelegateCommand(OnCancel);
            ClosingCommand = ClosingEventHandler;
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        private void OnSave()
        {
            DialogHost.Close("RootDialog", CategoryName);
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(CategoryName);
        }

        private void OnCancel()
        {
            DialogHost.Close("RootDialog");
        }

    }
}
