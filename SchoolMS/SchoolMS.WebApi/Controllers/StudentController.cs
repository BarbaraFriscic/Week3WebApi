using Microsoft.Ajax.Utilities;
using SchoolMS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolMS.WebApi.Controllers
{
    public class StudentController : ApiController
    {
        //public static List<School> schools = new List<School>
        //{
        //    new School { Id = 1, Name = "OŠ Retfala" },
        //    new School { Id = 2, Name = "OŠ Višnjevac" }
        //};
        //public static List<Student> students = new List<Student>
        //{
        //    new Student
        //{
        //    Id = 1,
        //    FirstName = "Marko",
        //    LastName = "Mihic",
        //    Address = new Address { City = "Zagreb" },
        //    DOB = DateTime.Now.Date,
        //    SchoolId = 2
        //},
        //    new Student
        //    {
        //    Id = 2,
        //    FirstName = "Tanja",
        //    LastName = "Saric",
        //    Address = new Address { City = "Osijek", Street = "Gunduliceva 9" },
        //    DOB = DateTime.Now.Date,
        //    SchoolId = 1
        //    },
        //    new Student
        //    {
        //    Id = 3,
        //    FirstName = "Iva",
        //    LastName = "Bulic",
        //    Address = new Address { City = "Osijek", PostalCode = "31000" },
        //    DOB = DateTime.Now.Date,
        //    SchoolId = 2
        //    }
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolMS;Integrated Security=True";
        //SqlConnection connection = new SqlConnection(connectionString);
        [HttpGet]
        [Route("api/student/get-all")]
        public HttpResponseMessage GetStudents()
        {
            try
            {
               SqlConnection connection = new SqlConnection(connectionString);
               using(connection)
                {
                    SqlCommand commmand = new SqlCommand("select * from Student", connection);
                    connection.Open();

                    SqlDataReader reader = commmand.ExecuteReader();

                    List<Student> students = new List<Student>();
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();
                            student.Id = reader.GetGuid(0);
                            student.FirstName = reader.GetString(1);
                            student.LastName = reader.GetString(2);
                            student.DOB = reader.GetDateTime(3);
                            student.Address = reader.GetString(4);
                            student.SchoolId = reader.GetGuid(5);
                            student.Average = reader.IsDBNull(6)?student.Average : reader.GetDecimal(6);

                            students.Add(student);
                        }
                        reader.Close();
                        return Request.CreateResponse(HttpStatusCode.OK, students);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No record of Students found");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing GetStudents");
            }
        }

        [HttpGet]
        //[Route("api/student/get-by-id/{id}")]
        public HttpResponseMessage GetStudent(Guid id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using(connection)
                {
                    SqlCommand command = new SqlCommand("select * from Student where Id=@id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Student student = new Student();
                    if(reader.HasRows)
                    {
                        reader.Read();
                        student.Id = reader.GetGuid(0);
                        student.FirstName = reader.GetString(1);
                        student.LastName = reader.GetString(2);
                        student.DOB = reader.GetDateTime(3);
                        student.Address = reader.GetString(4);
                        student.SchoolId = reader.GetGuid(5);
                        student.Average = reader.IsDBNull(6) ? student.Average : reader.GetDecimal(6);

                        reader.Close();
                        return Request.CreateResponse(HttpStatusCode.OK, student);
                    }                   
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No Student with an id: {id}");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting GetStudent({id})\")");             
            }        
        }
        [HttpDelete]
        public HttpResponseMessage DeleteStudent(Guid id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("delete from Student where Id=@id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int numberOfAffectedRows = command.ExecuteNonQuery();
                    if (numberOfAffectedRows > 0)
                    {                      
                        return Request.CreateResponse(HttpStatusCode.OK, $"Successfully deleted Student with an Id: {id}");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to delete Student with and Id:{id}");
                    }
                    
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
                SqlConnection connection = new SqlConnection(connectionString);
                using(connection)
                {                  
                    SqlCommand command = new SqlCommand("insert into Student values(@id, @firstName, @lastName, @dob, @address, @schoolId, @average)", connection);
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@firstName", student.FirstName);
                    command.Parameters.AddWithValue("@lastName", student.LastName);
                    command.Parameters.AddWithValue("@dob", student.DOB);
                    command.Parameters.AddWithValue("@address", student.Address);
                    command.Parameters.AddWithValue("@schoolId", student.SchoolId);
                    command.Parameters.AddWithValue("@average", (decimal)student.Average);

                    connection.Open();
                    int numberOfAffectedRows = command.ExecuteNonQuery();
                    if(numberOfAffectedRows > 0)
                    {                        
                        return Request.CreateResponse(HttpStatusCode.OK, "New Student created successfully");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to create new Student");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting CreateNewStudent(Student student)");
            }
        }

        //[HttpPut]
        //public HttpResponseMessage UpdateStudent([FromBody]int id, [FromUri] Student student)
        //{  
        //    try
        //    {
        //        Student studentToUpdate = students.Find(s => s.Id == id);
        //        if(studentToUpdate != null)
        //        {
        //            studentToUpdate.FirstName = string.IsNullOrWhiteSpace(student.FirstName)?studentToUpdate.FirstName : student.FirstName;
        //            studentToUpdate.LastName = string.IsNullOrWhiteSpace(student.LastName)?studentToUpdate.LastName : student.LastName;
        //            studentToUpdate.DOB = Convert.ToDateTime(string.IsNullOrWhiteSpace(student.DOB.ToString())?studentToUpdate.DOB : student.DOB);
        //            studentToUpdate.Address = studentToUpdate.Address;
        //            studentToUpdate.SchoolId = Convert.ToInt32(string.IsNullOrWhiteSpace(student.SchoolId.ToString()) ? studentToUpdate.SchoolId : student.SchoolId);
        //            return Request.CreateResponse(HttpStatusCode.OK, students);
        //        }

        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to find a student with an Id: {id}");
        //    }
        //    catch (Exception)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"An error occured while attempting UpdateStudent({id},student)");
        //    }

        //}
    }
}
