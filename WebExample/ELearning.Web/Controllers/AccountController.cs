using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.Common;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Controllers
{
    /// <summary>
    /// 普通用户账号管理相关的控制器
    /// </summary>
    public class AccountController : Controller
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

        public AccountController(
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logon(string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            return View(new LoginVM() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Logon(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                // 提取用户对象
                var user = await _userManager.FindByNameAsync(loginVM.UserName);
                if (user != null)
                {
                    // 检查用户是否被锁定
                    if (user.LockoutEnabled)
                    {
                        // 登录系统
                        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var applicationUserVM = _boVMService.GetVM(user);
                            if (!String.IsNullOrEmpty(loginVM.ReturnUrl))
                                return Redirect(loginVM.ReturnUrl);
                            else
                                return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "输入密码错误，请核实后重新输入。");
                            return View(loginVM);
                        }
                    }
                    else
                    {
                        ViewData["LoginStatusString"] = "用户已被锁定，无法登录!";
                        return View(loginVM);
                    }
                }
                else
                {
                    ViewData["LoginStatusString"] = "用户名或者密码错误。";
                    return View(loginVM);
                }

            }
            ViewData["LoginStatusString"] = "";
            return View(loginVM);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {
            var functionName = "新建系统用户数据";
            var boVM = _boVMService.GetVM();

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = functionName;
            return View(boVM);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}