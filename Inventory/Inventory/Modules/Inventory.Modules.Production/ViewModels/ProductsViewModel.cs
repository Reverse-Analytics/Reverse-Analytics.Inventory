using Inventory.Core;
using Inventory.Core.Mvvm;
using Inventory.Modules.Production.ViewModels.Forms;
using Inventory.Modules.Production.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels
{
    public class ProductsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        #region Services

        private readonly ICategorySerivce _categoryService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Collections

        public List<ProductDto> Products { get; private set; }
        public ObservableCollection<ProductDto> FilteredProducts { get; private set; }
        public ObservableCollection<ProductCategoryDto> Categories { get; private set; }

        #endregion

        #region Properties

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

        public bool KeepAlive => false;

        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        #endregion

        public ProductsViewModel(ICategorySerivce categoryService, IProductService productService, IDialogService dialogService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _dialogService = dialogService;

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

        #region Main methods

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
            try
            {
                IsBusy = true;

                var categories = await _categoryService.GetCategoriesAsync();
                var products = await _productService.GetProductsAsync();

                Categories.AddRange(categories);
                Products.AddRange(products);
                FilteredProducts.AddRange(products);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Command methods

        private async void OnAddProduct()
        {
            try
            {
                var product = await ShowAddProductForm();

                if (product is null) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    var createdProduct = await _productService.CreateProductAsync(product);

                    Products.Add(createdProduct);
                    FilteredProducts.Add(createdProduct);
                });

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnUpdateProduct(ProductDto selectedProduct)
        {
            try
            {
                if (selectedProduct is null) return;

                var productToUpdate = await ShowUpdateProductForm(selectedProduct);

                if (productToUpdate is null) return;

                IsBusy = true;

                await Task.Run(async () => await _productService.UpdateProductAsync(productToUpdate));

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnDeleteProduct(ProductDto selectedProduct)
        {
            try
            {
                if (selectedProduct is null) return;

                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _productService.DeleteProductAsync(selectedProduct.Id);

                    FilteredProducts.Remove(selectedProduct);
                });

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnArchiveProduct(ProductDto selectedProduct)
        {
            try
            {
                if (selectedProduct is null) return;

                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                await Task.Delay(1000);

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnShowDetails(ProductDto selectedProduct)
        {
            try
            {
                if (selectedProduct is null) return;

                var view = new ProductDetailsForm()
                {
                    DataContext = new ProductDetailsFormViewModel(selectedProduct)
                };

                await DialogHost.Show(view, RegionNames.DialogRegion);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await _dialogService.ShowError();
            }
        }

        #endregion

        #region Helper methods

        private async Task<ProductForCreateDto> ShowAddProductForm()
        {
            var view = new ProductForm()
            {
                DataContext = new ProductFormViewModel(_categoryService)
            };

            var result = await DialogHost.Show(view, "RootDialog");

            return result as ProductForCreateDto;
        }

        private async Task<ProductForUpdateDto> ShowUpdateProductForm(ProductDto product)
        {
            var view = new ProductForm()
            {
                DataContext = new ProductFormViewModel(_categoryService, product)
            };

            var result = await DialogHost.Show(view, "RootDialog");

            return result as ProductForUpdateDto;
        }

        #endregion
    }
}
