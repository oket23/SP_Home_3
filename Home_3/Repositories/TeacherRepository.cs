using Home_3.Data;
using Home_3.DTO;
using Home_3.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_3.Repositories;

public class TeacherRepository
{
    private readonly Home3Context _context;
    public TeacherRepository()
    {
        _context = new Home3Context();
    }
    public async Task AddTeacherAsync(Teacher teacher)
    {
        try
        {
            if (teacher != null && IsValidTeacherFirstName(teacher.FirstName) && IsValidTeacherLastName(teacher.LastName) && IsValidTeacherDateOfStart(teacher.DateOfStart) && IsValidTeacherSalary(teacher.Salary) && IsValidTeacherSubject(teacher.Subject))
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TeacherCalendar");
                var fileName = $"{teacher.FirstName}_{teacher.LastName}_{teacher.Subject}.txt";

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                
                var filePath = Path.Combine(directoryPath, fileName);

                await WriteTeacherDateToFileAsync(filePath, $"{teacher.FirstName} {teacher.LastName} Subject: {teacher.Subject}");

                teacher.CalendarPlanningPath = Path.Combine("TeacherCalendar", fileName);
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();

                Console.WriteLine("\nTeacher successfully added!");
            }
            else
            {
                Console.WriteLine("\nBad teachers!");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to add teacher or create calendar file!", ex);
        }
    }

    
    public async Task<Teacher> GetTeacherByIdAsync(int teacherId)
    {
        var teacher = await _context.Teachers
        .Include(x => x.Groups)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id.Equals(teacherId));
        
        if (teacher != null && File.Exists(teacher.CalendarPlanningPath))
        {
            try
            {
                teacher.CalendarPlanningDate = await GetTeacherDateAsync(teacher.CalendarPlanningPath);
                return teacher;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message,ex);
            }
        }
        return null;
    }
    public async Task<List<TeacherDTO>> GetAllTeacherAsync()
    {
        return await _context.Teachers
            .Select(x => new TeacherDTO
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
            })
            .ToListAsync();
    }
    
    public async Task AddGroupForTeacherAsync(int teacherId,int groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        var teacher = await _context.Teachers
            .Include(x => x.Groups)
            .FirstOrDefaultAsync(x => x.Id.Equals(teacherId));
        if(group == null)
        {
            Console.WriteLine("\nBad group id!");
            return;
        }
        if (teacher == null)
        {
            Console.WriteLine("\nBad teacher id!");
            return;
        }
        if (!teacher.Groups.Any(g => g.Id .Equals(groupId)))
        {
            teacher.Groups.Add(group);
            await _context.SaveChangesAsync();
            Console.WriteLine("\nGroup successfully added!");
            return;
        }
        Console.WriteLine("\nThis teacher already has this group!");
    }
    
    public async Task WriteTeacherDateToFileAsync(string path,string teacherDate)
    {
        using(var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using(var sw = new StreamWriter(fs))
            {
                await sw.WriteLineAsync(teacherDate);
            }
        }
    }
    private async Task<string> GetTeacherDateAsync(string path)
    {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (var sr = new StreamReader(fs))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
    private bool IsValidTeacherFirstName(string firstName)
    {
        return !string.IsNullOrEmpty(firstName.Trim());
    }
    private bool IsValidTeacherLastName(string lastName)
    {
        return !string.IsNullOrEmpty(lastName.Trim());
    }
    private bool IsValidTeacherDateOfStart(DateTime date)
    {
       return DateTime.Now >= date;
    }
    private bool IsValidTeacherSalary(decimal salary)
    {
        return salary > 0;
    }
    private bool IsValidTeacherSubject(string subject)
    {
        return !string.IsNullOrEmpty(subject.Trim());
    }
    private bool IsValidTeacherGrops(ICollection<MyGroup> groups)
    {
        return groups != null;
    }
}
