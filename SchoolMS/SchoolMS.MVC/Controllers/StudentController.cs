using SchoolMS.Common;
using SchoolMS.Model;
using SchoolMS.MVC.Models;
using SchoolMS.MVC.Models.StudentView;
using SchoolMS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
                List<StudentListView> studentsView = new List<StudentListView>();
                if (studentDtos == null)
                {
                    return View("Error");
                }
                foreach (StudentModelDTO student in studentDtos)
                {
                    StudentListView studentView = new StudentListView();
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
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            try
            {
                StudentModelDTO student = await StudentService.GetStudent(id);
                if (student == null)
                {
                    return View("Error");
                }
                StudentEditView studentEdit = new StudentEditView();
                studentEdit.Id = id;
                studentEdit.Address = student.Address;
                studentEdit.FirstName = student.FirstName;
                studentEdit.LastName = student.LastName;

                return View(studentEdit);
            }
            catch (Exception)
            {
                return View("Error");
            }         
        }
        [HttpPost]
        public async Task<ActionResult> Edit(StudentEditView student)
        {
            try
            {
                StudentModelDTO studentDto = new StudentModelDTO();
                studentDto.Id = student.Id;
                studentDto.Address = student.Address;
                studentDto.FirstName = student.FirstName;
                studentDto.LastName = student.LastName;

                bool isEdited = await StudentService.EditStudent(studentDto.Id, studentDto);
                if (!isEdited)
                {
                    return View("Error");
                }
                return RedirectToAction("StudentList");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                StudentModelDTO student = await StudentService.GetStudent(id);
                if (student == null)
                {
                    return View("Error");
                }
                StudentListView studentView = new StudentListView
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Id = id,
                    SchoolName = student.SchoolName
                };
                return View(studentView);
            }
            catch (Exception)
            {
                return View("Error");
            }            
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                StudentModelDTO studentDto = await StudentService.GetStudent(id);
                if(studentDto == null)
                {
                    return View("Error");
                }
                StudentListView studentView = new StudentListView
                {
                    FirstName=studentDto.FirstName,
                    LastName=studentDto.LastName,
                    Id = id,
                    SchoolName = studentDto.SchoolName
                };
                return View(studentView);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                bool isDeleted = await StudentService.DeleteStudent(id);
                if (!isDeleted)
                {
                    return View("Error");
                }
                return RedirectToAction("StudentList");
            }
            catch (Exception)
            {
                return View("Error");
            }           
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost, ActionName("Create")]
        public async Task<ActionResult> Create(StudentCreateView studentCreateView)
        {
            try
            {
                
                StudentModelDTO studentModel = new StudentModelDTO();
                studentModel.FirstName = studentCreateView.FirstName;
                studentModel.LastName = studentCreateView.LastName;
                studentModel.Address = studentCreateView.Address;
                studentModel.DOB = studentCreateView.DOB;
                studentModel.SchoolId = studentCreateView.SchoolId;

                bool isAdded = await StudentService.AddNewStudent(studentModel);
                if(!isAdded)
                {
                    return View("Error");
                }
                return RedirectToAction("StudentList");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}