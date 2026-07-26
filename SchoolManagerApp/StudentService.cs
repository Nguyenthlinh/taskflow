using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagerApp
{
    class StudentService : IStudentService
    {
        private List<Student> _students = new ();
        public void AddStudent(Student s)
        {
            _students.Add(s);
        }

        public double GetAverageGPA()
        {   
            // 1. Phải check Count == 0 TRƯỚC khi tính Average để tránh lỗi
            if (_students.Count == 0) return 0;

            var tb = _students.Average(s => s.GPA);
            return tb;
        }

        public Student GetStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            
            // 2. Phải ném ra lỗi tự định nghĩa (StudentNotFoundException)
            if (student == null) throw new StudentNotFoundException(id);
            
            return student;
        }

        public List<Student> GetTopStudents(int count)
        {
            var topStudents = _students.OrderByDescending(s => s.GPA).Take(count).ToList();
            return topStudents;
        }
    }
}
