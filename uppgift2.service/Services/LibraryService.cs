using LibraryService.Interfaces;
using Microsoft.EntityFrameworkCore;
using uppgift2.service.Data;
using uppgift2.service.DTOs;
using uppgift2.service.Models;

namespace uppgift2.service.Services;

public class LibraryService : ILibraryService
{
    private readonly LibraryContext _context;

    public LibraryService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top = 10)
    {
        return await _context.Loans
            .GroupBy(l => l.BookCopy.BookId)
            .Select(g => new MostBorrowedBookDto
            {
                BookId = g.Key,
                Title = g.First().BookCopy.Book.Title,
                Author = g.First().BookCopy.Book.Author,
                TotalLoans = g.Count()
            })
            .OrderByDescending(x => x.TotalLoans)
            .Take(top)
            .ToListAsync();
    }

    public async Task<BookAvailabilityDto?> GetBookAvailabilityAsync(int bookId)
    {
        var book = await _context.Books
            .Include(b => b.Copies)
            .ThenInclude(c => c.Loans)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null)
            return null;

        var totalCopies = book.Copies.Count;
        var borrowedCopies = book.Copies.Count(c => c.Loans.Any(l => l.ReturnDate == null));
        var availableCopies = totalCopies - borrowedCopies;

        return new BookAvailabilityDto
        {
            BookId = book.Id,
            Title = book.Title,
            TotalCopies = totalCopies,
            AvailableCopies = availableCopies,
            BorrowedCopies = borrowedCopies
        };
    }

    public async Task<List<TopBorrowerDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int top = 10)
    {
        return await _context.Loans
            .Where(l => l.LoanDate >= startDate && l.LoanDate <= endDate)
            .GroupBy(l => l.UserId)
            .Select(g => new TopBorrowerDto
            {
                UserId = g.Key,
                Name = g.First().User.Name,
                Email = g.First().User.Email,
                TotalLoans = g.Count()
            })
            .OrderByDescending(x => x.TotalLoans)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<UserLoanHistoryDto>> GetUserLoanHistoryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Loans
            .Include(l => l.BookCopy)
            .ThenInclude(bc => bc.Book)
            .Where(l => l.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(l => l.LoanDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(l => l.LoanDate <= endDate.Value);

        return await query
            .OrderByDescending(l => l.LoanDate)
            .Select(l => new UserLoanHistoryDto
            {
                LoanId = l.Id,
                BookTitle = l.BookCopy.Book.Title,
                Author = l.BookCopy.Book.Author,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate
            })
            .ToListAsync();
    }

    public async Task<List<RelatedBookDto>> GetRelatedBooksAsync(int bookId, int top = 10)
    {
        // Hitta användare som lånat den angivna boken
        var usersWhoBorrowedBook = await _context.Loans
            .Where(l => l.BookCopy.BookId == bookId)
            .Select(l => l.UserId)
            .Distinct()
            .ToListAsync();

        if (!usersWhoBorrowedBook.Any())
            return new List<RelatedBookDto>();

        // Hitta andra böcker som dessa användare lånat
        return await _context.Loans
            .Where(l => usersWhoBorrowedBook.Contains(l.UserId) && l.BookCopy.BookId != bookId)
            .GroupBy(l => l.BookCopy.BookId)
            .Select(g => new RelatedBookDto
            {
                BookId = g.Key,
                Title = g.First().BookCopy.Book.Title,
                Author = g.First().BookCopy.Book.Author,
                SharedBorrowers = g.Select(l => l.UserId).Distinct().Count()
            })
            .OrderByDescending(x => x.SharedBorrowers)
            .Take(top)
            .ToListAsync();
    }

    public async Task<ReadingSpeedDto?> GetReadingSpeedAsync(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
            return null;

        // Hitta alla slutförda lån för denna bok
        var completedLoans = await _context.Loans
            .Where(l => l.BookCopy.BookId == bookId && l.ReturnDate.HasValue)
            .ToListAsync();

        if (!completedLoans.Any())
            return new ReadingSpeedDto
            {
                BookId = book.Id,
                Title = book.Title,
                Pages = book.Pages,
                AveragePagesPerDay = 0,
                CompletedLoans = 0
            };

        // Beräkna genomsnittlig lästid i dagar
        var averageReadingDays = completedLoans
            .Average(l => (l.ReturnDate!.Value - l.LoanDate).TotalDays);

        var pagesPerDay = averageReadingDays > 0 ? book.Pages / averageReadingDays : 0;

        return new ReadingSpeedDto
        {
            BookId = book.Id,
            Title = book.Title,
            Pages = book.Pages,
            AveragePagesPerDay = Math.Round(pagesPerDay, 2),
            CompletedLoans = completedLoans.Count
        };
    }
}
