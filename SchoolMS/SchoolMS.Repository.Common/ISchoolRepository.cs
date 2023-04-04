using SchoolMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository.Common
{
    public interface ISchoolRepository
    {
        Task<List<SchoolDTO>> GetAllSchools();
    }
}
