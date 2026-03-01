using Grpc.Core;
using LibraryService.Grpc.Library;
using LibraryService.Interfaces;
using uppgift2.service.Services;

namespace uppgift2.service.GrpcServices;

public class LibraryGrpcService : LibraryService.Grpc.Library.LibraryService.LibraryServiceBase
{
    private readonly ILibraryService _libraryService;

    public LibraryGrpcService(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public override async Task<MostBorrowedResponse> GetMostBorrowedBooks(
        MostBorrowedRequest request,
        ServerCallContext context)
    {
        var result = await _libraryService.GetMostBorrowedBooksAsync(request.Top);

        var response = new MostBorrowedResponse();
        foreach (var book in result)
        {
            response.Books.Add(new MostBorrowedBook
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                TotalLoans = book.TotalLoans
            });
        }

        return response;
    }

    public override async Task<BookAvailabilityResponse> GetBookAvailability(
        BookAvailabilityRequest request,
        ServerCallContext context)
    {
        var result = await _libraryService.GetBookAvailabilityAsync(request.BookId);

        if (result == null)
        {
            return new BookAvailabilityResponse { Found = false };
        }

        return new BookAvailabilityResponse
        {
            BookId = result.BookId,
            Title = result.Title,
            TotalCopies = result.TotalCopies,
            AvailableCopies = result.AvailableCopies,
            BorrowedCopies = result.BorrowedCopies,
            Found = true
        };
    }

    public override async Task<TopBorrowersResponse> GetTopBorrowers(
        TopBorrowersRequest request,
        ServerCallContext context)
    {
        var startDate = DateTime.Parse(request.StartDate);
        var endDate = DateTime.Parse(request.EndDate);

        var result = await _libraryService.GetTopBorrowersAsync(startDate, endDate, request.Top);

        var response = new TopBorrowersResponse();
        foreach (var borrower in result)
        {
            response.Borrowers.Add(new TopBorrower
            {
                UserId = borrower.UserId,
                Name = borrower.Name,
                Email = borrower.Email,
                TotalLoans = borrower.TotalLoans
            });
        }

        return response;
    }

    public override async Task<UserLoanHistoryResponse> GetUserLoanHistory(
        UserLoanHistoryRequest request,
        ServerCallContext context)
    {
        DateTime? startDate = string.IsNullOrEmpty(request.StartDate) ? null : DateTime.Parse(request.StartDate);
        DateTime? endDate = string.IsNullOrEmpty(request.EndDate) ? null : DateTime.Parse(request.EndDate);

        var result = await _libraryService.GetUserLoanHistoryAsync(request.UserId, startDate, endDate);

        var response = new UserLoanHistoryResponse();
        foreach (var loan in result)
        {
            var userLoan = new UserLoan
            {
                LoanId = loan.LoanId,
                BookTitle = loan.BookTitle,
                Author = loan.Author,
                LoanDate = loan.LoanDate.ToString("yyyy-MM-dd")
            };

            if (loan.ReturnDate.HasValue)
            {
                userLoan.ReturnDate = loan.ReturnDate.Value.ToString("yyyy-MM-dd");
            }

            response.Loans.Add(userLoan);
        }

        return response;
    }

    public override async Task<RelatedBooksResponse> GetRelatedBooks(
        RelatedBooksRequest request,
        ServerCallContext context)
    {
        var result = await _libraryService.GetRelatedBooksAsync(request.BookId, request.Top);

        var response = new RelatedBooksResponse();
        foreach (var book in result)
        {
            response.Books.Add(new RelatedBook
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                SharedBorrowers = book.SharedBorrowers
            });
        }

        return response;
    }

    public override async Task<ReadingSpeedResponse> GetReadingSpeed(
        ReadingSpeedRequest request,
        ServerCallContext context)
    {
        var result = await _libraryService.GetReadingSpeedAsync(request.BookId);

        if (result == null)
        {
            return new ReadingSpeedResponse { Found = false };
        }

        return new ReadingSpeedResponse
        {
            BookId = result.BookId,
            Title = result.Title,
            Pages = result.Pages,
            AveragePagesPerDay = result.AveragePagesPerDay,
            CompletedLoans = result.CompletedLoans,
            Found = true
        };
    }
}
