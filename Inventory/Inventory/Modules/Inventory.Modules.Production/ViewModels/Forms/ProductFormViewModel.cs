using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class ProductFormViewModel : BindableBase
    {
        private readonly ICategorySerivce _categoryService;
        private readonly bool IsEditingMode;
        private readonly int productId;

        private ProductCategoryDto _selectedCategory;
        public ProductCategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public int categoryId;

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private double _volume;
        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        private double _weight;
        public double Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private decimal _incomePrice;
        public decimal IncomePrice
        {
            get => _incomePrice;
            set => SetProperty(ref _incomePrice, value);
        }

        private decimal _salePrice;
        public decimal SalePrice
        {
            get => _salePrice;
            set => SetProperty(ref _salePrice, value);
        }

        public ObservableCollection<ProductCategoryDto> Categories { get; }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ProductFormViewModel(ICategorySerivce categorySerivce)
        {
            _categoryService = categorySerivce;

            Categories = new ObservableCollection<ProductCategoryDto>();

            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CancelCommand = new DelegateCommand(OnCancel);

            LoadCategories();
        }

        public ProductFormViewModel(ICategorySerivce categorySerivce, ProductDto productToUpdate)
            : this(categorySerivce)
        {
            IsEditingMode = productToUpdate.ProductName is not null;

            productId = productToUpdate.Id;
            Name = productToUpdate.ProductName;
            Code = productToUpdate.ProductCode;
            Volume = productToUpdate.Volume;
            Weight = productToUpdate.Weight;
            IncomePrice = productToUpdate.SupplyPrice;
            SalePrice = productToUpdate.SalePrice;
            categoryId = productToUpdate.CategoryId;
        }

        public async void LoadCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            var selectedCategory = categories.FirstOrDefault(c => c.Id == categoryId);

            if (selectedCategory != null)
            {
                SelectedCategory = selectedCategory;
            }

            Categories.AddRange(categories);
        }

        public void OnSave()
        {
            if (IsEditingMode)
            {
                var productToUpdate = new ProductForUpdateDto()
                {
                    Id = productId,
                    ProductName = Name,
                    ProductCode = Code,
                    Volume = Volume,
                    Weight = Weight,
                    SupplyPrice = IncomePrice,
                    SalePrice = SalePrice,
                    CategoryId = SelectedCategory.Id
                };

                DialogHost.Close("RootDialog", productToUpdate);
            }
            else
            {
                var product = new ProductForCreateDto()
                {
                    ProductName = Name,
                    ProductCode = Code,
                    Volume = Volume,
                    Weight = Weight,
                    SupplyPrice = IncomePrice,
                    SalePrice = SalePrice,
                    CategoryId = SelectedCategory.Id
                };

                DialogHost.Close("RootDialog", product);
            }
        }

        private bool CanSave()
        {
            return SelectedCategory is not null &&
                !string.IsNullOrEmpty(Code) &&
                !string.IsNullOrEmpty(Name);
        }

        public void OnCancel()
        {
            DialogHost.Close("RootDialog");
        }
    }
}
