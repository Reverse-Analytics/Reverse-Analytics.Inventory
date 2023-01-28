using Inventory.Core.Dialogs;
using Inventory.Modules.Production.ViewModels.Forms;
using Inventory.Modules.Production.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels
{
    public class ProductsViewModel : BindableBase, IRegionMemberLifetime
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

        public bool KeepAlive => false;

        public ProductsViewModel(ICategorySerivce categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;

            Products = new List<ProductDto>();
            Categories = new ObservableCollection<ProductCategoryDto>();
            FilteredProducts = new ObservableCollection<ProductDto>();

            AddCommand = new DelegateCommand(OnAddProduct);
            EditCommand = new DelegateCommand<ProductDto>(OnUpdateProduct);
            DeleteCommand = new DelegateCommand<ProductDto>(OnDeleteProduct);
            ArchiveCommand = new DelegateCommand<ProductDto>(OnArchiveProduct);
            ShowDetailsCommand = new DelegateCommand<ProductDto>(OnShowDetails);

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

        private async void OnAddProduct()
        {
            var view = new ProductForm()
            {
                DataContext = new ProductFormViewModel(_categoryService)
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result is ProductForCreateDto)
            {
                var product = result as ProductForCreateDto;

                var createdProduct = await _productService.CreateProductAsync(product);

                Products.Add(createdProduct);
                FilteredProducts.Add(createdProduct);
            }
        }

        private async void OnUpdateProduct(ProductDto product)
        {
            var view = new ProductForm()
            {
                DataContext = new ProductFormViewModel(_categoryService, product)
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result is ProductForUpdateDto)
            {
                var productToUpdate = result as ProductForUpdateDto;

                await _productService.UpdateProductAsync(productToUpdate);
            }
        }

        private async void OnDeleteProduct(ProductDto product)
        {
            if (product is null)
            {
                return;
            }

            var view = new ConfirmationDialog("Confirm action.", $"Are you sure you want to delete: {product.ProductName}?");

            var result = await DialogHost.Show(view, "RootDialog");
            bool? isConfirm = result as bool?;

            if (isConfirm.HasValue && isConfirm.Value)
            {
                await _productService.DeleteProductAsync(product.Id);

                FilteredProducts.Remove(product);
            }
        }

        private void OnArchiveProduct(ProductDto selectedProduct)
        {

        }

        private void OnShowDetails(ProductDto selectedProduct)
        {

        }
    }
}
