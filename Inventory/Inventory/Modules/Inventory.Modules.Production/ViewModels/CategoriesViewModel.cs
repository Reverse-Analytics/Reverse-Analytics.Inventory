using Inventory.Core;
using Inventory.Core.Dialogs;
using Inventory.Core.Mvvm;
using Inventory.Modules.Production.ViewModels.CategoryDialogs;
using Inventory.Modules.Production.ViewModels.Forms;
using Inventory.Modules.Production.Views.CategoryDialogs;
using Inventory.Modules.Production.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels
{
    public class CategoriesViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private readonly ICategorySerivce _categoryService;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool KeepAlive => false;

        private List<ProductCategoryDto> _categoriesList;
        public ObservableCollection<ProductCategoryDto> Categories { get; set; }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        private readonly Prism.Services.Dialogs.IDialogService _dialogService;

        public CategoriesViewModel(ICategorySerivce categorySerivce, Prism.Services.Dialogs.IDialogService dialogService)
        {
            Title = "Categories";
            Categories = new ObservableCollection<ProductCategoryDto>();

            _categoryService = categorySerivce;

            AddCommand = new DelegateCommand(OnAddCategory);
            UpdateCommand = new DelegateCommand<ProductCategoryDto>(OnUpdate);
            DeleteCommand = new DelegateCommand<ProductCategoryDto>(OnDelete);
            ArchiveCommand = new DelegateCommand<ProductCategoryDto>(OnArchive);
            ShowDetailsCommand = new DelegateCommand<ProductCategoryDto>(OnShowDetails);

            LoadCategories();
            _dialogService = dialogService;
        }

        private async void LoadCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            _categoriesList = new List<ProductCategoryDto>(categories);

            Categories.AddRange(categories);
        }

        private async void OnAddCategory()
        {
            try
            {
                var view = new CategoriesFormDialogView()
                {
                    DataContext = new CategoriesFormDialogViewModel()
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is null || result is not ProductCategoryForCreateDto)
                {
                    return;
                }

                var category = result as ProductCategoryForCreateDto;

                await Task.Run(async () =>
                {
                    var createdCategory = await _categoryService.CreateCategoryAsync(category);

                    if (createdCategory is not null)
                    {
                        Categories.Add(createdCategory);
                    }
                });

                ShowSuccessDialog();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                ShowErrorDialog();
            }
        }

        private async void OnUpdate(ProductCategoryDto selectedCategory)
        {
            var view = new CategoriesFormDialogView()
            {
                DataContext = new CategoriesFormDialogViewModel(selectedCategory)
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is null || result is not ProductCategoryForUpdateDto)
            {
                return;
            }

            var category = result as ProductCategoryForUpdateDto;

            try
            {
                await Task.Run(async () =>
                {
                    await _categoryService.UpdateCategoryAsync(category);
                });

                selectedCategory.CategoryName = category.CategoryName;
                Categories.Clear();
                Categories.AddRange(_categoriesList);

                ShowSuccessDialog($"Category: {category.CategoryName} was successfully updated.");
            }
            catch (Exception ex)
            {
                ShowErrorDialog("There was an error while updating category. Please, try again later");
                Debug.WriteLine(ex);
            }
        }

        private async void OnDelete(ProductCategoryDto selectedCategory)
        {
            if (selectedCategory == null)
            {
                return;
            }

            var view = new ConfirmationDialog("Confirm action.", $"Are you sure you want to delete: {selectedCategory.CategoryName}?");

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is null || result is false)
            {
                return;
            }

            try
            {
                await Task.Run(async () =>
                {
                    await _categoryService.DeleteCategoryAsync(selectedCategory.Id);

                    Categories.Remove(selectedCategory);
                });

                ShowSuccessDialog($"Category: {selectedCategory.CategoryName} was successfully deleted.");
            }
            catch (Exception ex)
            {
                ShowErrorDialog("There was an error while deleting category. Please, try again later.");
                Debug.WriteLine(ex);
            }
        }

        private async void OnArchive(ProductCategoryDto selectedCategory)
        {


            if (selectedCategory == null)
            {
                return;
            }

            var view = new ConfirmationDialog("Confirm action.", $"Are you sure you want to archive: {selectedCategory.CategoryName}?");

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is null || result is false)
            {
                return;
            }

            // TODO implement archive

            await Task.Delay(1000);

            ShowSuccessDialog($"Category: {selectedCategory.CategoryName} was successfully archived.");
        }

        private async void OnShowDetails(ProductCategoryDto selectedCategory)
        {
            var view = new CategoryDetailsForm()
            {
                DataContext = new CategoryDetailsFormViewModel(selectedCategory)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }
    }
}
