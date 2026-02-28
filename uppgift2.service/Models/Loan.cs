namespace uppgift2.service.Models;

public class Loan
{
    public int Id { get; set; }
    public int BookCopyId { get; set; }
    public BookCopy BookCopy { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
