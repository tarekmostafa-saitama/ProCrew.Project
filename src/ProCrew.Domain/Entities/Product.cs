using ProCrew.Domain.Common;

namespace ProCrew.Domain.Entities;

public class Product : ITrackedAuditEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}