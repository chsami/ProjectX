using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Users.Commands;
using WebApi.Features.Users.Queries;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;
    private readonly IFireBaseService _fireBaseService;

    public UserController(ILogger<UserController> logger, IMediator mediator, IFireBaseService fireBaseService)
    {
        _logger = logger;
        _mediator = mediator;
        _fireBaseService = fireBaseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string id)
    {
        return Ok(await _mediator.Send(new GetUserByIdRequest() {Id = id}));
    }

    [HttpGet("UserByEmail")]
    [ProducesResponseType(typeof(GetUserByEmailResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        return Ok(await _mediator.Send(new GetUserByEmailRequest() {Email = email}));
    }
    
    
    [HttpGet("Users")]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] int pageNumber, int pageSize)
    {
        return Ok(await _mediator.Send(new GetUsersRequest() { PageNumber = pageNumber, PageSize = pageSize}));
    }
    
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] CreateUserRequest createUserRequest)
    {
        return Ok(await _mediator.Send(createUserRequest));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] UpdateUserRequest updateUserRequest)
    {
        return Ok(await _mediator.Send(updateUserRequest));
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromQuery] string userId)
    {
        return Ok(await _mediator.Send(new DeleteUserRequest() {UserId = userId}));
    }
    
    [HttpGet("Role")]
    [ProducesResponseType(typeof(GetUserRolesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Roles([FromQuery] string userId)
    {
        return Ok(await _mediator.Send(new GetUserRolesRequest() {UserId = userId}));
    }
    
    [HttpPost("Role")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Role([FromBody] AddUserRolesRequest addUserRolesRequest)
    {
        return Ok(await _mediator.Send(addUserRolesRequest));
    }
}
