using ClassroomApp;
using System.Runtime.Intrinsics.Arm;    
    

public class Program
{
    public static void Main(string[] args)
    {
        Classroom lop = new Classroom("SE1701");
        lop.AddStudent(new Student(1, "Nguyen Van Nam", 20, 3.7));
        lop.AddStudent(new Student(2, "Tran Thi Lan", 21, 2.8));
        lop.AddStudent(new Student(3, "Le Van Tuan", 22, 3.2));
        lop.AddStudent(new Student(4, "Pham Thi Mai", 20, 3.9));
        lop.AddStudent(new Student(5, "Hoang Van An", 23, 1.8));

        lop.PrintAll();
        Console.WriteLine("\nTop 3 students:");
        var topStudents = lop.GetTopStudents(3);
        foreach (var student in topStudents)
        {
            student.DisplayInfo();
        }
        Console.WriteLine("\nFind student by name 'Le Van Tuan':");
        var foundStudent = lop.FindByName("Le Van Tuan");
        if (foundStudent != null)
        {
            foundStudent.DisplayInfo();
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

    }
}