using Microsoft.EntityFrameworkCore;

namespace CFSL;

public class CFSLContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\Users\\Bula András\\source\\repos\\EFPractice\\CFSL\\cfsl.db");
    }
}
