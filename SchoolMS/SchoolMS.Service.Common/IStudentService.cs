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
        Task<List<StudentModel>> GetAllStudents();
        Task<StudentModel> GetStudent(Guid id);
        Task<bool> AddNewStudent(StudentModel student);
        Task<bool> EditStudent(Guid id, StudentModel student);
        Task<bool> DeleteStudent(Guid id);
    }
}
