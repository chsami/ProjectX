using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Tenants.Commands;
using WebApi.Features.Tenants.Queries;
using WebApi.Features.Users.Commands;
using WebApi.Features.Users.Queries;

namespace WebApi.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class TenantController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public TenantController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetTenantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] Guid id)
    {
        return Ok(await _mediator.Send(new GetTenantRequest() {Id = id}));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTenantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] CreateTenantRequest createTenantRequest)
    {
        return Ok(await _mediator.Send(createTenantRequest));
    }
}