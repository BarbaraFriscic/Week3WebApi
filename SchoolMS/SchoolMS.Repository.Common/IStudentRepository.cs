using SchoolMS.Model;
using SchoolMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace SchoolMS.Repository.Common
{
    public interface IStudentRepository
    {
        Task<IPagedList<StudentModelDTO>> GetAllStudents(string sortBy,string search, int pageNumber, int pageSize);
        Task<StudentModelDTO> GetStudent(Guid id);
        Task<bool> AddNewStudent(StudentModelDTO student);
        Task<bool> EditStudent(Guid id,  StudentModelDTO student);
        Task<bool> DeleteStudent(Guid id);
    }
}
