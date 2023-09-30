using Backend_API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers;

[Route("error/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseAPIController
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new APIResponse(code));
    }
}