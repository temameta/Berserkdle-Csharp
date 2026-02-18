using Microsoft.AspNetCore.Mvc;

namespace Berserkdle.Controllers;

public class GameController : Controller
{
    public IActionResult Main()
    {
        return View();
    }
}