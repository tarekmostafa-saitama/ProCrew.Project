using Microsoft.EntityFrameworkCore;
using ProCrew.Application;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Audits.Models;
using ProCrew.Domain.Common;
using ProCrew.Infrastructure.Trails;

namespace ProCrew.Infrastructure.Persistence.BaseContext;

public abstract class BaseDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ISerializerService serializerService,
    IAuditService auditService)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.DetectChanges();
        try
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            var trails = new List<Trail>();
            var auditEntries = HandleAuditingBeforeSaveChanges(trails);
            var result = await base.SaveChangesAsync(cancellationToken);
            await HandleAuditingAfterSaveChangesAsync(auditEntries, trails, cancellationToken);
            await auditService.CreateAuditsAsync(trails);

            return result;
        }
        finally
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }


    private List<AuditTrail> HandleAuditingBeforeSaveChanges(List<Trail> trails)
    {
        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<ITrackedAuditEntity>())
        {
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            var trailEntry = new AuditTrail(entry, serializerService)
            {
                TableName = entry.Entity.GetType().Name
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:

                        trailEntry.ChangedColumns.Add(propertyName);
                        trailEntry.TrailType = TrailType.Update;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;


                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            trails.Add(auditEntry.ToAuditTrail());

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }

    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, List<Trail> trails,
        CancellationToken cancellationToken = new())
    {
        if (trailEntries == null || trailEntries.Count == 0) return Task.CompletedTask;

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
                if (prop.Metadata.IsPrimaryKey())
                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                else
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;

            trails.Add(entry.ToAuditTrail());
        }

        return Task.CompletedTask;
    }
}