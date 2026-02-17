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

    public IActionResult Persons()
    {
        var allPersons = context.Persons.ToList();
        return View(allPersons);
    }

    public IActionResult CreateEditPerson()
    {
        return View();
    }

    public IActionResult CreateEditPersonForm(Person model)
    {
        context.Persons.Add(model);
        context.SaveChanges();
        return RedirectToAction("Persons");
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