using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface IRefundService
    {
        public Task<IEnumerable<Refund>> GetRefundsAsync();
        public Task<Refund> GetRefundByIdAsync(int id);
        public Task<Refund> CreateRefundAsync(Refund refundToCreate);
        public Task<Refund> UpdateRefundAsync(Refund refundToUpdate);
        public Task DeleteRefundAsync(int id);
    }
}
