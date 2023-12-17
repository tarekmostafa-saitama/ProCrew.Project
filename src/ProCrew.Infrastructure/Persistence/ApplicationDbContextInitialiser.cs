using Microsoft.Extensions.Logging;
using ProCrew.Domain.Entities;

namespace ProCrew.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;


    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Products.Any())
        {
            _context.Products.Add(new Product
                { Name = "Product 1", Description = "Description 1", Price = 100, Quantity = 10 });
            _context.Products.Add(new Product
                { Name = "Product 2", Description = "Description 2", Price = 200, Quantity = 20 });
            _context.Products.Add(new Product
                { Name = "Product 3", Description = "Description 3", Price = 300, Quantity = 30 });
            _context.Products.Add(new Product
                { Name = "Product 4", Description = "Description 4", Price = 400, Quantity = 40 });
            _context.Products.Add(new Product
                { Name = "Product 5", Description = "Description 5", Price = 500, Quantity = 50 });
            await _context.SaveChangesAsync();
        }
    }
}