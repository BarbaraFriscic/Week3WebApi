using Microsoft.Ajax.Utilities;
using SchoolMS.Model;
using SchoolMS.Service;
using SchoolMS.Service.Common;
using SchoolMS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolMS.WebApi.Controllers
{
    public class StudentController : ApiController
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolMS;Integrated Security=True";
        IStudentService _studentService = new StudentService();

        [HttpGet]
        [Route("api/student/get-all")]
        public HttpResponseMessage GetAllStudents()
        {
            List<StudentModel> students = _studentService.GetAllStudents();
            if(students == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No records of Students found.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, students);
            
        }

        [HttpGet]
        //[Route("api/student/get-by-id/{id}")]
        public HttpResponseMessage GetStudent(Guid id)
        {
            StudentModel student = _studentService.GetStudent(id);
            if(student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to find that Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        [HttpPost]
        public HttpResponseMessage AddNewStudent(StudentModel student)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccess = _studentService.AddNewStudent(student);
            if(!isSuccess)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to add new Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Added new Student successfully.");
        }

        [HttpDelete]
        public HttpResponseMessage DeleteStudent(Guid id)
        {
            bool isSuccess = _studentService.DeleteStudent(id);
            if(!isSuccess)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to delete Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
        }
        

        [HttpPut]
        public HttpResponseMessage UpdateStudent(Guid id, StudentModel student)
        {
            bool isSuccess = _studentService.EditStudent(id, student);
            if(!isSuccess)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to update Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Student updated successfully.");
        }
    }
}
