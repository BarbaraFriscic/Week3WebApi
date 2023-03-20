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
        public static List<School> Schools = new List<School>
        {
            new School { Id = 1, Name = "OŠ Retfala" },
            new School { Id = 2, Name = "OŠ Višnjevac" }
        };
        public static List<Student> Students = new List<Student>
        {
            new Student
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Mihic",
            Address = new Address { City = "Zagreb" },
            DOB = DateTime.Now.Date,
            SchoolId = 2
        },
            new Student
            {
            Id = 2,
            FirstName = "Tanja",
            LastName = "Saric",
            Address = new Address { City = "Osijek", Street = "Gunduliceva 9" },
            DOB = DateTime.Now.Date,
            SchoolId = 1
            },
            new Student
            {
            Id = 3,
            FirstName = "Iva",
            LastName = "Bulic",
            Address = new Address { City = "Osijek", PostalCode = "31000" },
            DOB = DateTime.Now.Date,
            SchoolId = 2
            }
    };
        public bool IsSuccess { get; set; }
       
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
            Student studentToDelete = Students.Find(s => s.Id == id);
            try
            {               
                Students.Remove(studentToDelete);
                IsSuccess = true;
            }
            catch (Exception)
            {
                IsSuccess = false;
            }          

            
            if(IsSuccess)
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

        [HttpPost]
        public List<Student> CreateStudent([FromBody] Student student)
        {
            try
            {               
                Students.Add(student);
                IsSuccess = true;
            }
            catch (Exception)
            {
               IsSuccess = false;
            }
                return Students;
        }

        [HttpPut]
        public List<Student> UpdateStudent(int id)
        {
            try
            {
                Student studentToUpdate = Students.Find(s => s.Id == id);
                studentToUpdate.FirstName = "novoIme";
                studentToUpdate.LastName = "novoPrezime";
                studentToUpdate.DOB = DateTime.Today.AddDays(1);
                IsSuccess = true;
            }
            catch (Exception)
            {
                IsSuccess = false;
            }
            return Students;
        }
    }
}
