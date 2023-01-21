using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;

namespace Inventory.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerDto>?> GetCustomersAsync();
        public Task<CustomerDto?> GetCustomerByIdAsync(int id);
        public Task<CustomerDto?> CreateCustomerAsync(CustomerForCreateDto customerToCreate);
        public Task UpdateCustomerAsync(CustomerForUpdateDto customerToUpdate);
        public Task DeleteCustomerAsync(int id);
    }
}
