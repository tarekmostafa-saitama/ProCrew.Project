using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProCrew.Application.Features.Products.Commands;
using ProCrew.Application.Features.Products.Models;
using ProCrew.Application.Features.Products.Queries;

namespace ProCrew.API.Controllers;

/// <summary>
///     API Controller for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]

public class ProductsController(ISender sender) : ControllerBase
{
    /// <summary>
    ///     Gets all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductVm>>> GetAllProductsAsync()
    {
        var products = await sender.Send(new GetProductsQueryAsync());
        return Ok(products);
    }

    /// <summary>
    ///     Gets a product by id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<ProductVm>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductVm>> GetProductAsync(int id)
    {
        var product = await sender.Send(new GetProductQueryAsync(id));
        return Ok(product);
    }

    /// <summary>
    ///     Creates a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProductAsync([FromBody] ProductVm productVm)
    {
        var productId = await sender.Send(new CreateProductCommandAsync(productVm));
        return CreatedAtAction("GetProduct", new { id = productId }, productVm);
    }

    /// <summary>
    ///     Updates an existing product
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProductAsync([FromBody] ProductVm productVm)
    {
        var productId = await sender.Send(new UpdateProductCommandAsync(productVm));
        return Ok(productId);
    }

    /// <summary>
    ///     Deletes a product by id
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType<ProductVm>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProductAsync(int id)
    {
        var productId = await sender.Send(new DeleteProductCommandAsync(id));
        return Ok(productId);
    }
}