using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Audits.Models;

namespace ProCrew.Application.Features.Audits.Queries;

public record GetAuditsQueryAsync:IRequest<List<Trail>>;


internal sealed class GetAuditsQueryAsyncHandler(IAuditService auditService) : IRequestHandler<GetAuditsQueryAsync, List<Trail>>
{
    public async Task<List<Trail>> Handle(GetAuditsQueryAsync request, CancellationToken cancellationToken)
    {
        return await auditService.GetAllAuditsAsync(); 
    }
}