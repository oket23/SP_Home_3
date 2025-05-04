using Home_3.Data;
using Home_3.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_3.Repositories;

public class StudentRepository
{
    private readonly Home3Context _context;
    public StudentRepository()
    {
        _context = new Home3Context();
    }

    public async Task AddStudentAsync(Student student,string groupName)
    {
        if (student != null && IsValidFirstName(student.FirstName) && IsValidLastName(student.LastName))
        {
            student.Group = await _context.Groups.FirstOrDefaultAsync(x => x.Name.Equals(groupName));
            if(student.Group != null)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                Console.WriteLine("\nStudent successfully added!");
                return;
            }
        }
        Console.WriteLine("\nBad student!");
    }

    public async Task<List<Student>> GetAllStudentAsync()
    {
        return await _context.Students
            .Include(x => x.Group)
            .ToListAsync();
    }
    public async Task<List<Student>> GetAllStudentForGroupAsync(string groupName)
    {
        return await _context.Students
            .Include(x => x.Group)
            .Where(x => x.Group.Name.Equals(groupName))
            .ToListAsync();
    }

    public async Task DeleteStudentByIdAsync(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            Console.WriteLine("\nStudent successfully deleted!");
        }
        else
        {
            Console.WriteLine("\nStudent not found!");
        } 
    }

    public async Task UpdateStudentAsync(int oldId,Student student,string groupName)
    {
        var oldStudent = await _context.Students.FindAsync(oldId);
        if (oldStudent == null)
    {
            Console.WriteLine("\nStudent not found!");
            return;
        }
        
        if (student != null && IsValidFirstName(student.FirstName) && IsValidLastName(student.LastName))
        {
            student.Group = await _context.Groups.FirstOrDefaultAsync(x => x.Name.Equals(groupName));
            if (student.Group != null)
            {
                oldStudent.FirstName = student.FirstName;
                oldStudent.LastName = student.LastName;
                oldStudent.BDate = student.BDate;
                oldStudent.Group = student.Group;

                await _context.SaveChangesAsync();
                Console.WriteLine("\nStudent successfully updeted!");
            }
            else
            {
                Console.WriteLine("\nBad group name!");
                return;
            }
        }
        else
        {
            Console.WriteLine("\nInvalid student data!");
        }
    }

    private bool IsValidFirstName(string firstName)
    {
        return !string.IsNullOrEmpty(firstName.Trim());
    }
    private bool IsValidLastName(string lastName)
    {
        return !string.IsNullOrEmpty(lastName.Trim());
    }
    private bool IsValidGroup(MyGroup group)
    {
        return group != null;
    }

}
