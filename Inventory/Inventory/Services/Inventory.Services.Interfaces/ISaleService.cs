using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<Sale>> GetAllSales();
        public Task<Sale> GetById(int id);
        public Task<Sale> CreateSale(Sale saleToCreate);
        public Task UpdateSale(Sale saleToUpdate);
        public Task DeleteSale(int id);
    }
}
