using Inventory.Core;
using Inventory.Core.Enums;
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
using System.Linq;
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
        public ObservableCollection<DebtStatus> Statuses { get; }

        private DebtStatus? _selectedStatus;
        public DebtStatus? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                FilterByStatus(value);
                SetProperty(ref _selectedStatus, value);
            }
        }

        public ICommand MakePaymentCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand CloseDebtCommand { get; }

        public DebtsViewModel(ISaleDebtService debtService, IDialogService dialogService)
        {
            _debtService = debtService;
            _dialogService = dialogService;

            debts = new List<SaleDebt>();
            FilteredDebts = new ObservableCollection<SaleDebt>();
            Statuses = new ObservableCollection<DebtStatus>();

            MakePaymentCommand = new DelegateCommand<SaleDebt>(OnMakePayment);
            ShowDetailsCommand = new DelegateCommand<SaleDebt>(OnShowDetails);
            CloseDebtCommand = new DelegateCommand<SaleDebt>(OnCloseDebt);

            LoadDebts();
        }

        private async void LoadDebts()
        {
            try
            {
                IsBusy = true;

                var result = await _debtService.GetSaleDebtsAsync();
                var statuses = result.Select(x => x.Status).Distinct().ToList();

                Statuses.AddRange(statuses);

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

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is not SaleDebt payment)
            {
                return;
            }

            try
            {
                var response = await _debtService.PayDebtAsync(debt.Id, payment.TotalPaid);
                var index = debts.IndexOf(debt);

                debts[index] = response;
                FilteredDebts[index] = response;

                await _dialogService.ShowSuccess("Payment was successful");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ShowDetailsForm(SaleDebt debt)
        {
            var view = new DebtDetailsForm()
            {
                DataContext = new DebtDetailsFormViewModel(debt)
            };

            await DialogHost.Show(view, RegionNames.DialogRegion);
        }

        private async void OnCloseDebt(SaleDebt debt)
        {
            if (debt is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var response = await _debtService.CloseDebtAsync(debt.Id);
                var index = debts.IndexOf(debt);
                debts[index] = response;
                FilteredDebts[index] = response;

                await _dialogService.ShowSuccess("Debt closed!");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error while closing a debt", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterByStatus(DebtStatus? status)
        {
            if (!status.HasValue)
            {
                FilteredDebts.Clear();
                FilteredDebts.AddRange(debts);

                return;
            }

            var filteredDebts = debts.Where(x => x.Status == status);

            FilteredDebts.Clear();
            FilteredDebts.AddRange(filteredDebts);
        }
    }
}
