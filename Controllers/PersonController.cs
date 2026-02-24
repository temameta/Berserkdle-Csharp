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

    public IActionResult CreateEditPersonForm(CreatePerson model)
    {
        var parsedWeapons = new List<string>();
        var parsedGroups = new List<string>();
        foreach (var weapon in model.Weapons.Split(","))
        {
            parsedWeapons.Add(weapon.Trim());
        }
        foreach (var group in model.Groups.Split(","))
        {
            parsedGroups.Add(group.Trim());
        }

        var newPerson = new Person
        {
            Name = model.Name,
            Gender = model.Gender,
            Species = model.Species,
            FirstArc = model.FirstArc,
            Groups = parsedGroups,
            Weapons = parsedWeapons
        };
        context.Persons.Add(newPerson);
        context.SaveChanges();
        return RedirectToAction("Persons");
    }
}