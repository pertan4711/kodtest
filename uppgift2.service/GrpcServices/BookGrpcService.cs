using Grpc.Core;
using LibraryService.Grpc.Books;
using uppgift2.service.Models;
using uppgift2.service.Services;

namespace uppgift2.service.GrpcServices;

public class BookGrpcService : LibraryService.Grpc.Books.BookService.BookServiceBase
{
    private readonly IBookService _bookService;

    public BookGrpcService(IBookService bookService)
    {
        _bookService = bookService;
    }

    public override async Task<GetAllBooksResponse> GetAllBooks(
        GetAllBooksRequest request,
        ServerCallContext context)
    {
        var books = await _bookService.GetAllBooksAsync();

        var response = new GetAllBooksResponse();
        foreach (var book in books)
        {
            response.Books.Add(MapToBookDto(book));
        }

        return response;
    }

    public override async Task<GetBookResponse> GetBook(
        GetBookRequest request,
        ServerCallContext context)
    {
        var book = await _bookService.GetBookByIdAsync(request.Id);

        if (book == null)
        {
            return new GetBookResponse { Found = false };
        }

        return new GetBookResponse
        {
            Book = MapToBookDto(book),
            Found = true
        };
    }

    public override async Task<CreateBookResponse> CreateBook(
        CreateBookRequest request,
        ServerCallContext context)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.Isbn,
            Pages = request.Pages,
            PublishedYear = request.PublishedYear
        };

        var createdBook = await _bookService.CreateBookAsync(book);

        return new CreateBookResponse
        {
            Book = MapToBookDto(createdBook)
        };
    }

    public override async Task<UpdateBookResponse> UpdateBook(
        UpdateBookRequest request,
        ServerCallContext context)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.Isbn,
            Pages = request.Pages,
            PublishedYear = request.PublishedYear
        };

        var updatedBook = await _bookService.UpdateBookAsync(request.Id, book);

        if (updatedBook == null)
        {
            return new UpdateBookResponse { Found = false };
        }

        return new UpdateBookResponse
        {
            Book = MapToBookDto(updatedBook),
            Found = true
        };
    }

    public override async Task<DeleteBookResponse> DeleteBook(
        DeleteBookRequest request,
        ServerCallContext context)
    {
        var success = await _bookService.DeleteBookAsync(request.Id);

        return new DeleteBookResponse { Success = success };
    }

    private BookDto MapToBookDto(Book book)
    {
        var dto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Isbn = book.ISBN,
            Pages = book.Pages,
            PublishedYear = book.PublishedYear
        };

        foreach (var copy in book.Copies)
        {
            dto.Copies.Add(new BookCopyDto
            {
                Id = copy.Id,
                BookId = copy.BookId,
                CopyNumber = copy.CopyNumber
            });
        }

        return dto;
    }
}
