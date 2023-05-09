namespace CourseWork.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int RealeseYear { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
