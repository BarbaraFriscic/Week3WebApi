using SchoolMS.Common;
using SchoolMS.DAL;
using SchoolMS.Model;
using SchoolMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository
{
    public class EFStudentRepository : IStudentRepository
    {
        public SchoolMSContext Context { get; set; }
        public EFStudentRepository(SchoolMSContext context)
        {
            Context = context;
        }

        public async Task<bool> AddNewStudent(StudentModelDTO student)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteStudent(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditStudent(Guid id, StudentModelDTO student)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter filtering)
        {
            try
            {
                List<Student> students = new List<Student>();
                List<StudentModelDTO> studentDtos = new List<StudentModelDTO>();
                //if (filtering != null)
                //{
                //    if(filtering.SchoolId != Guid.Empty)
                //    {
                //        students = await Context.Student.
                //            Where(s => s.Id == filtering.SchoolId).
                //            ToListAsync();
                //    }
                //}
                //if (paging != null)
                //{

                //}
                //if(sorting != null)
                //{

                //}
                

                studentDtos = await Context.Student.Select(s => new StudentModelDTO()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Address = s.Address,
                        Average = s.Average,
                        DOB = s.DOB,
                        SchoolId = s.SchoolId
                    }).ToListAsync();
             
                if(studentDtos.Count == 0)
                {
                    return null;
                }
                return studentDtos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<StudentModelDTO> GetStudent(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
