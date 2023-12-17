using ProCrew.Application.Features.Audits.Models;

namespace ProCrew.Application.Common.Services;

public interface IAuditService
{
    Task CreateAuditsAsync(List<Trail> trails);
    Task<List<Trail>> GetAllAuditsAsync();
    Task<Trail> GetAuditByIdAsync(string id);
}