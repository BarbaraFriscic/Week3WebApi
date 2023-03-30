using SchoolMS.Model;
using SchoolMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository.Common
{
    public interface IStudentRepository
    {
        Task<List<StudentModel>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter filtering);
        Task<StudentModel> GetStudent(Guid id);
        Task<bool> AddNewStudent(StudentModel student);
        Task<bool> EditStudent(Guid id,  StudentModel student);
        Task<bool> DeleteStudent(Guid id);
    }
}
