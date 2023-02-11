using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CFSL;

public class Book
{
    public int BookId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public override string ToString()
    {
        return $"Cím: {Name}, Író: {Author}, Stílus: {string.Join(",",Genres)}";
    }
}
