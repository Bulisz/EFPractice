using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CFMS;

public class CFMSContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=cfsm;User Id=sa;Password=Password123!;TrustServerCertificate=True");

        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }
}
