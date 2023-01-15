using Inventory.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels
{
    public class ProductsViewModel : BindableBase
    {
        private readonly ICategorySerivce _categoryService;
        private readonly IProductService _productService;

        public List<ProductDto> Products { get; private set; }
        public ObservableCollection<ProductDto> FilteredProducts { get; private set; }
        public ObservableCollection<ProductCategoryDto> Categories { get; private set; }

        private ProductCategoryDto _selectedCategory;
        public ProductCategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    SetProperty(ref _selectedCategory, value);
                    FilterProductsByCategory(value);
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        public ProductsViewModel(ICategorySerivce categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;

            Products = new List<ProductDto>();
            Categories = new ObservableCollection<ProductCategoryDto>();
            FilteredProducts = new ObservableCollection<ProductDto>();

            AddCommand = new DelegateCommand(OnAddProduct);
            EditCommand = new DelegateCommand<ProductDto>(OnUpdateProduct);
            DeleteCommand = new DelegateCommand<int?>(OnDeleteProduct);
            ArchiveCommand = new DelegateCommand<int?>(OnArchiveProduct);
            ShowDetailsCommand = new DelegateCommand<int?>(OnShowDetails);

            LoadCollections();
        }

        private void FilterProductsByCategory(ProductCategoryDto category)
        {
            if (category is null)
            {
                FilteredProducts.Clear();
                FilteredProducts.AddRange(Products);

                return;
            }

            var filteredProducts = Products.Where(p => p.CategoryId == category.Id).ToList();

            FilteredProducts.Clear();
            FilteredProducts.AddRange(filteredProducts);
        }

        private async void LoadCollections()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var products = await _productService.GetProductsAsync();

            Categories.AddRange(categories);
            Products.AddRange(products);
            FilteredProducts.AddRange(products);
        }

        private void OnAddProduct()
        {

        }

        private void OnUpdateProduct(ProductDto product)
        {

        }

        private void OnDeleteProduct(int? id)
        {

        }

        private void OnArchiveProduct(int? id)
        {

        }

        private void OnShowDetails(int? id)
        {

        }
    }
}
