using System.Diagnostics;
using System.Globalization;
using Berserkdle.Data;
using Microsoft.AspNetCore.Mvc;
using Berserkdle.Models;
using Microsoft.EntityFrameworkCore;

namespace Berserkdle.Controllers;

public class PersonController(BerserkdleDbContext context) : Controller
{
    public IActionResult Persons()
    {
        var allPersons = context.Persons.ToList();
        return View(allPersons);
    }
    
    public IActionResult PersonPage(int id)
    {
        return View(context.Persons.Find(id));
    }

    public IActionResult CreatePerson()
    {
        return View();
    }

    public IActionResult CreatePersonForm(CreatePersonForm model)
    {
        var parsedWeapons = new List<string>();
        var parsedGroups = new List<string>();
        TextInfo textInfo = new CultureInfo("ru-RU",false).TextInfo;
        foreach (var weapon in model.Weapons.Split(","))
            parsedWeapons.Add(textInfo.ToTitleCase(weapon.Trim()));
        foreach (var group in model.Groups.Split(","))
            parsedGroups.Add(textInfo.ToTitleCase(group.Trim()));

        var newPerson = new Person
        {
            Name = textInfo.ToTitleCase(model.Name),
            Gender = textInfo.ToTitleCase(model.Gender),
            Species = textInfo.ToTitleCase(model.Species),
            FirstArc = textInfo.ToTitleCase(model.FirstArc),
            Groups = parsedGroups,
            Weapons = parsedWeapons
        };
        context.Persons.Add(newPerson);
        context.SaveChanges();
        return RedirectToAction("Persons");
    }
    
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var person = await context.Persons.FindAsync(id);
        return View(person);
    }
    
    public async Task<IActionResult> DeletePerson(int id)
    {
        var person = await context.Persons.FindAsync(id);
        context.Persons.Remove(person);
        await context.SaveChangesAsync();
        return RedirectToAction("Persons");
    }
}