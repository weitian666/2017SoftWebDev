using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.Organization;
using ELearning.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 部门机构管理
    /// </summary>
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Department> _boRepository;
        private readonly IEntityRepository<Organ> _organRepository;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private DepartmentVMService _boVMService;

        public DepartmentController(
            IEntityRepository<Department> repository,
            IEntityRepository<Organ> orgRepository,
            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository = repository;
            _roleManager = roleManager;
            _userManager = userManager;
            _organRepository = orgRepository;
            _employeeRepository = employeeRepository;
            _gradeAndClassRepository = gradeAndClassRepository;

            _boVMService = new DepartmentVMService(_boRepository, _employeeRepository, _organRepository,_gradeAndClassRepository,_userManager,_roleManager);
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "部门数据列表";
            return View(boVMCollection);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "编辑部门数据";
            return View(boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(DepartmentVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction("Index");
            }

            _boVMService.SetRelevanceItems(boVM);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "编辑部门数据";
            return View(boVM);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "部门详细数据";
            return View(boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

        /// <summary>
        /// 根据部门 Id，创建或者编辑对应的用户组
        /// </summary>
        /// <returns></returns>
        public IActionResult SetApplicationRole(Guid id)
        {
            var boVM = _boVMService.GetVM(id);
            var tempVM = new CreateOrEditRoleWithDepartmentVM();

            tempVM.DepartmentId   = boVM.Id;
            tempVM.DepartmentName = boVM.Name;
            tempVM.RoleId         = boVM.ApplicationRoleId;
            tempVM.RoleName       = boVM.ApplicationRoleName;
            tempVM.SaveStatus     = "OK";

            if (String.IsNullOrEmpty(tempVM.RoleName))
                tempVM.RoleName = boVM.Name;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "设置部门关联角色";
            return PartialView("_SetApplicationRole", tempVM);
        }

        [HttpPost]
        public async Task<IActionResult> SetApplicationRole(CreateOrEditRoleWithDepartmentVM tempVM)
        {
            if (ModelState.IsValid)
            {
                var saveResult = await  _boVMService.CreateOrEditRelevaneceRole(tempVM.DepartmentId, tempVM.RoleName);
                if (saveResult.Succeeded)
                {
                    tempVM.SaveStatus = "OK";
                    var role = saveResult.BusinessObject as ApplicationRole;
                    tempVM.RoleId = role.Id.ToString();
                    tempVM.RoleName = role.Name;
                }
                else
                    tempVM.SaveStatus = "Failed";
            }

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "设置部门关联角色";
            return PartialView("_SetApplicationRole", tempVM);
        }
    }
}