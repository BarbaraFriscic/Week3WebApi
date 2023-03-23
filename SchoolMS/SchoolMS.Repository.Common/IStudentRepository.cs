using SchoolMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository.Common
{
    public interface IStudentRepository
    {
        List<StudentModel> GetAllStudents();
        StudentModel GetStudent(Guid id);
        bool AddNewStudent(StudentModel student);
        bool EditStudent(Guid id,  StudentModel student);
        bool DeleteStudent(Guid id);
    }
}
