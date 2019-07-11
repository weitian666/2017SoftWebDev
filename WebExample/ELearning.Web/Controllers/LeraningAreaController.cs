using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using ELearning.ViewModels.TeachingCourse;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Controllers
{
    public class LeraningAreaController : Controller
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

        private int _pageSize = 18;                               // 列表单页显示元素的条数
        private int _pageIndex = 1;                               // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1, 18); // 列表页处理所需要的参数

        private CourseItemVMService _boVMService;

        private Guid _courseId;
        private string _courseName;

        public LeraningAreaController(
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
        public async Task<IActionResult> Index()
        {
            ViewData["CourseCollection"] = await _boVMService.GetCoursesVMCollectionAsyn();

            return View();
        }

        /// <summary>
        /// 根据请求的课程 Course 的 Id 返回课程明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Course(Guid id)
        {
            // 获取课程
            var courseVM = _boVMService.GetCoursesVM(id);
            _courseId = courseVM.Id;
            _courseName = courseVM.Name;

            // 获取课程节点，缺省情况获取第一节点用于显示
            var courseItemVMCollection = await _boVMService.GetboVMCollectionAsyn(id);
            var firtCourseItemVM = courseItemVMCollection.FirstOrDefault();

            var contentVM = new CourseItemContentVM();
            
            // 获取课程节点明细
            if(firtCourseItemVM!=null)
                contentVM = await _boVMService.GetCourseItemContentVM(firtCourseItemVM.Id);

            ViewData["CourseId"] = _courseId;
            ViewData["CourseName"] = _courseName;

            ViewData["CourseCollection"] = await _boVMService.GetCoursesVMCollectionAsyn();

            ViewData["CourseId"] = _courseId;
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "单元列表：" + _courseName;

            return View("CourseItemContentDetail",contentVM);
        }

        public async Task<IActionResult> CourseItemContentDetail(Guid id)
        {

            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return View(contentVM);
        }

        public async Task<IActionResult> CourseItemContentDetailPartial(Guid id)
        {

            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return PartialView("_CourseItemContentDetail", contentVM);

        }

        public IActionResult TreeViewData(Guid id)
        {
            var treeViewNodes = _boVMService.GetNavigatorItems(id);
            return Json(treeViewNodes);
        }
    }
}