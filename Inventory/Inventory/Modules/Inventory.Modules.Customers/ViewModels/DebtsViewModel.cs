using Inventory.Core;
using Inventory.Core.Mvvm;
using Inventory.Modules.Customers.ViewModels.Forms;
using Inventory.Modules.Customers.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Debt;
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
        private readonly IDebtService _debtService;
        private readonly IDialogService _dialogService;

        public bool KeepAlive => false;

        private List<DebtDto> debts;
        public ObservableCollection<DebtDto> FilteredDebts { get; }
        public ObservableCollection<string> Statuses { get; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public ICommand MakePaymentCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        public DebtsViewModel(IDebtService service, IDialogService dialogService)
        {
            _debtService = service;
            _dialogService = dialogService;

            debts = new List<DebtDto>();
            FilteredDebts = new ObservableCollection<DebtDto>();
            Statuses = new ObservableCollection<string>();

            MakePaymentCommand = new DelegateCommand(OnMakePayment);
            ShowDetailsCommand = new DelegateCommand<DebtDto>(OnShowDetails);

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

                var result = await _debtService.GetDebtsAsync();

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

        private async void OnShowDetails(DebtDto debt)
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

        private async void OnMakePayment()
        {
            try
            {
                await ShowPaymentForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
        }

        private async Task ShowPaymentForm()
        {
            var view = new DebtPaymentForm();

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }

        private async Task ShowDetailsForm(DebtDto debt)
        {
            var view = new DebtDetailsForm()
            {
                DataContext = new DebtDetailsFormViewModel(debt)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }
    }
}
