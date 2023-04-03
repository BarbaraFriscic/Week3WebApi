using SchoolMS.Common;
using SchoolMS.Model;
using SchoolMS.MVC.Models;
using SchoolMS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SchoolMS.MVC.Controllers
{
    public class StudentController : Controller
    {
        protected IStudentService StudentService { get; set; }
        
        public StudentController(IStudentService studentService)
        {
            StudentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult> StudentList(DateTime? dobTo = null, DateTime? dobFrom = null, decimal? average = null,
            decimal? averageTo = null, decimal? averageFrom = null, string name = null,
            Nullable<Guid> schoolId = null, int pageNumber = 1, int pageSize = 5, string orderByColumn = "Id",
            string sortOrder = "asc")
        {
            try
            {
                Paging paging = new Paging
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
                Sorting sorting = new Sorting
                {
                    SortOrder = sortOrder,
                    OrderBy = orderByColumn,
                };
                StudentFilter filter = new StudentFilter
                {
                    Name = name == null ? null : name,
                    SchoolId = (Guid)(schoolId == null ? Guid.Empty : schoolId),
                    AverageFrom = averageFrom == null ? (decimal?)null : averageFrom,
                    AverageTo = averageTo == null ? (decimal?)null : averageTo,
                    Average = average == null ? (decimal?)null : average,
                    DOBFrom = dobFrom == null ? (DateTime?)null : dobFrom,
                    DOBTo = dobTo == null ? (DateTime?)null : dobTo,
                };
                List<StudentModelDTO> studentDtos = await StudentService.GetAllStudents(paging, sorting, filter);
                List<StudentViewModel> studentsView = new List<StudentViewModel>();
                if (studentDtos == null)
                {
                    return View("Error");
                }
                foreach (StudentModelDTO student in studentDtos)
                {
                    StudentViewModel studentView = new StudentViewModel();
                    studentView.Id = student.Id;
                    studentView.SchoolName = student.SchoolName;
                    studentView.FirstName = student.FirstName;
                    studentView.LastName = student.LastName;
                    studentsView.Add(studentView);
                }
                return View(studentsView);
            }
            catch (Exception)
            {
                return View("Error");
            }          
        }
    }
}