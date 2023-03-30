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
        Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter filtering);
        Task<StudentModelDTO> GetStudent(Guid id);
        Task<bool> AddNewStudent(StudentModelDTO student);
        Task<bool> EditStudent(Guid id,  StudentModelDTO student);
        Task<bool> DeleteStudent(Guid id);
    }
}
