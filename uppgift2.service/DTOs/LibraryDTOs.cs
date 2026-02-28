namespace uppgift2.service.DTOs;

public class MostBorrowedBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int TotalLoans { get; set; }
}

public class BookAvailabilityDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public int BorrowedCopies { get; set; }
}

public class TopBorrowerDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TotalLoans { get; set; }
}

public class UserLoanHistoryDto
{
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}

public class RelatedBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int SharedBorrowers { get; set; }
}

public class ReadingSpeedDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Pages { get; set; }
    public double AveragePagesPerDay { get; set; }
    public int CompletedLoans { get; set; }
}
