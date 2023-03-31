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
            try
            {
                if(student != null)
                {
                    Student studentDal = new Student
                    {
                        Id = Guid.NewGuid(),
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        SchoolId = student.SchoolId,
                        Address = student.Address,
                        DOB = student.DOB,
                        Average = student.Average
                    };
                    Context.Student.Add(studentDal);
                    await Context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }         
        }

        public async Task<bool> DeleteStudent(Guid id)
        {
            try
            {
                StudentModelDTO studentDTO = await GetStudent(id);
                if(studentDTO != null)
                {
                    Student student = await Context.Student.Where(s => s.Id == id).FirstOrDefaultAsync();
                    Context.Student.Remove(student);
                    await Context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditStudent(Guid id, StudentModelDTO student)
        {
            try
            {
                StudentModelDTO studentDTO = await GetStudent(id);
                if (studentDTO != null)
                {
                    Student studentDal = await Context.Student.Where(s => s.Id == student.Id).FirstOrDefaultAsync();                  
                    studentDal.FirstName = student.FirstName;
                    studentDal.LastName = student.LastName;
                    studentDal.Address = student.Address;
                    await Context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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
                

                studentDtos = await Context.Student.Include(s => s.School.Name).Select(s => new StudentModelDTO()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Address = s.Address,
                        Average = s.Average,
                        DOB = s.DOB,
                        SchoolId = s.SchoolId,
                        SchoolName = s.School.Name
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
        
        public async Task<StudentModelDTO> GetStudent(Guid id)
        {
            try
            {
                Student student = new Student();
                StudentModelDTO studentDTO = new StudentModelDTO();
                student = await Context.Student.Where(s => s.Id == id).FirstOrDefaultAsync();
                
                if(student != null)
                {
                    studentDTO.FirstName = student.FirstName;
                    studentDTO.LastName = student.LastName;
                    studentDTO.Address = student.Address;
                    studentDTO.Average = student.Average;
                    studentDTO.DOB = student.DOB;
                    studentDTO.SchoolId = student.SchoolId;
                    studentDTO.SchoolName = student.School.Name;
                    studentDTO.Id = id;

                    return studentDTO;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            


        }
    }
}
