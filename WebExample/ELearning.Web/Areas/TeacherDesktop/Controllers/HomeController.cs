using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.Entities.Tools.AdditionalItems;
using ELearning.UserAndRole;
using ELearning.ViewModels.TeachingCourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.TeacherDesktop.Controllers
{
    [Area("TeacherDesktop")]
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
        private readonly IEntityRepository<Course> _boRepository;

        private readonly IAuthorizationService _authorizationService;

        private CourseVMService _boVMService;

        public HomeController(
           UserManager<ApplicationUser> userManager,
           RoleManager<ApplicationRole> roleManager,

           IEntityRepository<Employee> employeeRepository,
           IEntityRepository<Student> studentRepository,
           IEntityRepository<Department> departmentRepository,
           IEntityRepository<GradeAndClass> gradeAndClassRepository,
           IEntityRepository<CourseWithRoles> courseWithRolesRepository,
           IEntityRepository<CourseWithUsers> courseWithUsersRepository,
           IEntityRepository<Course> repository,
           IAuthorizationService authorizationService
           )
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _courseWithRolesRepository = courseWithRolesRepository;
            _courseWithUsersRepository = courseWithUsersRepository;
            _boRepository = repository;

            _authorizationService = authorizationService;

            _boVMService = new CourseVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _departmentRepository, _gradeAndClassRepository, _courseWithRolesRepository, _courseWithUsersRepository, _boRepository);
        }

        [Area("TeacherDesktop")]
        public async Task<IActionResult> Index()
        {
            var boVMCollection = new List<CoursesVM>();              // 组织授权编辑的课程
            var personCourseVMCollection = new List<CoursesVM>();    // 个人创建的课程

            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                // 组织授权编辑的课程
                boVMCollection = await _boVMService.GetboVMCollectionByUserAsyn(userIdentity.Name, AuthorizationTypeEnum.编辑权限);
                // 获取个人编写的课程
                personCourseVMCollection = await _boVMService.GetboVMCollectionByCreatorAsyn(userIdentity.Name);
            }

            ViewData["PersonCourseVMCollection"] = personCourseVMCollection;

            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程清单";
            return View(boVMCollection);
        }

                [HttpGet]
        public IActionResult CreateOrEdit(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程定义管理";
            return View("CreateOrEdit", boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(CoursesVM boVM)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = User.Identity;
                if (!String.IsNullOrEmpty(userIdentity.Name))
                {
                    boVM.CourseCreatorName = userIdentity.Name;
                    boVM.CourseAdministrtorName = userIdentity.Name;
                }

                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction("Index");
                else
                {
                    ViewData["ModuleName"] = "课程管理";
                    ViewData["FunctionName"] = "课程定义管理";
                    return View("CreateOrEdit", boVM);
                }
            }

            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程定义管理";
            return View("CreateOrEdit", boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

        /// <summary>
        /// 为指定 Id 的课程进行访问授权管理授权管理
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Authorization(Guid id)
        {
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id,false, AuthorizationTypeEnum.管理权限);

            ViewData["CourseId"]     = boVM.CourseID;
            ViewData["ModuleName"]   = "课程管理";
            ViewData["FunctionName"] = "课程访问授权："+ boVM.CourseName;

            return View(boVM);
        }

        /// <summary>
        /// 根据课程 Id 获取全部的权限清单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthorizationItemsList(Guid id)
        {
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id, false);

            ViewData["CourseId"] = boVM.CourseID;
            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程访问授权：" + boVM.CourseName;

            return PartialView("_AuthorizationItemsList",boVM);
        }

        /// <summary>
        /// 根据课程 Id 和权限类型获取全部的权限清单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthorizationTypeItemsList(Guid id, AuthorizationTypeEnum authorizationType)
        {
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id, false, authorizationType);

            ViewData["CourseId"] = boVM.CourseID;
            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程访问授权：" + boVM.CourseName;

            return PartialView("_AuthorizationItemsList", boVM);
        }

        /// <summary>
        /// 根据课程 Id 处理授权分配清单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthorizationItemsListSelector(Guid id, AuthorizationTypeEnum authorizationType)
        {
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id, true, authorizationType);

            ViewData["CourseId"]     = boVM.CourseID;
            ViewData["ModuleName"]   = "课程管理";
            ViewData["FunctionName"] = "课程访问授权："+ boVM.CourseName;

            return PartialView("_AuthorizationItemsListSelector", boVM);
        }

        /// <summary>
        /// 根据 id 的待选元素（用户或者用户组）添加到授权中或者移除从授权中移除
        /// </summary>
        /// <param name="id">课程 Id</param>
        /// <param name="itemId">用户或者用户组 Id</param>
        /// <param name="isUser">用户还是用户组</param>
        /// <param name="isAdd">添加还是移除</param>
        /// <param name="authorizationType">授权类型</param>
        /// <returns></returns>
        public async Task<IActionResult> ProcessItemToAuthorization(Guid id, Guid itemId, bool isUser, bool isAdd, AuthorizationTypeEnum authorizationType)
        {
            await _boVMService.SetAuthorizedAsync(id,itemId, isUser,isAdd,authorizationType);
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id,true,authorizationType);

            ViewData["CourseId"] = boVM.CourseID;
            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程访问授权：" + boVM.CourseName;

            return PartialView("_AuthorizationItemsListSelector", boVM);
        }

        /// <summary>
        /// 移除授权元素
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="isUser"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveItemFromAuthorization(Guid id, Guid itemId, bool isUser, bool isAdd, AuthorizationTypeEnum authorizationType)
        {
            await _boVMService.SetAuthorizedAsync(id, itemId, isUser, isAdd, authorizationType);
            var boVM = await _boVMService.GetCourseAuthorizationSelectorVMAsync(id, true, authorizationType);

            ViewData["CourseId"] = boVM.CourseID;
            ViewData["ModuleName"] = "课程管理";
            ViewData["FunctionName"] = "课程访问授权：" + boVM.CourseName;

            return PartialView("_AuthorizationItemsList", boVM);
        }

    }
}