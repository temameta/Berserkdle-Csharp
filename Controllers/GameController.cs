using System.Text.Json;
using Berserkdle.Data;
using Berserkdle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Berserkdle.Controllers;

public class GameController : Controller
{
    private readonly BerserkdleDbContext _dbContext;

    public GameController(BerserkdleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Main()
    {
        var viewModel = InitializeOrGetGame();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Guess(string guessedPerson)
    {
        if (string.IsNullOrWhiteSpace(guessedPerson))
        {
            TempData["Error"] = "Введите имя персонажа";
            return RedirectToAction("Main");
        }

        var mysteryPersonName = HttpContext.Session.GetString("mysteryPerson");
        if (string.IsNullOrEmpty(mysteryPersonName))
            return RedirectToAction("NewGame");
        var guessedPersonData =
            await _dbContext.Persons.FirstOrDefaultAsync(p => p.Name.ToLower() == guessedPerson.ToLower());
        if (guessedPersonData == null)
        {
            TempData["Error"] = $"Персонаж '{guessedPerson}' не найден в базе";
            return RedirectToAction("Main");
        }

        var mysteryPerson = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Name == mysteryPersonName);
        if (mysteryPerson == null)
            return RedirectToAction("NewGame");
        var guessResult = ComparePersons(guessedPersonData, mysteryPerson);
        var guesses = GetGuessesFromSession();
        guesses.Add(guessResult);
        SaveGuessesToSession(guesses);
        var attempts = HttpContext.Session.GetInt32("attempts") ?? 0;
        HttpContext.Session.SetInt32("attempts", attempts + 1);
        if (guessedPersonData.Name.Equals(mysteryPersonName, StringComparison.OrdinalIgnoreCase))
        {
            HttpContext.Session.SetString("gameWon", "true");
            TempData["Victory"] = $"Поздравляю! Вы угадали персонажа за {attempts + 1} попыток!";
        }

        return RedirectToAction("Main");
    }

    public IActionResult NewGame()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Main");
    }

    private GameViewModel InitializeOrGetGame()
    {
        if (HttpContext.Session.GetString("mysteryPerson") == null)
            StartNewGame();
        return new GameViewModel
        {
            MysteryPerson = HttpContext.Session.GetString("mysteryPerson"),
            Attempts = HttpContext.Session.GetInt32("attempts") ?? 0,
            GameWon = HttpContext.Session.GetString("gameWon") == "true",
            AllNames = GetAllNamesFromSession(),
            Guesses = GetGuessesFromSession(),
            ErrorMessage = TempData["Error"] as string
        };
    }

    private void StartNewGame()
    {
        var allPersons = _dbContext.Persons.ToList();
        if (!allPersons.Any())
            throw new InvalidOperationException("Нет персонажей в базе данных");

        var random = new Random();
        var mysteryPerson = allPersons[random.Next(allPersons.Count)];

        HttpContext.Session.SetString("mysteryPerson", mysteryPerson.Name);
        HttpContext.Session.SetInt32("attempts", 0);
        HttpContext.Session.SetString("gameWon", "false");

        var allNames = _dbContext.Persons.Select(p => p.Name).ToList();
        HttpContext.Session.SetString("allNames", JsonSerializer.Serialize(allNames));

        SaveGuessesToSession(new List<PersonGuessResult>());
    }

    private PersonGuessResult ComparePersons(Person guessed, Person mystery)
    {
        string anyGroupsMatches, anyWeaponMatches;
        if (new HashSet<string>(guessed.Groups).SetEquals(new HashSet<string>(mystery.Groups)))
            anyGroupsMatches = "full";
        else
        {
            int equalEntries = 0;
            foreach (var guessedGroup in guessed.Groups)
            foreach (var mysteryGroup in mystery.Groups)
                if (guessedGroup == mysteryGroup)
                {
                    equalEntries++;
                    break;
                }

            anyGroupsMatches = equalEntries == 0 ? "none" : "semi";
        }

        if (new HashSet<string>(guessed.Weapons).SetEquals(new HashSet<string>(mystery.Weapons)))
            anyWeaponMatches = "full";
        else
        {
            int equalEntries = 0;
            foreach (var guessedWeapon in guessed.Weapons)
            foreach (var mysteryWeapon in mystery.Weapons)
                if (guessedWeapon == mysteryWeapon)
                {
                    equalEntries++;
                    break;
                }

            anyWeaponMatches = equalEntries == 0 ? "none" : "semi";
        }


        return new PersonGuessResult
        {
            Name = guessed.Name,
            NameMatches = guessed.Name.Equals(mystery.Name, StringComparison.OrdinalIgnoreCase),
            Gender = guessed.Gender,
            GenderMatches = guessed.Gender == mystery.Gender,
            Species = guessed.Species,
            SpeciesMatches = guessed.Species == mystery.Species,
            FirstArc = guessed.FirstArc,
            FirstArcMatches = guessed.FirstArc == mystery.FirstArc,
            Weapons = guessed.Weapons,
            AnyWeaponMatches = anyWeaponMatches,
            Groups = guessed.Groups,
            AnyGroupMatches = anyGroupsMatches
        };
    }

    private List<PersonGuessResult> GetGuessesFromSession()
    {
        var guessesJson = HttpContext.Session.GetString("guesses");
        return string.IsNullOrEmpty(guessesJson)
            ? new List<PersonGuessResult>()
            : JsonSerializer.Deserialize<List<PersonGuessResult>>(guessesJson);
    }

    private void SaveGuessesToSession(List<PersonGuessResult> guesses)
    {
        HttpContext.Session.SetString("guesses", JsonSerializer.Serialize(guesses));
    }

    private List<string> GetAllNamesFromSession()
    {
        var allNamesJson = HttpContext.Session.GetString("allNames");
        return string.IsNullOrEmpty(allNamesJson)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(allNamesJson);
    }
}