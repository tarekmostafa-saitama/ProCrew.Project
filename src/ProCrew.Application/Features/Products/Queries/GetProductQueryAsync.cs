using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Products.Models;

namespace ProCrew.Application.Features.Products.Queries;

public record GetProductQueryAsync(int Id) : IRequest<ProductVm>;

internal sealed class GetProductQueryAsyncHandler(IProductService productService)
    : IRequestHandler<GetProductQueryAsync, ProductVm>
{
    public async Task<ProductVm> Handle(GetProductQueryAsync request, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductAsync(request.Id);
        return product;
    }
}