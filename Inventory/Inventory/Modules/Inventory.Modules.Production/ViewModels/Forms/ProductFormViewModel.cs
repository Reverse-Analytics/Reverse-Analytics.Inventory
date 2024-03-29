﻿using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Core.Models;
using Inventory.Core.Enums;
using System.Collections.Generic;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class ProductFormViewModel : ViewModelBase
    {
        private readonly ICategorySerivce _categoryService;

        #region Properties

        private readonly bool IsEditingMode;
        private readonly int productId;
        private int categoryId;

        private ProductCategory _selectedCategory;
        public ProductCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                categoryId = value.Id;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

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

        #endregion

        public ObservableCollection<ProductCategory> Categories { get; }

        #region Commands

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructors

        public ProductFormViewModel(ICategorySerivce categorySerivce, ICollection<ProductCategory> categories)
        {
            _categoryService = categorySerivce;

            SaveCommand = new DelegateCommand(OnSave, CanSave);
            CancelCommand = new DelegateCommand(OnCancel);
            Categories = new ObservableCollection<ProductCategory>(categories);

            // LoadCategories();

            Title = "New Product";
        }

        public ProductFormViewModel(ICategorySerivce categorySerivce,
            Product productToUpdate,
            ICollection<ProductCategory> categories)
            : this(categorySerivce, categories)
        {
            ArgumentNullException.ThrowIfNull(productToUpdate, nameof(productToUpdate));

            IsEditingMode = productToUpdate.ProductName is not null;
            productId = productToUpdate.Id;
            Name = productToUpdate.ProductName;
            Code = productToUpdate.ProductCode;
            Volume = productToUpdate.Volume ?? 0;
            Weight = productToUpdate.Weight ?? 0;
            IncomePrice = productToUpdate.SupplyPrice ?? 0;
            SalePrice = productToUpdate.SalePrice;

            LoadCategory(productToUpdate.Category);

            Title = "Update Product";
        }

        #endregion

        private void LoadCategory(ProductCategory currentCategory)
        {
            if (currentCategory is null)
            {
                return;
            }

            var selected = Categories.FirstOrDefault(x => x.Id == currentCategory.Id);

            if (selected is null)
            {
                return;
            }

            SelectedCategory = selected;
        }

        public void OnSave()
        {
            if (IsEditingMode)
            {
                var productToUpdate = new Product()
                {
                    Id = productId,
                    ProductName = Name,
                    ProductCode = Code,
                    SalePrice = SalePrice,
                    SupplyPrice = IncomePrice,
                    Volume = Volume,
                    Weight = Weight,
                    UnitOfMeasurement = UnitOfMeasurement.Kg,
                    Category = SelectedCategory,
                    CategoryId = SelectedCategory.Id
                };


                DialogHost.Close("RootDialog", productToUpdate);
            }
            else
            {
                var productToCreate = new Product()
                {
                    ProductName = Name,
                    ProductCode = Code,
                    SalePrice = SalePrice,
                    SupplyPrice = IncomePrice,
                    Volume = Volume,
                    Weight = Weight,
                    UnitOfMeasurement = UnitOfMeasurement.Kg,
                    CategoryId = categoryId
                };

                DialogHost.Close("RootDialog", productToCreate);
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
