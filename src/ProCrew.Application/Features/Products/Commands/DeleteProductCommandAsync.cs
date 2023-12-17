using MediatR;
using ProCrew.Application.Common.Services;

namespace ProCrew.Application.Features.Products.Commands;

public record DeleteProductCommandAsync(int Id) : IRequest<int>;

internal sealed class DeleteProductCommandAsyncHandler(IProductService productService)
    : IRequestHandler<DeleteProductCommandAsync, int>
{
    public async Task<int> Handle(DeleteProductCommandAsync request, CancellationToken cancellationToken)
    {
        var productId = await productService.DeleteProductAsync(request.Id);
        return productId;
    }
}