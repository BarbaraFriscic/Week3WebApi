using SchoolMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository
{
    public class EFSchoolRepository
    {
        public SchoolMSContext Context { get; set; }
        public EFSchoolRepository(SchoolMSContext context)
        {
            Context = context;
        }
    }
}
