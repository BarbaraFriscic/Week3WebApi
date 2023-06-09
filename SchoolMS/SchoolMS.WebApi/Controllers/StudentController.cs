﻿using Microsoft.Ajax.Utilities;
using SchoolMS.Common;
using SchoolMS.Model;
using SchoolMS.Service;
using SchoolMS.Service.Common;
using SchoolMS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolMS.WebApi.Controllers
{
    public class StudentController : ApiController
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolMS;Integrated Security=True";
        protected IStudentService StudentService { get; set; }

        public StudentController(IStudentService studentService)
        {
            StudentService = studentService;
        }

        [HttpGet]
        [Route("api/student/get-all")]
        public async Task<HttpResponseMessage> GetAllStudents(
            DateTime? dobTo = null, DateTime? dobFrom = null,decimal? average = null, 
            decimal ? averageTo = null, decimal? averageFrom = null, string name = null, 
            Nullable<Guid> schoolId = null, int pageNumber =1, int pageSize = 5, string orderByColumn = "Id", 
            string sortOrder = "asc")
        {
            Paging paging = new Paging
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            Sorting sorting = new Sorting
            {
                SortOrder = sortOrder,
                OrderBy = orderByColumn
            };
            StudentFilter filtering = new StudentFilter
            {
                Name = name == null ? null : name,
                SchoolId = (Guid)(schoolId == null ? Guid.Empty : schoolId),
                AverageFrom = averageFrom == null? (decimal?)null : averageFrom,
                AverageTo = averageTo == null ? (decimal?)null : averageTo,
                Average = average == null? (decimal?)null : average,
                DOBFrom = dobFrom == null? (DateTime?)null : dobFrom,
                DOBTo = dobTo == null? (DateTime?)null : dobTo,
            };

            List<StudentModelDTO> students = await StudentService.GetAllStudents(paging, sorting, filtering);
            List<StudentRest> mappedStudents = new List<StudentRest>();
            if (students == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No records of Students found.");
            }
            foreach (StudentModelDTO student in students)
            {
                StudentRest mappedStudent = new StudentRest();
                mappedStudent.FirstName = student.FirstName;
                mappedStudent.LastName = student.LastName;
                mappedStudent.SchoolName = student.SchoolName;

                mappedStudents.Add(mappedStudent);
            }
            return Request.CreateResponse(HttpStatusCode.OK, mappedStudents);
        }

        [HttpGet]
        //[Route("api/student/get-by-id/{id}")]
        public async Task<HttpResponseMessage> GetStudent(Guid id)
        {
            StudentModelDTO student = await StudentService.GetStudent(id);
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to find that Student.");
            }
            StudentRest studentRest = new StudentRest();
            studentRest.FirstName = student.FirstName;
            studentRest.LastName = student.LastName;
            return Request.CreateResponse(HttpStatusCode.OK, studentRest);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddNewStudent(StudentPostRest studentRest)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            StudentModelDTO studentModel = new StudentModelDTO();
            studentModel.FirstName = studentRest.FirstName;
            studentModel.LastName = studentRest.LastName;
            studentModel.Address = studentRest.Address;
            studentModel.DOB = studentRest.DOB;
            studentModel.SchoolId = studentRest.SchoolId;
            studentModel.Average = studentRest.Average;

            bool isAdded = await StudentService.AddNewStudent(studentModel);
            if (!isAdded)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to add new Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Added new Student successfully.");
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteStudent(Guid id)
        {
            bool isDeleted = await StudentService.DeleteStudent(id);
            if (!isDeleted)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to delete Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
        }


        [HttpPut]
        public async Task<HttpResponseMessage> UpdateStudent(Guid id, StudentPutRest studentRest)
        {
            StudentModelDTO studentModel = new StudentModelDTO();
            studentModel.FirstName = studentRest.FirstName;
            studentModel.LastName = studentRest.LastName;
            studentModel.Address = studentRest.Address;
            
            bool isEdited = await StudentService.EditStudent(id, studentModel);
            if (!isEdited)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to update Student.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Student updated successfully.");
        }

        public class StudentPostRest
        {
            [Required(ErrorMessage = "First name is required")]
            [MinLength(2, ErrorMessage = "Minimal length is 2 characters")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [MinLength(2, ErrorMessage = "Minimal length is 2 characters")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Date of birth is required")]
            public DateTime DOB { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [MinLength(2, ErrorMessage = "Minimal length is 2 characters")]
            public string Address { get; set; }

            [Required(ErrorMessage = "School Id is required")]
            public Guid SchoolId { get; set; }

            public decimal? Average { get; set; } = null;
        }

        public class StudentPutRest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
        }
    }
}
