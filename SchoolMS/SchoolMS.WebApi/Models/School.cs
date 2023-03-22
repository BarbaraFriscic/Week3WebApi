using System;
using System.Collections.Generic;
using SchoolMS.WebApi.Models;
using System.Linq;
using System.Web;

namespace SchoolMS.WebApi.Models
{
    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }              
    }
}