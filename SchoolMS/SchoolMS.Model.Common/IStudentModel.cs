using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMS.Model.Common
{
    public interface IStudentModel
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DOB { get; set; }
        string Address { get; set; }
        Guid SchoolId { get; set; }
        string SchoolName { get; set; }
        decimal? Average { get; set; }

    }
}
