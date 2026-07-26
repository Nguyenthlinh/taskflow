using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagerApp
{
     class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(int id) : base($"Không tìm thấy sinh viên với ID = {id}")
        {
        }
        
    }
}
