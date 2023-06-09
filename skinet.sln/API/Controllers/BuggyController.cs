using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseController
{
    private readonly StoreContext _context;
    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
        return Ok("\"secret stuff\"");
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        var thing = _context.Products.Find(42684645);
        return thing == null ? NotFound(new ApiResponse(404)) : Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        _context.Products.Find(96786464).ToString();
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return BadRequest(new ApiResponse(400, $"{id} not valid!"));
    }
}
