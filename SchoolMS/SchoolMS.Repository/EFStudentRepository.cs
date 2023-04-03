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
                IQueryable<Student> query = Context.Student.Include(s => s.School).AsQueryable();

                if(studentFilter != null)
                {
                    query = CheckFilter(studentFilter, query);
                }  
                if (sorting != null)
                {                  
                    query = CheckOrderBy(sorting, query);
                }                
                if(paging != null)
                {
                    query = query.Skip((int)((paging.PageNumber - 1) * paging.PageSize)).Take((int)paging.PageSize);
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

        public IQueryable<Student> CheckFilter(StudentFilter studentFilter, IQueryable<Student> query)
        {
            if (studentFilter.SchoolId != Guid.Empty)
            {
                return query = query.Where(s => s.SchoolId == studentFilter.SchoolId);
            }
            if (studentFilter.Name != null)
            {
                return query = query.Where(s => s.FirstName.
                        Contains(studentFilter.Name) || s.LastName.
                        Contains(studentFilter.Name));
            }
            if (studentFilter.AverageFrom != null)
            {
                return query = query.Where(s => s.Average >= studentFilter.AverageFrom);
            }
            if (studentFilter.AverageTo != null)
            {
                return query = query.Where(s => s.Average <= studentFilter.AverageTo);

            }
            if (studentFilter.Average != null)
            {
                return query = query.Where(s => s.Average == studentFilter.Average);
            }
            if (studentFilter.DOBFrom != null)
            {
                if (studentFilter.DOBTo != null)
                {
                    return query = query.Where(s => s.DOB >= studentFilter.DOBFrom && s.DOB <= studentFilter.DOBTo);
                }
                return query = query.Where(s => s.DOB >= studentFilter.DOBFrom);
            }
            if (studentFilter.DOBTo != null)
            {
                return query = query.Where(s => s.DOB <= studentFilter.DOBTo);
            }
            return query;
        }

        public IQueryable<Student> CheckOrderBy(Sorting sorting, IQueryable<Student> query)
        {
            if (sorting.OrderBy.Equals("Id"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.Id) : query.OrderByDescending(s => s.Id);
            }
            else if (sorting.OrderBy.Equals("FirstName"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
            }
            else if (sorting.OrderBy.Equals("LastName"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName);
            }
            else if (sorting.OrderBy.Equals("Average"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.Average) : query.OrderByDescending(s => s.Average);
            }
            else if (sorting.OrderBy.Equals("DOB"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.DOB) : query.OrderByDescending(s => s.DOB);
            }
            else if (sorting.OrderBy.Equals("SchoolId"))
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.SchoolId) : query.OrderByDescending(s => s.SchoolId);
            }
            else
            {
                return query = sorting.SortOrder == "asc" ? query.OrderBy(s => s.Address) : query.OrderByDescending(s => s.Address);
            }
        }
    }
}
