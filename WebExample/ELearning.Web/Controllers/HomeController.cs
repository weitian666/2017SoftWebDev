using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ELearning.Web.Models;
using ELearning.ViewModels.Common;
using Microsoft.AspNetCore.Identity;
using ELearning.UserAndRole;
using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.ViewModels.UserAndRole;
using ELearning.DataAccess.Seeds;
using ELearning.ORM.SqlServer;
using ELearning.Entities.Organization;

namespace ELearning.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<BusinessImage> _imageRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private ApplicationUserVMService _boVMService;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Student> studentRepository,
            IEntityRepository<BusinessImage> imageRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<JobTitle> jobTitleRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository
            )
        {
            _signInManager           = signInManager;
            _userManager             = userManager;
            _roleManager             = roleManager;
            _employeeRepository      = employeeRepository;
            _studentRepository       = studentRepository;
            _imageRepository         = imageRepository;
            _departmentRepository    = departmentRepository;
            _jobTitleRepository = jobTitleRepository;
            _gradeAndClassRepository = gradeAndClassRepository;

            _boVMService = new ApplicationUserVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, imageRepository,_departmentRepository,_jobTitleRepository,_gradeAndClassRepository);
        }

        /// <summary>
        /// 站点入口方法：需要判断用户是否已经登录，如果已经登录，则显示用户登录信息，否则提供登录模块
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //await ApplicationDataSeed.ForRolesAndUsers(_roleManager,_userManager);

            var userVM = new ApplicationUserVM();

            #region 获取当前的访问用户信息
            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                var user = await _userManager.FindByNameAsync(userIdentity.Name);
                userVM = await _boVMService.GetVM(user);
            } 
            #endregion

            ViewData["ApplicationUserVM"] = userVM;
            ViewData["LoginVM"] = new LoginVM();

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// 用户注册资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var functionName = "新建系统用户数据";
            var boVM = await _boVMService.GetVM();

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = functionName;
            return View(boVM);

        }

        /// <summary>
        /// 受理提交的用户注册信息
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var isUniquelyUserName = await _boVMService.IsUniquelyForUserName(userVM.UserName);
                if (!isUniquelyUserName)
                {
                    // 处理名称重复校验
                    ModelState.AddModelError("UserName", "你提交的用户名经被系统使用，用户名。");
                    return View(userVM);
                }

                await _boVMService.SaveBo(userVM);

                #region 直接登录系统
                // 提取用户对象
                var user = await _userManager.FindByEmailAsync(userVM.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, lockoutOnFailure: false);
                    return RedirectToAction("Index");
                }
                else
                {
                }

                #endregion

                return RedirectToAction("Index");
            }

            return View(userVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
