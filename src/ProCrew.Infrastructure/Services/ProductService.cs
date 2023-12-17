using Mapster;
using Microsoft.EntityFrameworkCore;
using ProCrew.Application.Common.Exceptions;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Products.Models;
using ProCrew.Domain.Entities;
using ProCrew.Infrastructure.Persistence;

namespace ProCrew.Infrastructure.Services;

public class ProductService(ApplicationDbContext dbContext) : IProductService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<int> CreateProductAsync(ProductVm productVm)
    {
        var product = productVm.Adapt<Product>();
        _dbContext.Products.Add(product);
        var result = await _dbContext.SaveChangesAsync();
        return product.Id;
    }

    public async Task<int> UpdateProductAsync(ProductVm productVm)
    {
        var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id ==productVm.Id);
        if (product == null) throw new NotFoundException("Product not found");
        product = productVm.Adapt<Product>();
        _dbContext.Products.Update(product);
        var result = await _dbContext.SaveChangesAsync();
        return product.Id;
    }

    public async Task<int> DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id ==id);
        if (product == null) throw new NotFoundException("Product not found");
        _dbContext.Products.Remove(product);
        var result = await _dbContext.SaveChangesAsync();
        return id;
    }

    public async Task<ProductVm> GetProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) throw new NotFoundException("Product not found");
        return product.Adapt<ProductVm>();
    }

    public async Task<List<ProductVm>> GetAllProductsAsync()
    {
        var products = await _dbContext.Products.AsNoTracking().ToListAsync();
        return products.Adapt<List<ProductVm>>();
    }
}