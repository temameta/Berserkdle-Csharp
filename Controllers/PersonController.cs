using System.Diagnostics;
using Berserkdle.Data;
using Microsoft.AspNetCore.Mvc;
using Berserkdle.Models;

namespace Berserkdle.Controllers;

public class PersonController(BerserkdleDbContext context) : Controller
{
    public IActionResult Persons()
    {
        var allPersons = context.Persons.ToList();
        return View(allPersons);
    }

    [Route("/Home/PersonPage/{id}")]
    public IActionResult PersonPage(int id)
    {
        return View(context.Persons.Find(id));
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
}