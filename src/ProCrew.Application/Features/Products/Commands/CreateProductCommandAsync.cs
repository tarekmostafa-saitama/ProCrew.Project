using FluentValidation;
using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Products.Models;

namespace ProCrew.Application.Features.Products.Commands;

public record CreateProductCommandAsync(ProductVm ProductVm) : IRequest<int>;

public class CreateProductValidatorAsync : AbstractValidator<CreateProductCommandAsync>
{
    public CreateProductValidatorAsync()
    {
        RuleFor(x => x.ProductVm.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.ProductVm.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ProductVm.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.ProductVm.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
    }
}

internal sealed class CreateProductCommandAsyncHandler(IProductService productService)
    : IRequestHandler<CreateProductCommandAsync, int>
{
    public async Task<int> Handle(CreateProductCommandAsync request, CancellationToken cancellationToken)
    {
        var productId = await productService.CreateProductAsync(request.ProductVm);
        return productId;
    }
}