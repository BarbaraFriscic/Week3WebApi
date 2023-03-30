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
        //public int DOBFrom { get; set; }
        //public int DOBTo { get; set;}
        //public decimal Average { get; set; }
        public decimal? AverageFrom { get; set; } = null;
        public decimal? AverageTo { get; set; } = null;
    }
}
