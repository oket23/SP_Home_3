using Home_3.Data;
using Home_3.DTO;
using Home_3.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_3.Repositories;

public class GroupRepository
{
    private readonly Home3Context _context;

    public GroupRepository()
    {
        _context = new Home3Context();
    }

    public async Task AddGroupAsync(MyGroup group)
    {
        if(group != null && IsValidGroupCourse(group.Course) && IsValidGroupName(group.Name))
        {
            group.Teacher = await _context.Teachers.FindAsync(group.TeacherId);
            if (group.Teacher != null)
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                Console.WriteLine("\nGroup successfully added!");
                return;
            }
            Console.WriteLine("\nBad teacher!");
        }
    }
    public async Task AddStudentToGroupAsync(int groupId,int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var group = await _context.Groups.
            Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id.Equals(groupId));

        if (group == null)
        {
            Console.WriteLine("\nBad group id");
            return;
        }
        if (student == null)
        {
            Console.WriteLine("\nBad student id");
            return;
        }
        if (!group.Students.Any(x => x.Id == studentId))
        {
            group.Students.Add(student);
            await _context.SaveChangesAsync();
            Console.WriteLine("\nStudent successfully added!");
            return;
        }
        Console.WriteLine("\nThis student is already in this group!");
    }
    public async Task<List<GroupWithStudentsDto>> GetAllGroupsAsync()
    {
        return await _context.Groups
            .Include(x => x.Students)
            .Select(x => new GroupWithStudentsDto
            {
                GroupName = x.Name,
                StudentNames = x.Students.Select(x => $"{x.FirstName} {x.LastName}").ToList()
            })
            .ToListAsync();
    }
    public async Task TransferStudentAsync(int groupXId,int studentId, int groupYId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var firstGroup = await _context.Groups
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id.Equals(groupXId));
        var secondGroup = await _context.Groups
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id.Equals(groupYId));

        if (student != null && firstGroup != null && secondGroup != null)
        {
            if (firstGroup.Students.Any(x => x.Id.Equals(studentId)) && !secondGroup.Students.Any(x => x.Id.Equals(studentId)))
            {
                firstGroup.Students.Remove(student);
                secondGroup.Students.Add(student);

                await _context.SaveChangesAsync();
            }
        }
    }//мій метод AddStudentToGroupAsync працює аналогічно цьому,тому я використовою його
    private bool IsValidGroupName(string groupName)
    {
        return !string.IsNullOrEmpty(groupName.Trim());
    }
    private bool IsValidGroupCourse(int course)
    {
        return course > 0 && course <= 5;
    }
}
