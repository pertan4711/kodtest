namespace LibraryService.Interfaces;

using uppgift2.service.DTOs;

public interface ILibraryService
{
    Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top = 10);
    Task<BookAvailabilityDto?> GetBookAvailabilityAsync(int bookId);
    Task<List<TopBorrowerDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int top = 10);
    Task<List<UserLoanHistoryDto>> GetUserLoanHistoryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
    Task<List<RelatedBookDto>> GetRelatedBooksAsync(int bookId, int top = 10);
    Task<ReadingSpeedDto?> GetReadingSpeedAsync(int bookId);
}
