using ReverseAnalytics.Domain.DTOs.Debt;

namespace Inventory.Services.Interfaces
{
    public interface IDebtService
    {
        public Task<IEnumerable<DebtDto>> GetDebtsAsync();
        public Task<DebtDto?> GetDebtByIdAsync(int id);
    }
}
