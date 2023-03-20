using SchoolMS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolMS.WebApi.Controllers
{
    public class StudentController : ApiController
    {
        public List<School> Schools = new List<School>();
        public List<Student> Students = new List<Student>();
        public StudentController()
        {
          School firstSchool = new School { Id = 1, Name = "OŠ Retfala" };
          School secondSchool = new School { Id = 2, Name = "OŠ Višnjevac" };

        Student firstStudent = new Student
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Mihic",
            Address = new Address { City = "Zagreb" },
            DOB = DateTime.Now.Date,
            SchoolId = 2
        };
        Student secondStudent = new Student
        {
            Id = 2,
            FirstName = "Tanja",
            LastName = "Saric",
            Address = new Address { City = "Osijek", Street = "Gunduliceva 9" },
            DOB = DateTime.Now.Date,
            SchoolId = 1
        };
        Student thirdStudent = new Student
        {
            Id = 3,
            FirstName = "Iva",
            LastName = "Bulic",
            Address = new Address { City = "Osijek", PostalCode = "31000" },
            DOB = DateTime.Now.Date,
            SchoolId = 2
        };

            Schools.Add(firstSchool);
            Schools.Add(secondSchool);
            Students.Add(firstStudent);
            Students.Add(secondStudent);
            Students.Add(thirdStudent);
        }

        [HttpGet]
        public List<Student> GetStudents()
        {
            
            return Students;
        }
        
        [HttpGet]
        public Student GetStudent(int id)
        {
            
            Student studentToReturn = Students.Find(s => s.Id == id);
            return studentToReturn;
        }
        [HttpDelete]
        public string DeleteStudent(int id)
        {
            string returnValue= "";
            bool isSuccess;
            Student studentToDelete = Students.Find(s => s.Id == id);
            try
            {               
                Students.Remove(studentToDelete);
                isSuccess = true;
            }
            catch (Exception)
            {
              isSuccess = false;
            }
            if(isSuccess)
            {
                
                foreach (Student student in Students) 
                {
                    returnValue += student.ToString() + "\n";
                }
                return "Success" + $"\n{returnValue}";
            }
            else
            {
                return "Unable to delete student";
            }
            
        }


    }
}
