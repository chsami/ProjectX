using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Products.Commands;
using WebApi.Features.Products.Queries;

namespace WebApi.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<GetAllProductsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(await _mediator.Send(new GetAllProductsRequest()));
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    public async Task<GetProductResponse> GetProduct(Guid productId)
    {
        var response = await _mediator.Send(new GetProductRequest() { Id = productId });

        return response;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateProduct(CreateProductRequest product)
    {
        return Ok(await _mediator.Send(product));
    }
}
