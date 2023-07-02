using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class CategoryDetailsFormViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _numberOfItems;
        public int NumberOfItems
        {
            get => _numberOfItems;
            set => SetProperty(ref _numberOfItems, value);
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public ICommand CloseCommand { get; }

        public CategoryDetailsFormViewModel(ProductCategory category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            Id = category.Id;
            CategoryName = category.CategoryName;
            NumberOfItems = category.Id;

            CloseCommand = new DelegateCommand(OnClose);
        }

        public void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
