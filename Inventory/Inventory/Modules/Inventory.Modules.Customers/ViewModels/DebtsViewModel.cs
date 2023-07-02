using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Customers.ViewModels.Forms;
using Inventory.Modules.Customers.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels
{
    public class DebtsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private readonly ISaleDebtService _debtService;
        private readonly IDialogService _dialogService;

        public bool KeepAlive => false;

        private List<SaleDebt> debts;
        public ObservableCollection<SaleDebt> FilteredDebts { get; }
        public ObservableCollection<string> Statuses { get; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public ICommand MakePaymentCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        public DebtsViewModel(ISaleDebtService debtService, IDialogService dialogService)
        {
            _debtService = debtService;
            _dialogService = dialogService;

            debts = new List<SaleDebt>();
            FilteredDebts = new ObservableCollection<SaleDebt>();
            Statuses = new ObservableCollection<string>();

            MakePaymentCommand = new DelegateCommand<SaleDebt>(OnMakePayment);
            ShowDetailsCommand = new DelegateCommand<SaleDebt>(OnShowDetails);

            LoadDebts();
        }

        private async void LoadDebts()
        {
            try
            {
                IsBusy = true;

                Statuses.Add("Closed");
                Statuses.Add("Overdue");
                Statuses.Add("Payment required");
                Statuses.Add("Due soon");

                var result = await _debtService.GetSaleDebtsAsync();

                debts.AddRange(result);
                FilteredDebts.AddRange(result);
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

        private async void OnShowDetails(SaleDebt debt)
        {
            try
            {
                await ShowDetailsForm(debt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await _dialogService.ShowError();
            }
        }

        private async void OnMakePayment(SaleDebt debt)
        {
            try
            {
                await ShowPaymentForm(debt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
        }

        private async Task ShowPaymentForm(SaleDebt debt)
        {
            var view = new DebtPaymentForm
            {
                DataContext = new DebtPaymentFormViewModel(debt)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }

        private async Task ShowDetailsForm(SaleDebt debt)
        {
            var view = new DebtDetailsForm()
            {
                DataContext = new DebtDetailsFormViewModel(debt)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }
    }
}
