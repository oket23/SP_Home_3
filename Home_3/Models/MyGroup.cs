namespace Home_3.Models;

public class MyGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Course { get; set; }
    public ICollection<Student> Students { get; set; }
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    

}
