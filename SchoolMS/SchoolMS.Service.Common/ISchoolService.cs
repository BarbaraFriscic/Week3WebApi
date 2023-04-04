using SchoolMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Service.Common
{
    public interface ISchoolService
    {
        Task<List<SchoolDTO>> GetAllSchools();
    }
}
