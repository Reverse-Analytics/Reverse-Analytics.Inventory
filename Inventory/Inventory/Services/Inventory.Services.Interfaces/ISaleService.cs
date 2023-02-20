using ReverseAnalytics.Domain.DTOs.Sale;

namespace Inventory.Services.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleDto>> GetAllSales();
        public Task<SaleDto> GetById(int id);
        public Task<SaleDto> CreateSale(SaleForCreateDto saleToCreate);
        public Task UpdateSale(SaleForUpdateDto saleToUpdate);
        public Task DeleteSale(int id);
    }
}
