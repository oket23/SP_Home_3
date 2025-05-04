using System.ComponentModel.DataAnnotations.Schema;

namespace Home_3.Models;

public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfStart { get; set; }
    public decimal Salary { get; set; }
    public string Subject { get; set; }
    public string CalendarPlanningPath { get; set; }
    [NotMapped]
    public string? CalendarPlanningDate { get; set; }
    public ICollection<MyGroup> Groups { get; set; }
}
