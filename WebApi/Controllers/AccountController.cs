using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Accounts.Commands;

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

    [HttpPost(Name = "Register")]
    public async Task<int> Post(RegisterAccountRequest registerAccountRequest)
    {
        await _mediator.Send(registerAccountRequest);
        return 0;
    }
}
