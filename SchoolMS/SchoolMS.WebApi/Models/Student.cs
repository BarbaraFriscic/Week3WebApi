using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace SchoolMS.WebApi.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        [MinLength(2, ErrorMessage ="Minimal length is 2 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Minimal length is 2 characters")]
        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public Address Address { get; set; }

        public int SchoolId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\nFirst Name: {FirstName}\nLast Name: {LastName}\nDOB: {DOB}\nSchoolId: {SchoolId}";
        }

    }
}