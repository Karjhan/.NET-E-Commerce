using Backend_API.DataContexts;
using Backend_API.Entities;
using Backend_API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class BuggyController : BaseAPIController
{
    private readonly StoreContext _context;

    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundResult()
    {
        return NotFound(new APIResponse(404));
    }
    
    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        Product ceva = null;
        ceva.ToString();
        return Ok();
    }
    
    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new APIResponse(400));
    }
    
    [HttpGet("badrequest/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return Ok();
    }
}