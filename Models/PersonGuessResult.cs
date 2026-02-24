namespace Berserkdle.Models;

public class PersonGuessResult
{
    public string Name { get; set; }
    public bool NameMatches { get; set; }
    public string Gender { get; set; }
    public bool GenderMatches { get; set; }
    public string Species { get; set; }
    public bool SpeciesMatches { get; set; }
    public string FirstArc { get; set; }
    public bool FirstArcMatches { get; set; }
    public List<string> Weapons { get; set; } = new();
    public bool AnyWeaponMatches { get; set; }
    public List<string> Groups { get; set; } = new();
    public bool AnyGroupMatches { get; set; }
}