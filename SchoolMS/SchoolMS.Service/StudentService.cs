using SchoolMS.Common;
using SchoolMS.Model;
using SchoolMS.Repository;
using SchoolMS.Repository.Common;
using SchoolMS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Service
{
    public class StudentService : IStudentService
    {
        protected IStudentRepository StudentRepository { get; set; }

        public StudentService(IStudentRepository studentRepository)
        {
            StudentRepository = studentRepository;
        }

        public async Task<List<StudentModelDTO>> GetAllStudents(Paging paging, Sorting sorting, StudentFilter filtering)
        {
            List<StudentModelDTO> students = await StudentRepository.GetAllStudents(paging, sorting, filtering);

            return students;
        }

        public async Task<StudentModelDTO> GetStudent(Guid id)
        {
            StudentModelDTO studentModel= await StudentRepository.GetStudent(id);

            return studentModel;
        }

        public async Task<bool> AddNewStudent(StudentModelDTO student)
        {
            bool isAdded = await StudentRepository.AddNewStudent(student);
            return isAdded;
        }

        public async Task<bool> EditStudent(Guid id, StudentModelDTO student)
        {
            StudentModelDTO studentCheck = await StudentRepository.GetStudent(id);
            if(studentCheck == null)
            {
                return false;
            }
            StudentModelDTO studentToEdit = new StudentModelDTO
            {
                Id = id,
                FirstName = student.FirstName == default ? studentCheck.FirstName : student.FirstName,
                LastName = student.LastName == default ? studentCheck.LastName : student.LastName,
                DOB = student.DOB == default ? studentCheck.DOB : student.DOB,
                Address = student.Address == default ? studentCheck.Address : student.Address,
                SchoolId = student.SchoolId == default ? studentCheck.SchoolId : student.SchoolId,
                Average = student.Average == default ? studentCheck.Average == null ? default : (decimal)studentCheck.Average : (decimal)student.Average
            };

            bool isEdited = await StudentRepository.EditStudent(id, studentToEdit);
            return isEdited;
        }

        public async Task<bool> DeleteStudent(Guid id)
        {
            StudentModelDTO studentCheck = await StudentRepository.GetStudent(id);
            if(studentCheck == null)
            {
                return false;
            }
            bool isDeleted = await StudentRepository.DeleteStudent(id);
            return isDeleted;
        }
    }
}
