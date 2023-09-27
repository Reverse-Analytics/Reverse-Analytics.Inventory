using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.ViewModels.Forms;
using Inventory.Modules.Sales.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Inventory.Modules.Sales.ViewModels
{
    internal class RefundsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private readonly IDialogService _dialogService;
        private readonly IRefundService _refundService;
        private readonly ISaleService _saleService;

        public List<Sale> Sales { get; private set; }
        public List<Refund> refunds { get; private set; }
        public ObservableCollection<Refund> Refunds { get; private set; }

        public DelegateCommand CreateRefundCommand { get; }

        public bool KeepAlive => false;

        public RefundsViewModel(IDialogService dialogService, IRefundService refundService, ISaleService saleService)
        {
            _dialogService = dialogService;
            _refundService = refundService;
            _saleService = saleService;

            CreateRefundCommand = new DelegateCommand(OnCreateRefund);

            refunds = new List<Refund>();
            Sales = new List<Sale>();
            Refunds = new ObservableCollection<Refund>();

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                IsBusy = true;

                var result = await _refundService.GetRefundsAsync();
                var sales = await _saleService.GetAllSales();

                refunds.AddRange(result);
                Refunds.AddRange(result);
                Sales.AddRange(sales);
            }
            catch (HttpRequestException ex)
            {
                await _dialogService.ShowError("Network error", ex.Message);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Something went wrong", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnCreateRefund()
        {
            var view = new SaleRefundForm()
            {
                DataContext = new SaleRefundFormViewModel(_dialogService, Sales)
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            if (result is not Refund refund)
            {
                return;
            }

            try
            {
                IsBusy = true;

                var createdRefund = await _refundService.CreateRefundAsync(refund);

                if (createdRefund is not null)
                {
                    refunds.Add(createdRefund);
                    Refunds.Add(createdRefund);
                }
            }
            catch (HttpRequestException ex)
            {
                await _dialogService.ShowError("Network error", ex.Message);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Something went wrong", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
