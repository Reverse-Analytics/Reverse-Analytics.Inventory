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
using System.Linq;
using System.Net.Http;

namespace Inventory.Modules.Sales.ViewModels
{
    internal class RefundsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        private readonly IDialogService _dialogService;
        private readonly IRefundService _refundService;
        private readonly ISaleService _saleService;

        private readonly List<Refund> refunds;
        private readonly List<Sale> Sales;
        public ObservableCollection<Refund> Refunds { get; private set; }

        public DelegateCommand CreateCommand { get; }
        public DelegateCommand<Refund> DeleteCommand { get; }
        public DelegateCommand<Refund> ShowDetailsCommand { get; }
        public DelegateCommand<Refund> PrintReceiptCommand { get; }

        public bool KeepAlive => false;

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                FilterRefundsByDate();
            }
        }

        public RefundsViewModel(IDialogService dialogService, IRefundService refundService, ISaleService saleService)
        {
            SelectedDate = DateTime.Now;

            _dialogService = dialogService;
            _refundService = refundService;
            _saleService = saleService;

            CreateCommand = new DelegateCommand(OnCreateRefund);
            DeleteCommand = new DelegateCommand<Refund>(OnDelete);
            ShowDetailsCommand = new DelegateCommand<Refund>(OnShowDetails);
            PrintReceiptCommand = new DelegateCommand<Refund>(OnPrintReceipt);


            refunds = new List<Refund>();
            Sales = new List<Sale>();
            Refunds = new ObservableCollection<Refund>();

            LoadData();
        }

        #region Command methods

        private async void OnCreateRefund()
        {
            var refundableSales = Sales.Where(s => !Refunds.Any(r => s.Id == r.SaleId)).ToList();
            var view = new SaleRefundForm()
            {
                DataContext = new SaleRefundFormViewModel(_dialogService, refundableSales)
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

        private async void OnDelete(Refund refund)
        {
            if (refund is null)
            {
                return;
            }

            try
            {
                IsBusy = true;

                refunds.Remove(refund);
                Refunds.Remove(refund);

                await _refundService.DeleteRefundAsync(refund.Id);

                await _dialogService.ShowSuccess("Success", $"Refund {refund.Id} was deleted.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error", $"There was an error deleting refund {refund.Id}. {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnShowDetails(Refund refund)
        {
            if (refund is null)
            {
                return;
            }

            var view = new RefundDetailsForm()
            {
                DataContext = new RefundDetailsFormViewModel(refund)
            };

            try
            {
                await DialogHost.Show(view, RegionNames.DialogRegion);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error opening refund details.", ex.Message);
            }
        }

        private async void OnPrintReceipt(Refund refund)
        {
            throw new NotImplementedException();
        }

        #endregion

        private async void LoadData()
        {
            try
            {
                IsBusy = true;

                var result = await _refundService.GetRefundsAsync();
                var sales = await _saleService.GetAllSales();
                var refundsForCurrentDate = result.Where(x => x.RefundDate.Date == SelectedDate.Value.Date);

                refunds.AddRange(result);
                Refunds.AddRange(refundsForCurrentDate);
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

        private void FilterRefundsByDate()
        {
            if (refunds is null || !refunds.Any())
            {
                return;
            }

            if (SelectedDate is null)
            {
                Refunds.Clear();
                Refunds.AddRange(refunds);

                return;
            }

            var filteredRefunds = refunds.Where(x => x.RefundDate.Date == SelectedDate.Value.Date);

            Refunds.Clear();
            Refunds.AddRange(filteredRefunds);
        }
    }
}
