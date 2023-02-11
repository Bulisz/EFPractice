using Microsoft.EntityFrameworkCore;

namespace CFSL;

public class Program
{
    //Code-First SGLite
    static void Main()
    {
        DatabaseCreate();

        AddRawData();
        PrintContentByBook();

        AddCompactDataWithNewGenres();
        PrintContentByBook();

        AddCompactDataWithExistedGenres();
        PrintContentByBook();

        //----------Speckó metódusok: Add-Book -Author -Genre

        AddNewAuthor("Rider Haggard");
        AddNewGenre("Adventure story");

        using (CFSLContext db = new())
        {
            Author? autToNewBook = db.Authors.FirstOrDefault(aut => aut.Name == "Rider Haggard");
            Genre? genToNewBook = db.Genres.FirstOrDefault(gen => gen.Name == "Adventure story");
            if(autToNewBook is not null && genToNewBook is not null)
            {
                AddNewBook("Sárga isten", autToNewBook, genToNewBook);
                AddNewBook("Ayesha", autToNewBook);
            }
        }
        PrintContentByBook();

        using (CFSLContext db = new())
        {
            Book? book = db.Books.FirstOrDefault(bok => bok.Name == "Sárga isten");
            Genre? gen1ToAdd = db.Genres.FirstOrDefault(gen => gen.Name == "Fantasy");
            Genre? gen2ToAdd = db.Genres.FirstOrDefault(gen => gen.Name == "Historical");
            if (book is not null && gen1ToAdd is not null && gen2ToAdd is not null)
            {
                AddGenreToBook(book, new Genre[] { gen1ToAdd, gen2ToAdd });
            }
        }
        PrintContentByBook();

        PrintContentByGenre();
        PrintContentByAuthor();
    }

    public static void PrintContentByAuthor()
    {
        using CFSLContext db = new();
        db.Authors.Include(author => author.Books)
                .ThenInclude(book => book.Genres)
                .OrderBy(author => author.Name)
                .ForEachAsync(author =>
                {
                    Console.WriteLine(author);
                    author.Books.ToList().ForEach(book => Console.WriteLine("--" + book));
                });
        Console.WriteLine(new string('-', 60));
    }

    public static void PrintContentByGenre()
    {
        using CFSLContext db = new();
        db.Genres.Include(genre => genre.Books)
                .ThenInclude(book => book.Author)
                .OrderBy(genre => genre.Name)
                .ForEachAsync(genre =>
                {
                    Console.WriteLine(genre);
                    genre.Books.ToList().ForEach(book => Console.WriteLine("--"+book));
                });
        Console.WriteLine(new string('-', 60));
    }

    public static void PrintContentByBook()
    {
        using CFSLContext db = new ();
        db.Books.Include(book => book.Author)
                .Include(book => book.Genres)
                .ForEachAsync(Console.WriteLine);
        Console.WriteLine(new string('-',60));
    }

    public static void DatabaseCreate()
    {
        using CFSLContext db = new();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
    public static void AddRawData()
    {
        //Minden adat külön felvitt
        using CFSLContext db = new ();

        Author aut1 = new() { Name = "J.R.R.Tolkien" };
        Author aut2 = new() { Name = "Isac Asimov" };

        Genre gen1 = new() { Name = "Fantasy" };
        Genre gen2 = new() { Name = "Sci-fi" };

        Book boo1 = new() { Name = "Gyűrűk ura", Author = aut1 };
        Book boo2 = new() { Name = "Alapítvány", Author = aut2 };
        boo1.Genres.Add(gen1);
        boo2.Genres.Add(gen2);

        db.AddRange(aut1, aut2, gen1, gen2, boo1, boo2);
        db.SaveChanges();
    }

    public static void AddCompactDataWithNewGenres()
    {
        //Író és könyv együtt felvitt új stílussal
        using CFSLContext db = new();
        Genre gen1 = new() { Name = "Comedy" };
        Genre gen2 = new() { Name = "Historical" };
        Book boo1 = new() { Name = "Meg se kínáltak", Author = new Author() { Name = "Bödőcs Tibor" } };
        Book boo2 = new() { Name = "Hét év tibetben", Author = new Author() { Name = "Heinrich Harrer" } };
        boo1.Genres.Add(gen1);
        boo2.Genres.Add(gen2);

        db.AddRange(boo1, boo2);
        db.SaveChanges();
    }

    public static void AddCompactDataWithExistedGenres()
    {
        //Író és könyv együtt felvitt már meglevő stílussal
        using CFSLContext db = new();

        Genre gen1 = db.Genres.First(genre => genre.Name == "Fantasy");
        Genre gen2 = db.Genres.First(genre => genre.Name == "Sci-fi");
        Book boo1 = new() { Name = "Conan, a barbár", Author = new Author() { Name = "Robert E. Howard" } };
        Book boo2 = new() { Name = "2001: Űrodüsszeia", Author = new Author() { Name = "Arthur C. Clarke" } };
        boo1.Genres.Add(gen1);
        boo2.Genres.Add(gen2);

        db.AddRange( boo1, boo2);
        db.SaveChanges();
    }

    public static void AddNewGenre(string genre)
    {
        using CFSLContext db = new ();
        db.Genres.Add(new Genre() { Name = genre });
        db.SaveChanges();
    }
    public static void AddNewAuthor(string Name)
    {
        using CFSLContext db = new ();
        db.Authors.Add(new Author() { Name = Name });
        db.SaveChanges();
    }

    public static void AddNewBook(string Name, Author author)
    {
        using CFSLContext db = new ();
        Author autToNewBook = db.Authors.Find(author.AuthorId)!;
        Book newBook = new() { Name = Name, Author = autToNewBook };
        db.Books.Add(newBook);

        db.SaveChanges();
    }
    public static void AddNewBook(string Name, Author author, Genre genre)
    {
        using CFSLContext db = new ();

        Author autToNewBook = db.Authors.Find(author.AuthorId)!;
        Genre genToNewBook = db.Genres.Find(genre.GenreId)!;
        Book newBook = new () { Name = Name ,Author = autToNewBook };

        newBook.Genres.Add(genToNewBook);
        db.Books.Add(newBook);
        
        db.SaveChanges();
    }

    public static void AddGenreToBook(Book book, params Genre[] genre)
    {
        using CFSLContext db = new ();
        Book actBook = db.Books.Find(book.BookId)!;
        genre.ToList().ForEach(actBook.Genres.Add);
        db.SaveChanges();
    }

}