using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
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
    /// 班级管理控制器
    /// </summary>
    [Area("Admin")]
    public class GradeAndClassController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<Organ> _orgRepository;
        private readonly IEntityRepository<GradeAndClass> _boRepository;

        private GradeAndClassVMService _boVMService;

        public GradeAndClassController(
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<Student> studentRepository,
            IEntityRepository<Organ> orgRepository,
            IEntityRepository<GradeAndClass> repository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository = repository;
            _studentRepository = studentRepository;
            _orgRepository = orgRepository;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
            _roleManager = roleManager;

            _boVMService = new GradeAndClassVMService(_departmentRepository, _studentRepository, _orgRepository, _boRepository, _userManager, _roleManager);
        }


        public async Task<IActionResult> Index()
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();

            ViewData["ModuleName"] = "班级与学生管理";
            ViewData["FunctionName"] = "班级数据列表";

            return View(boVMCollection);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "班级与学生管理";
            ViewData["FunctionName"] = "编辑班级数据";
            return View(boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(GradeAndClassVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction("Index");
            }

            _boVMService.SetRelevanceItems(boVM);

            ViewData["ModuleName"] = "班级与学生管理";
            ViewData["FunctionName"] = "编辑班级数据";
            return View(boVM);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "班级与学生管理";
            ViewData["FunctionName"] = "班级详细数据";
            return View(boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

        /// <summary>
        /// 根据班级 Id，创建或者编辑对应的用户组
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
            ViewData["FunctionName"] = "设置班级关联角色";
            return PartialView("_SetApplicationRole", tempVM);
        }

        [HttpPost]
        public async Task<IActionResult> SetApplicationRole(CreateOrEditRoleWithDepartmentVM tempVM)
        {
            if (ModelState.IsValid)
            {
                var saveResult = await _boVMService.CreateOrEditRelevaneceRole(tempVM.DepartmentId, tempVM.RoleName);
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
            ViewData["FunctionName"] = "设置班级关联角色";
            return PartialView("_SetApplicationRole", tempVM);
        }
    }
}