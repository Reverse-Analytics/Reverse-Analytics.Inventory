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

        public bool KeepAlive => false;

        #endregion

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

        #region Main methods

        private async void LoadCategories()
        {
            try
            {
                IsBusy = true;

                var categories = await _categoryService.GetCategoriesAsync();
                _categoriesList = new List<ProductCategoryDto>(categories);

                Categories.AddRange(categories);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(message: "There was an error retrieving categories... Please, try again later.");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Command methods

        private async void OnAddCategory()
        {
            try
            {
                var category = await ShowAddCategoryForm();

                if (category is null) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    var createdCategory = await _categoryService.CreateCategoryAsync(category);

                    if (createdCategory is not null)
                    {
                        Categories.Add(createdCategory);
                    }
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

        private async void OnUpdate(ProductCategoryDto selectedCategory)
        {
            try
            {
                var category = await ShowUpdateCategoryForm(selectedCategory);

                if (category is null) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _categoryService.UpdateCategoryAsync(category);
                });

                selectedCategory.CategoryName = category.CategoryName;

                Categories.Clear();
                Categories.AddRange(_categoriesList);

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError();
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
                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _categoryService.DeleteCategoryAsync(selectedCategory.Id);

                    Categories.Remove(selectedCategory);
                });

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError();
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
                var isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                // TODO implement archiving
                await Task.Delay(1500);

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError();
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

            return result as ProductCategoryForCreateDto;
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

            return result as ProductCategoryForUpdateDto;
        }

        #endregion
    }
}
