using ProCrew.Application.Common.ServiceContracts;
using ProCrew.Application.Features.Products.Models;

namespace ProCrew.Application.Common.Services;

public interface IProductService : IScopedService
{
    Task<int> CreateProductAsync(ProductVm productVm);
    Task<int> UpdateProductAsync(ProductVm productVm);
    Task<int> DeleteProductAsync(int id);

    Task<ProductVm> GetProductAsync(int id);
    Task<List<ProductVm>> GetAllProductsAsync();
}