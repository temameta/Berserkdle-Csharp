using System.Diagnostics;
using Berserkdle.Data;
using Microsoft.AspNetCore.Mvc;
using Berserkdle.Models;

namespace Berserkdle.Controllers;

public class HomeController(ILogger<HomeController> logger, BerserkdleDbContext context)
    : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}