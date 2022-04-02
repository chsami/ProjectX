using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet(Name = "GetProduct")]
    public async Task<GetProductResponse> Get(int productId)
    {
        var response = await _mediator.Send(new GetProductRequest() { Id = productId });

        return response;
    }
}
