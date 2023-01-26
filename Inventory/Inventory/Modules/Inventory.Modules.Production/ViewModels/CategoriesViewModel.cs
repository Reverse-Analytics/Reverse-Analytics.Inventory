using Inventory.Core.Dialogs;
using Inventory.Modules.Production.ViewModels.CategoryDialogs;
using Inventory.Modules.Production.Views.CategoryDialogs;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.ProductCategory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Inventory.Modules.Production.ViewModels
{
    public class CategoriesViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly ICategorySerivce _categoryService;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private List<ProductCategoryDto> _categoriesList;
        public ObservableCollection<ProductCategoryDto> Categories { get; set; }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ArchiveCommand { get; }

        public bool KeepAlive => false;

        public CategoriesViewModel(ICategorySerivce categorySerivce)
        {
            Title = "Categories";
            Categories = new ObservableCollection<ProductCategoryDto>();

            _categoryService = categorySerivce;

            AddCommand = new DelegateCommand(OnAddCategory);
            EditCommand = new DelegateCommand<ProductCategoryDto>(OnEdit);
            DeleteCommand = new DelegateCommand<int?>(OnDelete);
            ArchiveCommand = new DelegateCommand(OnArchive);

            InitializeCollections();
        }

        private async void InitializeCollections() 
        {
            var categories = await _categoryService.GetCategoriesAsync();
            _categoriesList = new List<ProductCategoryDto>(categories);

            Categories.AddRange(categories);
        }

        private async void OnAddCategory()
        {
            var view = new CategoriesFormDialogView()
            {
                DataContext = new CategoriesFormDialogViewModel()
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (result is null || result is not ProductCategoryForCreateDto)
            {
                return;
            }

            var category = result as ProductCategoryForCreateDto;
            var createdCategory = await _categoryService.CreateCategoryAsync(category);

            if (createdCategory is not null)
            {
                Categories.Add(createdCategory);
            }
        }

        private async void OnEdit(ProductCategoryDto selectedCategory)
        {
            var view = new CategoriesFormDialogView()
            {
                DataContext = new CategoriesFormDialogViewModel(selectedCategory)
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            if (result is not null && result is ProductCategoryForUpdateDto)
            {
                var category = result as ProductCategoryForUpdateDto;
                await _categoryService.UpdateCategoryAsync(category);

                selectedCategory.CategoryName = category.CategoryName;
                Categories.Clear();
                Categories.AddRange(_categoriesList);
            }
        }

        private async void OnDelete(int? id)
        {
            if (!id.HasValue)
            {
                return;
            }

            var selectedCategory = Categories.FirstOrDefault(c => c.Id == id);
            var view = new ConfirmationDialog("Confirm action.", $"Are you sure you want to delete: {selectedCategory.CategoryName}?");

            var result = await DialogHost.Show(view, "RootDialog");
            bool? isConfirm = result as bool?;

            if (isConfirm.HasValue && isConfirm.Value)
            {
                await _categoryService.DeleteCategoryAsync(id.Value);

                Categories.Remove(selectedCategory);
            }
        }

        private async void OnArchive()
        {

        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");
    }
}
