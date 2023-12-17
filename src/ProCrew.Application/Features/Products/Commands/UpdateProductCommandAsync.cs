using FluentValidation;
using MediatR;
using ProCrew.Application.Common.Services;
using ProCrew.Application.Features.Products.Models;

namespace ProCrew.Application.Features.Products.Commands;

public record UpdateProductCommandAsync(ProductVm ProductVm) : IRequest<int>;

public class UpdateProductValidatorAsync : AbstractValidator<UpdateProductCommandAsync>
{
    public UpdateProductValidatorAsync()
    {
        RuleFor(x => x.ProductVm.Id)
            .NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.ProductVm.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.ProductVm.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.ProductVm.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.ProductVm.Quantity)
            .NotEmpty().WithMessage("Quantity is required.")
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}

internal sealed class UpdateProductCommandAsyncHandler(IProductService productService)
    : IRequestHandler<UpdateProductCommandAsync, int>
{
    public async Task<int> Handle(UpdateProductCommandAsync request, CancellationToken cancellationToken)
    {
        var productId = await productService.UpdateProductAsync(request.ProductVm);
        return productId;
    }
}