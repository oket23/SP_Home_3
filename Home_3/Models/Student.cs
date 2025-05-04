namespace Home_3.Models;
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BDate { get; set; }
    public int GroupId { get; set; }
    public MyGroup Group { get; set; }
}
