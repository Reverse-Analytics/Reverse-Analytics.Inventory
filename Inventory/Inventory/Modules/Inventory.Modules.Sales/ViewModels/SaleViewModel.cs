using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Sale;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventory.Modules.Sales.ViewModels
{
    public class SaleViewModel : ViewModelBase, IRegionMemberLifetime
    {

        #region Services

        private readonly ISaleService _service;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        public bool KeepAlive => false;

        #endregion

        #region Commands

        public ICommand DetailsCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }

        #endregion

        #region Collections

        private readonly List<SaleDto> _sales;
        public ObservableCollection<SaleDto> Sales { get; private set; }

        #endregion

        public SaleViewModel(ISaleService service, IDialogService dialogService)
        {
            _service = service;
            _dialogService = dialogService;

            Sales = new ObservableCollection<SaleDto>();
            _sales = new List<SaleDto>();

            LoadSales().ConfigureAwait(false);
        }

        private async Task LoadSales()
        {
            try
            {
                IsBusy = true;

                var sales = await _service.GetAllSales();

                Sales.AddRange(sales);
                _sales.AddRange(sales);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
