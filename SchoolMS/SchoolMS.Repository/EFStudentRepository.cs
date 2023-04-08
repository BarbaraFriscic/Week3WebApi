using PagedList;
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
using AutoMapper;


namespace SchoolMS.Repository
{
    public class EFStudentRepository : IStudentRepository
    {
        public SchoolMSContext Context { get; set; }
        public IMapper _mapper;
        public EFStudentRepository(SchoolMSContext context, IMapper mapper)
        {
            Context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNewStudent(StudentModelDTO student)
        {
            try
            {
                if(student != null)
                {
                    Student studentDal = _mapper.Map<Student>(student);
                    //Student studentDal = new Student
                    //{
                    //    Id = Guid.NewGuid(),
                    //    FirstName = student.FirstName,
                    //    LastName = student.LastName,
                    //    SchoolId = student.SchoolId,
                    //    Address = student.Address,
                    //    DOB = student.DOB,
                    //    Average = student.Average
                    //};
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
                    studentDal = _mapper.Map<Student>(studentDTO);
                    //studentDal.FirstName = student.FirstName;
                    //studentDal.LastName = student.LastName;
                    //studentDal.Address = student.Address;
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

        public async Task<IPagedList<StudentModelDTO>> GetAllStudents(string sortBy, string search, int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<Student> query = Context.Student.Include(s => s.School).Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || search == null).AsQueryable();

                switch (sortBy)
                {
                    case null:
                        query = query.OrderByDescending(s => s.LastName);
                        break;
                    case "FirstName desc":
                        query = query.OrderByDescending(s => s.FirstName);
                        break;
                    case "FirstName":
                        query = query.OrderBy(s => s.FirstName);
                        break;
                    case "LastName desc":
                        query = query.OrderByDescending(s => s.LastName);
                        break;
                    case "LastName":
                        query = query.OrderBy(s => s.LastName);
                        break;
                    case "SchoolName desc":
                        query = query.OrderByDescending(s => s.School.Name);
                        break;
                    case "SchoolName":
                        query = query.OrderBy(s => s.School.Name);
                        break;
                }

                var result=query.ToPagedList(pageNumber, pageSize);

                List<StudentModelDTO> studentDtos = result.Select(s => _mapper.Map<StudentModelDTO>(s)).ToList();

                //List<StudentModelDTO> studentDtos = result.Select(s => new StudentModelDTO()
                //{
                //    Id = s.Id,
                //    FirstName = s.FirstName,
                //    LastName = s.LastName,
                //    Average = s.Average,
                //    Address = s.Address,
                //    SchoolId = s.SchoolId,
                //    DOB = s.DOB,
                //    SchoolName = s.School.Name
                //}).ToList();

                var pagedList = new StaticPagedList<StudentModelDTO>(studentDtos,pageNumber, pageSize, result.TotalItemCount);

                return await Task.FromResult(pagedList);                     
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
                //StudentModelDTO studentDTO = new StudentModelDTO();
                student = await Context.Student.Where(s => s.Id == id).FirstOrDefaultAsync();
                
                if(student != null)
                {
                    StudentModelDTO studentDTO = _mapper.Map<StudentModelDTO>(student);
                    //studentDTO.FirstName = student.FirstName;
                    //studentDTO.LastName = student.LastName;
                    //studentDTO.Address = student.Address;
                    //studentDTO.Average = student.Average;
                    //studentDTO.DOB = student.DOB;
                    //studentDTO.SchoolId = student.SchoolId;
                    //studentDTO.SchoolName = student.School.Name;
                    //studentDTO.Id = id;

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
