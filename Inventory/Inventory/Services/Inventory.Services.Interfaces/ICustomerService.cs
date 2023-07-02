using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>?> GetCustomersAsync();
        public Task<Customer?> GetCustomerByIdAsync(int id);
        public Task<Customer?> CreateCustomerAsync(Customer customerToCreate);
        public Task UpdateCustomerAsync(Customer customerToUpdate);
        public Task DeleteCustomerAsync(int id);
    }
}
