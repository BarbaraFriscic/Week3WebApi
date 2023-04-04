using SchoolMS.DAL;
using SchoolMS.Model;
using SchoolMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Repository
{
    public class EFSchoolRepository : ISchoolRepository
    {
        public SchoolMSContext Context { get; set; }
        public EFSchoolRepository(SchoolMSContext context)
        {
            Context = context;
        }
        public async Task<List<SchoolDTO>> GetAllSchools()
        {
            try
            {
                List<SchoolDTO> schools = await Context.School.Select(s => new SchoolDTO()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                }).ToListAsync();
                if(schools.Count > 0)
                {
                    return schools;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
