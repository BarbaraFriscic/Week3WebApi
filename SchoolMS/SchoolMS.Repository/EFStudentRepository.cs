using SchoolMS.Common;
using SchoolMS.DAL;
using SchoolMS.Model;
using SchoolMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public async Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter studentFilter)
        {
            try
            {
                IQueryable<Student> query = Context.Student.Include(s => s.School);

                if(studentFilter != null)
                {   if(studentFilter.SchoolId != Guid.Empty)
                    {
                        query = query.Where(s => s.SchoolId == studentFilter.SchoolId);
                    }
                    if (studentFilter.Name != null)
                    {
                        query = query.Where(s => s.FirstName.
                                Contains(studentFilter.Name)||s.LastName.
                                Contains(studentFilter.Name));
                    }
                    if (studentFilter.AverageFrom != null)
                    {
                        query = query.Where(s =>s.Average >=  studentFilter.AverageFrom);
                    }
                    if (studentFilter.AverageTo != null)
                    {
                        query = query.Where(s => s.Average <= studentFilter.AverageTo);

                    }
                    if (studentFilter.Average != null)
                    {
                        query = query.Where(s => s.Average == studentFilter.Average);
                    }
                    if (studentFilter.DOBFrom != null)
                    {
                        if (studentFilter.DOBTo != null)
                        {
                            query = query.Where(s => s.DOB >= studentFilter.DOBFrom && s.DOB <= studentFilter.DOBTo);
                        }
                        query = query.Where(s => s.DOB >= studentFilter.DOBFrom);
                    }
                    if (studentFilter.DOBTo != null)
                    {
                        query = query.Where(s => s.DOB <= studentFilter.DOBTo);
                    }
                }  
                if (sorting != null)
                {              
                    
                }
                List<StudentModelDTO> studentDtos = await query.Select(s => new StudentModelDTO()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Average = s.Average,
                    Address = s.Address,
                    SchoolId = s.SchoolId,
                    DOB = s.DOB,
                    SchoolName = s.School.Name
                }).ToListAsync();

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
