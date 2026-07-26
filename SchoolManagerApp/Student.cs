using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagerApp
{
   public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double GPA { get; set; }

        public Student(int id, string name, double gpa)
        {
            Id = id;
            Name = name;
            GPA = gpa;
        }
    }
}
