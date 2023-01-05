using Inventory.Modules.Production.Views.CategoryDialogs;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using dialogNames = Inventory.Core.CategoryDialogNames;

namespace Inventory.Modules.Production.ViewModels
{
    public class CategoriesViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        public ICommand AddCategoryCommand { get; }
        private Visibility _addCategoryModalVisibility = Visibility.Collapsed;
        public Visibility AddCategoryModalVisibility 
        {
            get => _addCategoryModalVisibility;
            set => SetProperty(ref _addCategoryModalVisibility, value);
        }

        public List<string> ShortStringList { get; } = new List<string>
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6",
            "Item 7",
            "Item 8",
            "Item 9",
            "Item 10",
            "Item 12",
            "Item 13",
            "Item 14",
            "Item 15",
            "Item 16",
            "Item 17",
            "Item 18",
            "Item 19",
        };

        public CategoriesViewModel(IDialogService dialogService)
        {
            Title = "Categories";

            _dialogService = dialogService;

            AddCategoryCommand = new DelegateCommand(OnAddCategory);
        }

        private async void OnAddCategory()
        {
            // _dialogService.ShowDialog(dialogNames.AddCategory);
            //if(AddCategoryModalVisibility == Visibility.Collapsed)
            //{
            //    AddCategoryModalVisibility = Visibility.Visible;
            //}
            //else
            //{
            //    AddCategoryModalVisibility = Visibility.Collapsed;
            //}

            var view = new AddCategoryDialogView()
            {
                DataContext = this
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");
    }
}
