using Berserkdle.Data;

namespace Berserkdle.Models;

public class GameViewModel
{
    public List<string> AllNames { get; set; } = new();
    public List<PersonGuessResult> Guesses { get; set; } = new();
    public int Attempts { get; set; }
    public bool GameWon { get; set; }
    public string MysteryPerson { get; set; }
    public string ErrorMessage { get; set; }
}