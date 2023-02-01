using Inventory.Core;
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
        #region Services

        private readonly IDialogService _dialogService;
        private readonly ICategorySerivce _categoryService;

        #endregion

        #region Properties

        private string _title = "Categories";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

        public bool KeepAlive => false;

        #region Collections

        private List<ProductCategoryDto> _categoriesList;
        public ObservableCollection<ProductCategoryDto> Categories { get; set; }

        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        #endregion

        public CategoriesViewModel(ICategorySerivce categorySerivce, IDialogService dialogService)
        {
            Categories = new ObservableCollection<ProductCategoryDto>();

            _categoryService = categorySerivce;
            _dialogService = dialogService;

            AddCommand = new DelegateCommand(OnAddCategory);
            UpdateCommand = new DelegateCommand<ProductCategoryDto>(OnUpdate);
            DeleteCommand = new DelegateCommand<ProductCategoryDto>(OnDelete);
            ArchiveCommand = new DelegateCommand<ProductCategoryDto>(OnArchive);
            ShowDetailsCommand = new DelegateCommand<ProductCategoryDto>(OnShowDetails);

            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                _categoriesList = new List<ProductCategoryDto>(categories);

                Categories.AddRange(categories);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(message: "There was an error retrieving categories... Please, try again later.");
                Debug.WriteLine(ex);
            }
        }

        #region Command methods

        private async void OnAddCategory()
        {
            try
            {
                var category = await ShowAddCategoryForm();

                if (category is null)
                {
                    return;
                }

                IsBusy = true;

                await Task.Run(async () =>
                {
                    var createdCategory = await _categoryService.CreateCategoryAsync(category);

                    if (createdCategory is not null)
                    {
                        Categories.Add(createdCategory);
                    }
                });

                await _dialogService.ShowSuccess(message: $"Category: {category.CategoryName} was added.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await _dialogService.ShowError(message: $"There was an error adding new Category. Please, try again later.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnUpdate(ProductCategoryDto selectedCategory)
        {
            try
            {
                var category = await ShowUpdateCategoryForm(selectedCategory);

                if (category is null)
                {
                    return;
                }

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _categoryService.UpdateCategoryAsync(category);
                });

                selectedCategory.CategoryName = category.CategoryName;

                Categories.Clear();
                Categories.AddRange(_categoriesList);

                await _dialogService.ShowSuccess(message: $"Category: {category.CategoryName} was successfully updated.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(message: "There was an error while updating category. Please, try again later");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnDelete(ProductCategoryDto selectedCategory)
        {
            try
            {
                if (selectedCategory == null)
                {
                    return;
                }

                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm)
                {
                    return;
                }

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _categoryService.DeleteCategoryAsync(selectedCategory.Id);

                    Categories.Remove(selectedCategory);
                });

                await _dialogService.ShowSuccess(message: $"Category: {selectedCategory.CategoryName} was successfully deleted.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(message: "There was an error while deleting category. Please, try again later.");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnArchive(ProductCategoryDto selectedCategory)
        {
            try
            {
                if (selectedCategory == null)
                {
                    return;
                }

                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm)
                {
                    return;
                }

                IsBusy = true;

                // TODO implement archiving
                await Task.Delay(1500);

                await _dialogService.ShowSuccess(message: $"Category: {selectedCategory.CategoryName} was successfully archived.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(message: $"There was an error archiving Category: {selectedCategory.CategoryName}. Please, try again later");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnShowDetails(ProductCategoryDto selectedCategory)
        {
            var view = new CategoryDetailsForm()
            {
                DataContext = new CategoryDetailsFormViewModel(selectedCategory)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }

        #endregion

        #region Helper methods

        private static async Task<ProductCategoryForCreateDto> ShowAddCategoryForm()
        {
            var view = new CategoriesFormDialogView()
            {
                DataContext = new CategoriesFormDialogViewModel()
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is null || result is not ProductCategoryForCreateDto category)
            {
                return null;
            }

            return category;
        }

        private static async Task<ProductCategoryForUpdateDto> ShowUpdateCategoryForm(ProductCategoryDto categoryToUpdate)
        {
            if (categoryToUpdate is null)
            {
                return null;
            }

            var view = new CategoriesFormDialogView()
            {
                DataContext = new CategoriesFormDialogViewModel(categoryToUpdate)
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is null || result is not ProductCategoryForUpdateDto updatedCategory)
            {
                return null;
            }

            return updatedCategory;
        }

        #endregion
    }
}
