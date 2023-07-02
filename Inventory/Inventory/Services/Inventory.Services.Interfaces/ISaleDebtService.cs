using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface ISaleDebtService
    {
        public Task<IEnumerable<SaleDebt>> GetSaleDebtsAsync();
        public Task<SaleDebt> GetSaleDebtByIdAsync(int id);
        public Task<SaleDebt> CreateSaleDebtAsync(SaleDebt saleDebtToCreate);
        public Task UpdateSaleDebtAsync(SaleDebt saleDebtToUpdate);
        public Task DeleteSaleDebtAsync(int id);
    }
}
