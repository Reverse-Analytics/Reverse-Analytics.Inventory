using Inventory.Core;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class CategoryDetailsFormViewModel : ViewModelBase
    {
        public ICommand CloseCommand { get; }

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

        public CategoryDetailsFormViewModel(ProductCategoryDto category)
        {
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
