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
using PagedList;
using PagedList.Mvc;
using AutoMapper;

namespace SchoolMS.MVC.Controllers
{
    public class StudentController : Controller
    {
        private IMapper _mapper { get; set; }
        protected IStudentService StudentService { get; set; }
        protected ISchoolService SchoolService { get; set; }
        
        public StudentController(IStudentService studentService, ISchoolService schoolService, IMapper mapper)
        {
            StudentService = studentService;
            SchoolService = schoolService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> StudentList(string sortBy, string searchBy, string search, int? pageNumber, int? pageSize)
        {
            try
            {
                ViewBag.SortByFirstName = string.IsNullOrEmpty(sortBy) ? "FirstName desc" : "";
                ViewBag.SortByLastName = sortBy == "LastName" ? "LastName desc" : "LastName";
                ViewBag.SortBySchoolName = sortBy == "SchoolName" ? "SchoolName desc" : "SchoolName";

                IPagedList<StudentModelDTO> studentDtos = await StudentService.GetAllStudents(sortBy, search, pageNumber ?? 1, pageSize ?? 5);
                
                if (studentDtos == null)
                {
                    return View("Error");
                }
                List<StudentListView> studentsView = studentDtos.Select(s => new StudentListView()
                {
                    FirstName = s.FirstName,
                    Id = s.Id,
                    LastName = s.LastName,
                    SchoolName = s.SchoolName
                }).ToList();
                
                var pagedList = new StaticPagedList<StudentListView>(studentsView, pageNumber ?? 1,pageSize ?? 5,studentDtos.TotalItemCount);

                return await Task.FromResult(View(pagedList));
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
                StudentEditView studentEdit = _mapper.Map<StudentEditView>(student);

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
                var studentDto = _mapper.Map<StudentEditView ,StudentModelDTO>(student);

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
                StudentListView studentView = _mapper.Map<StudentListView>(student);
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
                StudentListView studentView = _mapper.Map<StudentListView>(studentDto);
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
        public async Task<ActionResult> Create()
        {
            return await Task.FromResult(View("Create"));
        }
        [HttpPost, ActionName("Create")]
        public async Task<ActionResult> Create(StudentCreateView studentCreateView)
        {
            try
            {
                StudentModelDTO studentModel = _mapper.Map<StudentModelDTO>(studentCreateView);

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