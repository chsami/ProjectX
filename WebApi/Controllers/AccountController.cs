using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Accounts.Commands;
using WebApi.Features.Accounts.Queries;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMediator _mediator;

    public AccountController(ILogger<AccountController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet(Name = "Roles")]
    public async Task<List<GetRolesResponse>> GetRoles([FromQuery] string userId)
    {
        var response = await _mediator.Send(new GetRolesRequest() {UserId = userId});
        return response;
    }
    
    [HttpPost(Name = "AddRole")]
    public async Task<IActionResult> AddRole([FromBody] AddRoleRequest addRoleRequest)
    {
        await _mediator.Send(addRoleRequest);
        return Ok();
    }

    [HttpPost(Name = "Register")]
    public async Task<int> Post(RegisterAccountRequest registerAccountRequest)
    {
        await _mediator.Send(registerAccountRequest);
        return 0;
    }
}
