using Home_3.Data;
using Home_3.Models;
using Home_3.Repositories;
using Microsoft.Identity.Client;

namespace Home_3;

public class Program
{
    static Home3Context context = new Home3Context();
    static StudentRepository studentRepository = new StudentRepository();
    static TeacherRepository teacherRepository = new TeacherRepository();
    static GroupRepository groupRepository = new GroupRepository();

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Write("Menu:\n1.Work with student\n2.Work with teacher\n3.Work with group\n4.Exit\nChoise option: ");

            if(!int.TryParse(Console.ReadLine(),out int option))
            {
                Console.WriteLine("\nChoise correct option!\n");
                continue;
            }
            switch (option)
            {
                case 1:
                    await WorkWithStudent();
                    break;
                case 2:
                    await WorkWithTeacher();
                    break;
                case 3:
                    await WorkWithGroup();
                    break;
                case 4:
                    Console.WriteLine("\nBye bye...");
                    return;

                default:
                    Console.WriteLine("\nChoise correct option!\n");
                    break;
            }
            Console.WriteLine();
        }
    }

    static async Task WorkWithStudent()
    {
        Console.Write("\nWork with student\n1.Add student\n2.Get all students\n3.Get all students from group\n4.Delete student by id\n5.Update student date\n6.Return\nChoise option: ");
        if(!int.TryParse(Console.ReadLine(),out int studentChoise))
        {
            Console.WriteLine("\nChoise correct option!\n");
            return;
        }
        switch (studentChoise)
        {
            case 1:
                var tempStudent = new Student();
                Console.Write("\nEnter student firts name: ");
                tempStudent.FirstName = Console.ReadLine();
                tempStudent.FirstName = CapitalizeFirstLetter(tempStudent.FirstName);

                Console.Write("Enter student last name: ");
                tempStudent.LastName = Console.ReadLine();
                tempStudent.LastName = CapitalizeFirstLetter(tempStudent.LastName);

                Console.Write("Enter student birthday date: ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime bdate))
                {
                    Console.WriteLine("\nEnter correct date!");
                    break;
                }
                tempStudent.BDate = bdate;

                Console.Write("Enter group name: ");
                var groupName = Console.ReadLine();

                if (!string.IsNullOrEmpty(groupName))
                {
                    await studentRepository.AddStudentAsync(tempStudent, groupName.Trim());
                }
                else
                {
                    Console.WriteLine("\nBad group name!");
                }
                break;
            case 2:
                var allStudents = await studentRepository.GetAllStudentAsync();
                if(allStudents.Count > 0)
                {
                    Console.WriteLine("\nAll students:");
                    foreach (var item in allStudents)
                    {
                        Console.WriteLine($"{item.Id} {item.FirstName} {item.LastName} {item.BDate} {item.Group.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("\nStudent not found!");
                }
               
                break;
            case 3:
                Console.Write("\nEnter group name: ");
                var allStudentsForGroup = await studentRepository.GetAllStudentForGroupAsync(Console.ReadLine());
                if (allStudentsForGroup.Count > 0)
                {
                    Console.WriteLine($"\nAll students from {allStudentsForGroup[0].Group.Name}:");
                    foreach (var item in allStudentsForGroup)
                    {
                        Console.WriteLine($"{item.Id} {item.FirstName} {item.LastName} {item.BDate}");
                    }
                }
                else
                {
                    Console.WriteLine("\nGroups not found!");
                }
                break;
            case 4:
                Console.Write("\nEnter student id: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("\nEnter valid id!");
                    break;
                }
                await studentRepository.DeleteStudentByIdAsync(id);
                break;
            case 5:
                Console.Write("\nEnter user id: ");
                if (!int.TryParse(Console.ReadLine(), out int oldId))
                {
                    Console.WriteLine("\nEnter valid id!");
                    break;
                }
            
                var updatedStudent = new Student();
                Console.Write("Enter new student firts name: ");
                updatedStudent.FirstName = Console.ReadLine();
                updatedStudent.FirstName = CapitalizeFirstLetter(updatedStudent.FirstName);

                Console.Write("Enter new student last name: ");
                updatedStudent.LastName = Console.ReadLine();
                updatedStudent.LastName = CapitalizeFirstLetter(updatedStudent.LastName);

                Console.Write("Enter student birthday date: ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newBdate))
                {
                    Console.WriteLine("\nEnter correct date!");
                    break;
                }
                updatedStudent.BDate = newBdate;

                Console.Write("Enter new group name: ");
                var newGroupName = Console.ReadLine();

                if (!string.IsNullOrEmpty(newGroupName))
                {
                    await studentRepository.UpdateStudentAsync(oldId, updatedStudent, newGroupName.Trim());
                }
                else
                {
                    Console.WriteLine("\nBad group name!");
                }

                break;
            case 6:
                Console.WriteLine("\nReturning...");
                return;
            default:
                Console.WriteLine("\nChoise correct option!\n");
                break;
        }
    }
    static async Task WorkWithTeacher()
    {
        Console.Write("\nWork with teachers\n1.Add teachers\n2.Get teachers by id\n3.Get all teachers\n4.Add group for teacher\n5.Return\nChoise option: ");
        if (!int.TryParse(Console.ReadLine(), out int teacherChoise))
        {
            Console.WriteLine("\nChoise correct option!\n");
            return;
        }
        switch (teacherChoise)
        {
            case 1:
                var tempTeacher = new Teacher();
                Console.Write("\nEnter teacher firts name: ");
                tempTeacher.FirstName = Console.ReadLine();
                tempTeacher.FirstName = CapitalizeFirstLetter(tempTeacher.FirstName);

                Console.Write("Enter teacher last name: ");
                tempTeacher.LastName = Console.ReadLine();
                tempTeacher.LastName = CapitalizeFirstLetter(tempTeacher.LastName);

                Console.Write("Enter teacher birthday date: ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime bdate))
                {
                    Console.WriteLine("\nEnter correct date!");
                    break;
                }
                tempTeacher.DateOfStart = bdate;

                Console.Write("Enter salary: ");
                tempTeacher.Salary = decimal.Parse(Console.ReadLine());

                Console.Write("Enter teacher subject: ");
                tempTeacher.Subject = Console.ReadLine();
                tempTeacher.Subject = CapitalizeFirstLetter(tempTeacher.Subject);

                await teacherRepository.AddTeacherAsync(tempTeacher);
                break;
            case 2:
                Console.Write("\nEnter teachers id: ");
                if(!int.TryParse(Console.ReadLine(),out int teacherId))
                {
                    Console.WriteLine("\nEnter valid id!");
                    break;
                }
                var tempTeachers2 = await teacherRepository.GetTeacherByIdAsync(teacherId);
                if(tempTeachers2 != null)
                {
                    Console.WriteLine($"\n{tempTeachers2.Id} {tempTeachers2.FirstName} {tempTeachers2.LastName} {tempTeachers2.Salary} {tempTeachers2.DateOfStart} {tempTeachers2.Subject} {tempTeachers2.CalendarPlanningPath}\n{tempTeachers2.CalendarPlanningDate}");
                    break;
                }
                Console.WriteLine("\nTeacher not found!");
                break;
            case 3:
                var allTeachers = await teacherRepository.GetAllTeacherAsync();
                if (allTeachers.Count > 0)
                {
                    Console.WriteLine("\nAll teachers:");
                    foreach (var item in allTeachers)
                    {
                        Console.WriteLine($"{item.FirstName} {item.LastName}");
                    }
                    break;
                }
                Console.WriteLine("\nTeachers not found!");
                
                break;
            case 4:
                Console.Write("\nEnter group id: ");
                if (!int.TryParse(Console.ReadLine(), out int groupId))
                {
                    Console.WriteLine("\nEnter valid id!");
                    break;
                }
                Console.Write("Enter teacher id: ");
                if (!int.TryParse(Console.ReadLine(), out int teacherTempId))
                {
                    Console.WriteLine("\nEnter valid id!");
                    break;
                }
                await teacherRepository.AddGroupForTeacherAsync(teacherTempId, groupId);
                break;
            case 5:
                Console.WriteLine("\nReturning...");
                return;
            default:
                Console.WriteLine("\nChoise correct option!\n");
                break;
        }
    }
    static async Task WorkWithGroup()
    {
        Console.Write("\nWork with group\n1.Add group\n2.Add student to group\n3.Get all group with student\n4.Transfer student\n5.Return\nChoise option: ");
        if (!int.TryParse(Console.ReadLine(), out int groupChoise))
        {
            Console.WriteLine("\nChoise correct option!\n");
            return;
        }
        switch (groupChoise)
        {
            case 1:
                var tempGroup = new MyGroup();

                Console.Write("\nEnter group name: ");
                tempGroup.Name = Console.ReadLine();

                Console.Write("Enter course: ");
                if (!int.TryParse(Console.ReadLine(), out int course))
                {
                    Console.WriteLine("\nEnter valid course!");
                    break;
                }
                tempGroup.Course = course;

                Console.Write("Enter teacher id: ");
                if (!int.TryParse(Console.ReadLine(), out int teacherId))
                {
                    Console.WriteLine("\nEnter valid teacher id!");
                    break;
                }
                tempGroup.TeacherId = teacherId;

                await groupRepository.AddGroupAsync(tempGroup);

                break;
            case 2:
                Console.Write("\nEnter student id: ");
                if (!int.TryParse(Console.ReadLine(), out int studentId))
                {
                    Console.WriteLine("\nEnter valid student id!");
                    break;
                }
                Console.Write("\nEnter group id: ");
                if (!int.TryParse(Console.ReadLine(), out int groupId))
                {
                    Console.WriteLine("\nEnter valid group id!");
                    break;
                }
                await groupRepository.AddStudentToGroupAsync(groupId, studentId);

                break;
            case 3:
                var allGroups = await groupRepository.GetAllGroupsAsync();
                if (allGroups.Count > 0)
                {
                    foreach (var group in allGroups)
                    {
                        Console.WriteLine($"\nGroup {group.GroupName}:");

                        if (group.StudentNames.Any())
                        {
                            Console.WriteLine("Students:");
                            foreach (var studentName in group.StudentNames)
                            {
                                Console.WriteLine($"{studentName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No students in this group.");
                        }

                    }
                    break;
                }
                Console.WriteLine("\nGroups not found!");
                break;
            case 4:
                //У моєму виконанні 2 і 4 кейси працюють однаково,але інший метод для трансферу я написав (TransferStudentAsync) 
                Console.Write("\nEnter student id: ");
                if (!int.TryParse(Console.ReadLine(), out int studentId2))
                {
                    Console.WriteLine("\nEnter valid student id!");
                    break;
                }
                Console.Write("Enter group id: ");
                if (!int.TryParse(Console.ReadLine(), out int groupId2))
                {
                    Console.WriteLine("\nEnter valid group id!");
                    break;
                }
                await groupRepository.AddStudentToGroupAsync(groupId2, studentId2);
                break;
            case 5:
                Console.WriteLine("\nReturning...");
                return;
            default:
                Console.WriteLine("\nChoise correct option!\n");
                break;
        }
    }
    public static string CapitalizeFirstLetter(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
    }
}
