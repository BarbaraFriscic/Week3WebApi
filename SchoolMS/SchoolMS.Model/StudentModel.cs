using SchoolMS.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Model
{
    public class StudentModel : IStudentModel
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

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

        public decimal? Average { get; set; }
    }
}
