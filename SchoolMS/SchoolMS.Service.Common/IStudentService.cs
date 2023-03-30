using SchoolMS.Common;
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
        Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter filtering);
        Task<StudentModelDTO> GetStudent(Guid id);
        Task<bool> AddNewStudent(StudentModelDTO student);
        Task<bool> EditStudent(Guid id, StudentModelDTO student);
        Task<bool> DeleteStudent(Guid id);
    }
}
