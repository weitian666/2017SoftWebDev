using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using ELearning.ViewModels.TeachingCourse;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.StudentDesktop.Controllers
{
    /// <summary>
    /// 学生学习中心入口
    /// </summary>
    [Area("StudentDesktop")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private readonly IEntityRepository<CourseWithRoles> _courseWithRolesRepository;
        private readonly IEntityRepository<CourseWithUsers> _courseWithUsersRepository;

        private readonly IEntityRepository<CourseItem> _boRepository;
        private readonly IEntityRepository<Course> _courseRepository;
        private readonly IEntityRepository<CourseItemContent> _courseItemContentRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageService;
        private readonly IEntityRepository<BusinessFile> _businessFileService;
        private readonly IEntityRepository<BusinessVideo> _businessVideoService;

        private CourseItemVMService _boVMService;

        private Guid _courseId;
        private string _courseName;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,

            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Student> studentRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository,
            IEntityRepository<CourseWithRoles> courseWithRolesRepository,
            IEntityRepository<CourseWithUsers> courseWithUsersRepository,

            IEntityRepository<CourseItem> boRepository,
            IEntityRepository<Course> courseRepository,
            IEntityRepository<CourseItemContent> courseItemContentRepository,
            IEntityRepository<BusinessImage> image,
            IEntityRepository<BusinessFile> file,
            IEntityRepository<BusinessVideo> video)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _courseWithRolesRepository = courseWithRolesRepository;
            _courseWithUsersRepository = courseWithUsersRepository;

            _boRepository = boRepository;
            _courseRepository = courseRepository;
            _courseItemContentRepository = courseItemContentRepository;
            _businessFileService = file;
            _businessImageService = image;
            _businessVideoService = video;

            _boVMService = new CourseItemVMService(
                _userManager,
                _roleManager,
                _employeeRepository,
                _studentRepository,
                _departmentRepository,
                _gradeAndClassRepository,
                _courseWithRolesRepository,
                _courseWithUsersRepository,
                _boRepository, _courseRepository,
                _courseItemContentRepository,
                _businessImageService,
                _businessFileService,
                _businessVideoService
                );
        }

        /// <summary>
        /// 入口
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {

            var courseVMCollection = await _boVMService.GetCoursesVMCollectionAsyn();
            var courseVM = courseVMCollection.FirstOrDefault();
            CourseItemContentVM contentVM = null;
            if (courseVM == null)
            {
                ViewData["ModuleName"] = "课程内容管理";
                ViewData["FunctionName"] = "";
                return View(contentVM);
            }
            else
            {
                var boVMCollection = await _boVMService.GetboVMCollectionAsyn(courseVM.Id);
                _courseId = courseVM.Id;
                _courseName = courseVM.Name;
                var boVM = boVMCollection.FirstOrDefault();
                if (courseVM != null)
                {
                    _courseId = courseVM.Id;
                    _courseName = courseVM.Name;
                }
                else
                {
                    _courseName = "未创建任何课程，无法处理课程单元数据！";
                }

                contentVM = await _boVMService.GetCourseItemContentVM(boVM.Id);

                ViewData["CourseId"] = contentVM.CourseID;
                ViewData["CourseName"] = contentVM.CourseName;

                ViewData["ModuleName"] = "课程内容管理";
                ViewData["FunctionName"] = contentVM.Name;
                return View(contentVM);
            }
        }

        /// <summary>
        /// 具体的课程学习的页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CourseDetail(Guid id)
        {
            var courseVM = _boVMService.GetCoursesVM(id);
            _courseId = courseVM.Id;
            _courseName = courseVM.Name;

            if (courseVM != null)
            {
                _courseId = courseVM.Id;
                _courseName = courseVM.Name;
            }
            else
            {
                _courseName = "未创建任何课程，无法处理课程单元数据！";
            }

            CourseItemContentVM contentVM = null;
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(id);
            var boVM = boVMCollection.FirstOrDefault();
            if (boVM != null)
            {
                contentVM = await _boVMService.GetCourseItemContentVM(boVM.Id);
                if (contentVM != null)
                {
                    ViewData["FunctionName"] = contentVM.Name;
                    ViewData["CourseId"] = contentVM.CourseID;
                }
            }

            ViewData["ModuleName"] = _courseName;
            ViewData["CourseName"] =  _courseName;
            return View(contentVM);
        }

        /// <summary>
        /// 根据课程单元 Id，获取课程单元内容视图模型，返回到预览视图 _MaintenanceDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CourseDetailPartial(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.CourseItemName;
            contentVM.SaveStatus = "";

            return PartialView("_CourseDetail", contentVM);
        }

        public IActionResult TreeViewData(Guid id)
        {
            var treeViewNodes = _boVMService.GetNavigatorItems(id);
            return Json(treeViewNodes);
        }

    }
}