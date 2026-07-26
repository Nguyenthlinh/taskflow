using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApp
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double GPA { get; set; }
        public Student(int id, string name, int age, double gpa)
        {
            Id = id;
            Name = name;
            Age = age;
            GPA = gpa;
        }
       //Method xếp hạng sinh viên dựa trên GPA
        public string GetRank()
        {
            if (GPA >= 3.6)
                return "gioi";
            else if (GPA >= 3.0)
                return "kha";
            else if (GPA >= 2.0)
                return "trung binh ";
            else
                return "yeu";
        }
        //method hiển thị thông tin sinh viên
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Age: {Age}, GPA: {GPA}, Rank: {GetRank()}");
        }
    }
}
