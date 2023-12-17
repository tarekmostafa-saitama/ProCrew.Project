using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Products.Models;

namespace ProCrew.Application.Features.Products.Queries;

public record GetProductsQueryAsync : IRequest<List<ProductVm>>;

internal sealed class GetProductsQueryAsyncHandler(IProductService productService)
    : IRequestHandler<GetProductsQueryAsync, List<ProductVm>>
{
    public async Task<List<ProductVm>> Handle(GetProductsQueryAsync request, CancellationToken cancellationToken)
    {
        var products = await productService.GetAllProductsAsync();
        return products;
    }
}