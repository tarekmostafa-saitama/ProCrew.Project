using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Audits.Models;

namespace ProCrew.Application.Features.Audits.Queries;

public record GetAuditQueryAsync(string Id) : IRequest<Trail>;

internal sealed class GetAuditQueryAsyncHandler(IAuditService  auditService) : IRequestHandler<GetAuditQueryAsync , Trail>
{
    public async Task<Trail> Handle(GetAuditQueryAsync request, CancellationToken cancellationToken)
    {
        var trail = await auditService.GetAuditByIdAsync(request.Id);
        return trail;
    }
}
