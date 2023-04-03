using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMS.MVC.Models
{
    public class StudentView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SchoolName { get; set; }
    }
}