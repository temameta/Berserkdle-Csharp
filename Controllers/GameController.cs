using System.Text.Json;
using Berserkdle.Data;
using Berserkdle.Models;
using Microsoft.AspNetCore.Mvc;

namespace Berserkdle.Controllers;

public class GameController(BerserkdleDbContext dbContext) : Controller
{
    public IActionResult Main()
    {
        if (HttpContext.Session.GetInt32("attempts") == null)
            HttpContext.Session.SetInt32("attempts", 0);
        if (HttpContext.Session.GetString("mysteryPerson") == null)
        {
            if (dbContext.Persons.Any())
            {
                Random rnd = new Random();
                List<Person> allPersons = dbContext.Persons.ToList();
                int index = rnd.Next(allPersons.Count);
                HttpContext.Session.SetString("mysteryPerson", allPersons[index].Name);
            }
            else
            {
                throw new InvalidDataException("No Persons find");
            }
        }
        if (HttpContext.Session.GetString("guessedPersons") == null)
            HttpContext.Session.SetString("guessedPersons", JsonSerializer.Serialize(new List<string>()));
        if (HttpContext.Session.GetString("allNames") == null)
            HttpContext.Session.SetString("allNames", JsonSerializer.Serialize(dbContext.Persons.Select(p => p.Name).ToList()));
        
        List<string> guessedPersons =
            JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("guessedPersons"));
        List<Person> guessed = new List<Person>();
        foreach (var person in guessedPersons)
        {
            guessed.Add(dbContext.Persons.FirstOrDefault(p => p.Name == person));
            Console.WriteLine(person);
        }

        var gameView = new GameView(JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("allNames")), guessed);
        
        return View(gameView);
    }
    
    //[Route("/Home/PersonPage/{guessedPerson}")]
    public IActionResult Guess(string guessedPerson)
    {
        HttpContext.Session.SetInt32("attempts", HttpContext.Session.GetInt32("attempts").Value + 1);
        List<string> guessedPersons =
            JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("guessedPersons"));
        guessedPersons.Add(guessedPerson);
        HttpContext.Session.SetString("guessedPersons", JsonSerializer.Serialize(guessedPersons));
        if (HttpContext.Session.GetString("mysteryPerson").Equals(guessedPerson))
            HttpContext.Session.SetString("gameWon", "true");
        return RedirectToAction("Main");
    }
}