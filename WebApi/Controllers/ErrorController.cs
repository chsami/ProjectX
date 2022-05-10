using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}