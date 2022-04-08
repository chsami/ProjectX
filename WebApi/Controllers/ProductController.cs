using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Products.Commands;
using WebApi.Features.Products.Queries;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet(Name = "GetProduct")]
    public async Task<GetProductResponse> Get(Guid productId)
    {
        var response = await _mediator.Send(new GetProductRequest() { Id = productId });

        return response;
    }


    [HttpPost(Name = "AddProduct")]
    public async Task<Guid> Post(CreateProductRequest product)
    {
        var id = await _mediator.Send(product);

        return id;
    }
}
