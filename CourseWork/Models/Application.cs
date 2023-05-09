using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace CourseWork.Models;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int StudentId { get; set; }

    public int BookId { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [ValidateNever]
    public virtual Book Book { get; set; } = null!;
    [ValidateNever]
    public virtual Student Student { get; set; } = null!;
}
