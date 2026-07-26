using SchoolManagerApp;
using System.Runtime.Intrinsics.Arm;

class Program
{
    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Khai báo kiểu Interface, khởi tạo kiểu Class (Đúng chuẩn OOP)
        IStudentService service = new StudentService();

        // Thêm data mẫu
        service.AddStudent(new Student(1, "Nam", 8.5));
        service.AddStudent(new Student(2, "Lan", 9.0));
        service.AddStudent(new Student(3, "Bình", 6.5));
        service.AddStudent(new Student(4, "Hoa", 7.0));
        service.AddStudent(new Student(5, "Tuấn", 9.5));

        // 1. Thử chức năng lấy Top 3
        Console.WriteLine("=== TOP 3 SINH VIÊN ===");
        List<Student> top3 = service.GetTopStudents(3);
        // ??? Dùng foreach để in từng bạn trong top3 ra màn hình
        foreach (var student in top3)
        {
            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, GPA: {student.GPA}");
        }

        // 2. Thử chức năng lấy điểm trung bình
        Console.WriteLine("\n=== ĐIỂM TRUNG BÌNH ===");
        Console.WriteLine($"\nĐiểm trung bình cả lớp: {service.GetAverageGPA():F2}");


        // 3. Thử bắt lỗi
        Console.WriteLine("\n=== THỬ BẮT LỖI ===");
        try
        {
            Student s = service.GetStudent(999); // ID không tồn tại
            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, GPA: {s.GPA}");

        }
        catch (StudentNotFoundException ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }

    }
}