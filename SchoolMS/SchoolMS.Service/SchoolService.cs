using SchoolMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Service
{
    public class SchoolService
    {
        protected IStudentRepository StudentRepository { get; set; }

        public SchoolService(IStudentRepository studentRepository)
        {
            StudentRepository = studentRepository;
        }

        
}
