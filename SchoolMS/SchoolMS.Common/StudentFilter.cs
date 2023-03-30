using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Common
{
    public class StudentFilter
    {
        public string Name { get; set; }       
        public Guid? SchoolId { get; set; }
        public DateTime? DOBFrom { get; set; } = null;
        public DateTime? DOBTo { get; set; } = null;
        public decimal? Average { get; set; } = null;
        public decimal? AverageFrom { get; set; } = null;
        public decimal? AverageTo { get; set; } = null;
    }
}
