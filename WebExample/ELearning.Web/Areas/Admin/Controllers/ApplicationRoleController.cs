using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Seeds;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.ORM.SqlServer;
using ELearning.UserAndRole;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统角色管理控制器
    /// </summary>
    [Area("Admin")]
    public class ApplicationRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private ApplicationRoleVMService _boVMService;

        public ApplicationRoleController(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;

            _boVMService = new ApplicationRoleVMService(_userManager,_roleManager,_departmentRepository,_gradeAndClassRepository);
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();

            ViewData["ModuleName"] = "系统用户组管理";
            ViewData["FunctionName"] = "用户组数据列表";

            return View(boVMCollection);
        }

        /// <summary>
        /// 获取列表页所需要的常规数据
        /// </summary>
        /// <returns></returns>
        [Area("Admin")]
        public async Task<IActionResult> List(string keyword)
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(keyword);

            ViewData["ModuleName"] = "系统用户组管理模块";
            ViewData["FunctionName"] = "用户组数据列表";

            return View("Index",boVMCollection);
        }

        /// <summary>
        /// 新建或者编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var boVM = await _boVMService.GetVM(id);

            ViewData["ModuleName"] = "系统用户组管理";
            ViewData["FunctionName"] = "新建用户组数据";
            if (!boVM.IsNew)
                ViewData["FunctionName"] = "编辑用户组数据";

            return View(boVM);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> CreateOrEdit(ApplicationRoleVM boVM)
        {
            ViewData["FunctionName"] = "编辑用户组数据";
            ViewData["ModuleName"] = "系统用户组管理";

            if (ModelState.IsValid)
            {
                if (boVM.IsNew)
                {
                    // 对于新建数据的角色组名称进行唯一性检查
                    var isUniquelyForName = await _boVMService.IsUniquelyForName(boVM.Name);
                    if (!isUniquelyForName)
                    {
                        // 处理名称重复校验
                        ModelState.AddModelError("Name", "用户组名称重复，请重新输入一个新的角色组名称。");
                        _boVMService.SetTypeItems(boVM);
                        return View(boVM);
                    }
                }
                // 保存数据
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction(nameof(Index));
            }

            _boVMService.SetTypeItems(boVM);
            return View(boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

    }
}