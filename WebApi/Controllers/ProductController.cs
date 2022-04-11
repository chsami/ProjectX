using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Products.Commands;
using WebApi.Features.Products.Queries;

namespace WebApi.Controllers;

[ApiController]
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
    public async Task<List<GetAllProductsResponse>> GetAllProducts()
    {
        var response = await _mediator.Send(new GetAllProductsRequest());

        return response;
    }

    [HttpGet("{productId:guid}")]
    public async Task<GetProductResponse> GetProduct(Guid productId)
    {
        var response = await _mediator.Send(new GetProductRequest() { Id = productId });

        return response;
    }


    [HttpPost]
    public async Task<Guid> CreateProduct(CreateProductRequest product)
    {
        var id = await _mediator.Send(product);

        return id;
    }
}
