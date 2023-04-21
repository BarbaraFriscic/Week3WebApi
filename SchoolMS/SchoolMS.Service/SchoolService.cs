using SchoolMS.Model;
using SchoolMS.Repository.Common;
using SchoolMS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Service
{
    public class SchoolService : ISchoolService
    {
        public ISchoolRepository Repository { get; set; }
        public SchoolService(ISchoolRepository repository)
        {
            Repository = repository;
        }
        
        public async Task<List<SchoolDTO>> GetAllSchools()
        {
           List<SchoolDTO> schools = await Repository.GetAllSchools();

           return schools;           
        }
    }
}
