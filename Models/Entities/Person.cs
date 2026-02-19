using System.ComponentModel.DataAnnotations;

namespace Berserkdle.Models;

public class Person
{
    public int Id { get; set; }
    
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Gender { get; set; }

    [Required]
    public string? FirstArc { get; set; }
    
    [Required]
    public string? Species { get; set; }
    
    [Required] 
    public List<string>? Weapons { get; set; }
    
    [Required] 
    public List<string>? Groups { get; set; }
}