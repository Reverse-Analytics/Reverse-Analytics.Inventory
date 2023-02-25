using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Debt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        public DebtsViewModel(IDebtService service, IDialogService dialogService)
        {
            _debtService = service;
            _dialogService = dialogService;

            debts = new List<DebtDto>();
            FilteredDebts = new ObservableCollection<DebtDto>();
            Statuses = new ObservableCollection<string>();

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
    }
}
