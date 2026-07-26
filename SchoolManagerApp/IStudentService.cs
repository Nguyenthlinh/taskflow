using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagerApp
{
     interface IStudentService
    {
        void AddStudent(Student s);
        Student GetStudent(int id);
        List<Student> GetTopStudents(int count);
        double GetAverageGPA();
    }
}
