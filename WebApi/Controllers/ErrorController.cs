using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ErrorController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [Route("error")]
    public ProblemDetails Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error; // Your exception

        return new ProblemDetails()
        {
            Detail = exception?.StackTrace,
            Title = exception?.Message
        };
    }
    [Route("abc")]
    public string Abc()
    {
        var a = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DefaultConnection");
        return a ?? "empty";
    }
    [Route("connectionstring")]
    public string Connectionstring()
    {
        var result = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");
        return result ?? "empty";
    }
}