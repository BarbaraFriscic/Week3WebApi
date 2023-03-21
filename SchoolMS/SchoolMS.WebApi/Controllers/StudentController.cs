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

        [HttpGet]
        public HttpResponseMessage GetStudents()
        {
            try
            {
                List<Student> studentsToReturn = Students.ToList();
                if (studentsToReturn != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, studentsToReturn);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Students found");
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing GetStudents");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStudent(int id)
        {
            try
            {
                // Student studentToReturn = Students.Find(s => s.Id == id);
                Student studentToReturn = Students.Where(s => s.Id == id).FirstOrDefault();
                if (studentToReturn != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, studentToReturn);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No Student with an id: {id}");
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting GetStudent({id})");
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteStudent(int id)
        {
            try
            {
                //Student studentToDelete = Students.Find(s => s.Id == id);
                Student studentToDelete = Students.Where(s => s.Id == id).FirstOrDefault();
                if(studentToDelete != null)
                {
                    Students.Remove(studentToDelete);
                    return Request.CreateResponse(HttpStatusCode.OK, Students);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No Student with an Id:{id}. Unable to Delete.");
                }

            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting DeleteStudent({id})\"");
            }

        }

        [HttpPost]
        public List<Student> CreateStudent([FromBody] Student student)
        {
            try
            {
                Students.Add(student);
            }
            catch (Exception)
            {
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
            }
            catch (Exception)
            {
            }
            return Students;
        }
    }
}
