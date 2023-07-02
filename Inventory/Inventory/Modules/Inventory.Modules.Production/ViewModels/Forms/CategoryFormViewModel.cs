using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Diagnostics;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class CategoryFormViewModel : ViewModelBase
    {
        #region Properties

        private readonly bool isEditingMode = false;
        private readonly int id;

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

        #endregion

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CloseCommand { get; }

        public CategoryFormViewModel()
        {
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CloseCommand = new DelegateCommand(OnClose);

            Title = "New Category";
        }

        public CategoryFormViewModel(ProductCategory categoryToUpdate)
            : this()
        {
            ArgumentNullException.ThrowIfNull(categoryToUpdate, nameof(categoryToUpdate));

            CategoryName = categoryToUpdate.CategoryName;
            id = categoryToUpdate.Id;
            isEditingMode = !string.IsNullOrEmpty(categoryToUpdate.CategoryName);

            Title = "Update Category";
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        private void OnSave()
        {
            if (isEditingMode)
            {
                var category = new ProductCategory(id, CategoryName);
                DialogHost.Close("RootDialog", category);
            }
            else
            {
                var category = new ProductCategory(CategoryName);
                DialogHost.Close("RootDialog", category);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(CategoryName);
        }

        private void OnClose()
        {
            DialogHost.Close("RootDialog");
        }

    }
}
