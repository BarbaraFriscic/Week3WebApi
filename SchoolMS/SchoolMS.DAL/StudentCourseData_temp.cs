namespace SchoolMS.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentCourseData_temp
    {
        [Key]
        [StringLength(50)]
        public string FirstName { get; set; }

        public int? NumberOfGrades { get; set; }

        public int? SumGrades { get; set; }

        public decimal? AverageGrade { get; set; }
    }
}
