using Microsoft.EntityFrameworkCore;
using uppgift2.service.Models;

namespace uppgift2.service.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Books
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Harry Potter och De vises sten", Author = "J.K. Rowling", ISBN = "978-91-29-65843-7", Pages = 335, PublishedYear = 1997 },
            new Book { Id = 2, Title = "Sagan om ringen: Härskarringen", Author = "J.R.R. Tolkien", ISBN = "978-91-0-012345-6", Pages = 423, PublishedYear = 1954 },
            new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "978-91-7011-234-5", Pages = 328, PublishedYear = 1949 },
            new Book { Id = 4, Title = "Stolthet och fördom", Author = "Jane Austen", ISBN = "978-91-0-011122-3", Pages = 432, PublishedYear = 1813 },
            new Book { Id = 5, Title = "Hungerspelen", Author = "Suzanne Collins", ISBN = "978-91-7429-123-4", Pages = 374, PublishedYear = 2008 },
            new Book { Id = 6, Title = "Bröderna Lejonhjärta", Author = "Astrid Lindgren", ISBN = "978-91-29-65432-1", Pages = 224, PublishedYear = 1973 },
            new Book { Id = 7, Title = "Mästerdetektiven Blomkvist", Author = "Astrid Lindgren", ISBN = "978-91-29-65111-2", Pages = 156, PublishedYear = 1946 },
            new Book { Id = 8, Title = "Hobbit", Author = "J.R.R. Tolkien", ISBN = "978-91-0-012222-2", Pages = 310, PublishedYear = 1937 }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Anna Andersson", Email = "anna@example.com", MemberSince = new DateTime(2020, 1, 15) },
            new User { Id = 2, Name = "Erik Eriksson", Email = "erik@example.com", MemberSince = new DateTime(2019, 5, 22) },
            new User { Id = 3, Name = "Maria Svensson", Email = "maria@example.com", MemberSince = new DateTime(2021, 3, 10) },
            new User { Id = 4, Name = "Johan Karlsson", Email = "johan@example.com", MemberSince = new DateTime(2020, 8, 5) },
            new User { Id = 5, Name = "Lisa Nilsson", Email = "lisa@example.com", MemberSince = new DateTime(2022, 1, 20) }
        );

        // Seed BookCopies
        modelBuilder.Entity<BookCopy>().HasData(
            // Harry Potter - 3 exemplar
            new BookCopy { Id = 1, BookId = 1, CopyNumber = "HP-001" },
            new BookCopy { Id = 2, BookId = 1, CopyNumber = "HP-002" },
            new BookCopy { Id = 3, BookId = 1, CopyNumber = "HP-003" },
            // Sagan om ringen - 2 exemplar
            new BookCopy { Id = 4, BookId = 2, CopyNumber = "LOTR-001" },
            new BookCopy { Id = 5, BookId = 2, CopyNumber = "LOTR-002" },
            // 1984 - 2 exemplar
            new BookCopy { Id = 6, BookId = 3, CopyNumber = "1984-001" },
            new BookCopy { Id = 7, BookId = 3, CopyNumber = "1984-002" },
            // Stolthet och fördom - 1 exemplar
            new BookCopy { Id = 8, BookId = 4, CopyNumber = "PP-001" },
            // Hungerspelen - 3 exemplar
            new BookCopy { Id = 9, BookId = 5, CopyNumber = "HG-001" },
            new BookCopy { Id = 10, BookId = 5, CopyNumber = "HG-002" },
            new BookCopy { Id = 11, BookId = 5, CopyNumber = "HG-003" },
            // Bröderna Lejonhjärta - 2 exemplar
            new BookCopy { Id = 12, BookId = 6, CopyNumber = "BL-001" },
            new BookCopy { Id = 13, BookId = 6, CopyNumber = "BL-002" },
            // Mästerdetektiven Blomkvist - 1 exemplar
            new BookCopy { Id = 14, BookId = 7, CopyNumber = "MB-001" },
            // Hobbit - 2 exemplar
            new BookCopy { Id = 15, BookId = 8, CopyNumber = "HOB-001" },
            new BookCopy { Id = 16, BookId = 8, CopyNumber = "HOB-002" }
        );

        // Seed Loans (historiska och aktiva lån)
        modelBuilder.Entity<Loan>().HasData(
            // Anna (UserId: 1) - aktiv läsare
            new Loan { Id = 1, BookCopyId = 1, UserId = 1, LoanDate = new DateTime(2024, 1, 5), ReturnDate = new DateTime(2024, 1, 20) },
            new Loan { Id = 2, BookCopyId = 4, UserId = 1, LoanDate = new DateTime(2024, 2, 1), ReturnDate = new DateTime(2024, 2, 25) },
            new Loan { Id = 3, BookCopyId = 9, UserId = 1, LoanDate = new DateTime(2024, 3, 5), ReturnDate = new DateTime(2024, 3, 18) },
            new Loan { Id = 4, BookCopyId = 6, UserId = 1, LoanDate = new DateTime(2024, 11, 15), ReturnDate = null }, // Aktuellt lån

            // Erik (UserId: 2) - läser mycket
            new Loan { Id = 5, BookCopyId = 2, UserId = 2, LoanDate = new DateTime(2024, 1, 10), ReturnDate = new DateTime(2024, 1, 25) },
            new Loan { Id = 6, BookCopyId = 5, UserId = 2, LoanDate = new DateTime(2024, 1, 28), ReturnDate = new DateTime(2024, 2, 20) },
            new Loan { Id = 7, BookCopyId = 10, UserId = 2, LoanDate = new DateTime(2024, 2, 22), ReturnDate = new DateTime(2024, 3, 8) },
            new Loan { Id = 8, BookCopyId = 12, UserId = 2, LoanDate = new DateTime(2024, 3, 10), ReturnDate = new DateTime(2024, 3, 22) },
            new Loan { Id = 9, BookCopyId = 15, UserId = 2, LoanDate = new DateTime(2024, 4, 1), ReturnDate = new DateTime(2024, 4, 20) },
            new Loan { Id = 10, BookCopyId = 8, UserId = 2, LoanDate = new DateTime(2024, 11, 20), ReturnDate = null }, // Aktuellt lån

            // Maria (UserId: 3)
            new Loan { Id = 11, BookCopyId = 3, UserId = 3, LoanDate = new DateTime(2024, 2, 5), ReturnDate = new DateTime(2024, 2, 19) },
            new Loan { Id = 12, BookCopyId = 11, UserId = 3, LoanDate = new DateTime(2024, 3, 1), ReturnDate = new DateTime(2024, 3, 14) },
            new Loan { Id = 13, BookCopyId = 13, UserId = 3, LoanDate = new DateTime(2024, 11, 25), ReturnDate = null }, // Aktuellt lån

            // Johan (UserId: 4)
            new Loan { Id = 14, BookCopyId = 1, UserId = 4, LoanDate = new DateTime(2024, 2, 1), ReturnDate = new DateTime(2024, 2, 14) },
            new Loan { Id = 15, BookCopyId = 9, UserId = 4, LoanDate = new DateTime(2024, 2, 16), ReturnDate = new DateTime(2024, 3, 2) },
            new Loan { Id = 16, BookCopyId = 16, UserId = 4, LoanDate = new DateTime(2024, 11, 28), ReturnDate = null }, // Aktuellt lån

            // Lisa (UserId: 5)
            new Loan { Id = 17, BookCopyId = 6, UserId = 5, LoanDate = new DateTime(2024, 1, 15), ReturnDate = new DateTime(2024, 2, 5) },
            new Loan { Id = 18, BookCopyId = 14, UserId = 5, LoanDate = new DateTime(2024, 3, 10), ReturnDate = new DateTime(2024, 3, 18) },
            new Loan { Id = 19, BookCopyId = 2, UserId = 5, LoanDate = new DateTime(2024, 11, 30), ReturnDate = null } // Aktuellt lån
        );
    }
}
