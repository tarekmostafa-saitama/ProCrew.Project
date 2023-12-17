using Microsoft.EntityFrameworkCore;
using ProCrew.Application.Common.Services;
using ProCrew.Domain.Entities;
using ProCrew.Infrastructure.Persistence.BaseContext;

namespace ProCrew.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ISerializerService serializerService,
    IAuditService auditService)
    : BaseDbContext(options, serializerService, auditService)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}