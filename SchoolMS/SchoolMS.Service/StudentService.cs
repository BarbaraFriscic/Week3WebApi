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
        IStudentRepository _studentRepository = new StudentRepository();

        public List<StudentModel> GetAllStudents()
        {
            List<StudentModel> students = _studentRepository.GetAllStudents();

            return students;
        }

        public StudentModel GetStudent(Guid id)
        {
            StudentModel studentModel= _studentRepository.GetStudent(id);

            return studentModel;
        }

        public bool AddNewStudent(StudentModel student)
        {
            bool isSuccess = _studentRepository.AddNewStudent(student);
            return isSuccess;
        }

        public bool EditStudent(Guid id, StudentModel student)
        {
            StudentModel studentCheck = _studentRepository.GetStudent(id);
            if(studentCheck == null)
            {
                return false;
            }
            StudentModel studentToEdit = new StudentModel
            {
                FirstName = student.FirstName == default ? studentCheck.FirstName : student.FirstName,
                LastName = student.LastName == default ? studentCheck.LastName : student.LastName,
                DOB = student.DOB == default ? studentCheck.DOB : student.DOB,
                Address = student.Address == default ? studentCheck.Address : student.Address,
                SchoolId = student.SchoolId == default ? studentCheck.SchoolId : student.SchoolId,
                Average = student.Average == default ? studentCheck.Average == null ? default : (decimal)studentCheck.Average : (decimal)student.Average
            };

            bool isSuccess = _studentRepository.EditStudent(id, studentToEdit);
            return isSuccess;
        }

        public bool DeleteStudent(Guid id)
        {
            bool isSuccess = _studentRepository.DeleteStudent(id);
            return isSuccess;
        }
    }
}
