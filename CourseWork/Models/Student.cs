namespace CourseWork.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int BooksCount { get; set; }

    public string TicketId { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
