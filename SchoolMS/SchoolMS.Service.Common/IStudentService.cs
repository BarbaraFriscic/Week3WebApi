using SchoolMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Service.Common
{
    public interface IStudentService
    {
        List<StudentModel> GetAllStudents();
        StudentModel GetStudent(Guid id);
        bool AddNewStudent(StudentModel student);
        bool EditStudent(Guid id, StudentModel student);
        bool DeleteStudent(Guid id);
    }
}
