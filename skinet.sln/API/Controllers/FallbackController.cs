using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FallbackController : Controller
{
    public IActionResult Index() => PhysicalFile(
        Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
        "text/HTML"
    );
}