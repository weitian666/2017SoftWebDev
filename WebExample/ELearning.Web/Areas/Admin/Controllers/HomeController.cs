using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.Common;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        [Area("Admin")]
        public IActionResult Index()
        {
            //var userAvatarPath = "";
            //var userName = "";

            //var userIdentity = User.Identity;
            //if (String.IsNullOrEmpty(userIdentity.Name))
            //{
            //    return RedirectToAction("Login");
            //}
            //else
            //{
            //    var user = await _userManager.FindByNameAsync(userIdentity.Name);
            //    userAvatarPath = _imageRepository.GetSingleBy(x => x.RelevanceObjectID == user.Id).UploadPath;
            //    userName = user.ChineseFullName;
            //}

            //ViewData["LoginUserAvatarPath"] = userAvatarPath;
            //ViewData["LoginUserName"] = userName;

            var roles = User.Claims.Where(x => x.Type == ClaimTypes.Role);

            ViewData["ModuleName"] = "后台数据管理中心";
            ViewData["FunctionName"] = "数据列表";

            return View();
        }

        /// <summary>
        /// 进入登录界面
        /// </summary>
        /// <returns></returns>
        [Area("Admin")]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        /// <summary>
        /// 处理登录
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                // 提取用户对象
                //var user = await _userManager.FindByEmailAsync(loginVM.Email);
                var user = await _userManager.FindByNameAsync(loginVM.UserName);
                if (user != null)
                {
                    // 检查用户是否被锁定
                    if (user.LockoutEnabled)
                    {
                        // 登录系统
                        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var applicationUserVM = _boVMService.GetVM(user);
                            return RedirectToAction("Index"); 
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "你输入的用户密码错误，请核实后重新输入。");
                            return View(loginVM);
                        }
                    }
                    else
                    {
                        ViewData["LoginStatusString"] = "你的用户名已经被锁定了，无法登录。";
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

    }
}