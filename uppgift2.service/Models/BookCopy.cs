namespace uppgift2.service.Models;

public class BookCopy
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public string CopyNumber { get; set; } = string.Empty;
    
    public ICollection<Loan> Loans { get; set; } = [];
}
