using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApp
{
    public class Classroom
    {
        public string ClassName { get; set; }
        private List<Student> _students = new ();//_ trước tên = quy ước ngầm: đây là private field của class.
        public Classroom(string className)
        {
            ClassName = className;
        }
        //them sinh vien vao lop
        public void AddStudent(Student student)
        {
            _students.Add(student);
            Console.WriteLine($"Added student: {student.Name} to class: {ClassName}");
        }
        //lấy top N sinh viên theo GPA
                public List<Student> GetTopStudents(int n)
        {
            return _students.OrderByDescending(sv => sv.GPA)  // sort GPA cao → thấp
                            .Take(n)                           // lấy n phần tử đầu
                            .ToList();                         // chuyển thành List
        }
        //tìm sinh viên theo tên
        public Student? FindByName(string name) // ← dấu ? = có thể trả về null
        {
            return _students.FirstOrDefault(sv => 
            sv.Name.ToLower().Contains(name.ToLower()));
            // ToLower() → tìm kiếm không phân biệt HOA/thường
            // Contains() → tìm tên chứa keyword, không cần nhập chính xác
            // FirstOrDefault() → trả null nếu không tìm thấy (không crash)
        }
        //thông tin lớp học
        public void PrintAll()
        {
            Console.WriteLine($"Class: {ClassName}");
            foreach (var student in _students)
            {
                student.DisplayInfo();
            }
        }

    }
}
