namespace SchoolMS.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentCourseData_temp4
    {
        [Key]
        public Guid StudentId { get; set; }

        public decimal? AverageGrade { get; set; }
    }
}
