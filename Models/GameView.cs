using Berserkdle.Data;

namespace Berserkdle.Models;

public class GameView(List<string> AllNames, List<Person> GuessedPersons)
{
    public List<string> AllNames { get; set; }
    public List<Person> GuessedPersons { get; set; }
}