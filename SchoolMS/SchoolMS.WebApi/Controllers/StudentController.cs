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
        public static List<School> schools = new List<School>
        {
            new School { Id = 1, Name = "OŠ Retfala" },
            new School { Id = 2, Name = "OŠ Višnjevac" }
        };
        public static List<Student> students = new List<Student>
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
                List<Student> studentsToReturn = students.ToList();
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
                Student studentToReturn = students.Where(s => s.Id == id).FirstOrDefault();
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
                Student studentToDelete = students.Where(s => s.Id == id).FirstOrDefault();
                if(studentToDelete != null)
                {
                    students.Remove(studentToDelete);
                    return Request.CreateResponse(HttpStatusCode.OK, students);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No Student with an Id:{id}. Unable to Delete.");
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting DeleteStudent({id})");
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateNewStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to create new student");
                }
                Student studentCheck = students.Find(s => s.Id == student.Id);
                if (studentCheck != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Student with an Id: {student.Id} already exists.");
                }

                Student newStudent = new Student
                {
                      Id = student.Id,
                      FirstName = student.FirstName,
                      LastName = student.LastName,
                      DOB = student.DOB,
                 };
                 students.Add(newStudent);
                 return Request.CreateResponse(HttpStatusCode.OK, newStudent);                               
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting CreateNewStudent(Student student)");
            }
           
        }

        [HttpPut]
        public HttpResponseMessage UpdateStudent(int id, [FromUri] Student student)
        {
            try
            {
                Student studentToUpdate = students.Find(s => s.Id == id);
                if(studentToUpdate != null)
                {
                    studentToUpdate.FirstName = string.IsNullOrWhiteSpace(student.FirstName)?studentToUpdate.FirstName : student.FirstName;
                    studentToUpdate.LastName = string.IsNullOrWhiteSpace(student.LastName)?studentToUpdate.LastName : student.LastName;
                    studentToUpdate.DOB = Convert.ToDateTime(string.IsNullOrWhiteSpace(student.DOB.ToString())?studentToUpdate.DOB : student.DOB);

                    return Request.CreateResponse(HttpStatusCode.OK, students);
                }

                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to find a student with an Id: {id}");
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting UpdateStudent({id},student)");
            }
           
        }
    }
}
