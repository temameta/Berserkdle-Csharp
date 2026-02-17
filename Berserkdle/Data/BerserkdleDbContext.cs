using Berserkdle.Models;
using Microsoft.EntityFrameworkCore;

namespace Berserkdle.Data;

public class BerserkdleDbContext : DbContext
{
    public DbSet<Person>  Persons { get; set; }
    
    public BerserkdleDbContext(DbContextOptions<BerserkdleDbContext> options) : base(options)
    {}
}