using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Debt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inventory.Modules.Customers.ViewModels
{
    public class DebtsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private readonly IDebtService _debtService;
        private readonly IDialogService _dialogService;

        public bool KeepAlive => false;

        private List<DebtDto> debts;
        public ObservableCollection<DebtDto> FilteredDebts { get; }

        public DebtsViewModel(IDebtService service)
        {
            _debtService = service;

            debts = new List<DebtDto>();
            FilteredDebts = new ObservableCollection<DebtDto>();

            LoadDebts();
        }

        private async void LoadDebts()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    var result = await _debtService.GetDebtsAsync();

                    debts.AddRange(result);
                    FilteredDebts.AddRange(result);
                });
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
