﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.Organization;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.StudentDesktop.Controllers
{
    /// <summary>
    /// 学生个人资料管理控制器
    /// 1. 在允许的范围内修改自己的个人资料；
    /// 2. 修改密码；
    /// 3. 查询个人学习日志。
    /// </summary>
    [Area("StudentDesktop")]
    public class StudentProfileController : Controller
    {
        #region 控制器需要处理的数据
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;
        #endregion

        private ApplicationUserVMService _boVMService;

        public StudentProfileController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Student> studentRepository,
            IEntityRepository<BusinessImage> businessImageRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<JobTitle> jobTitleRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _businessImageRepository = businessImageRepository;
            _departmentRepository = departmentRepository;
            _jobTitleRepository = jobTitleRepository;
            _gradeAndClassRepository = gradeAndClassRepository;

            _boVMService = new ApplicationUserVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _businessImageRepository, _departmentRepository, _jobTitleRepository, _gradeAndClassRepository);
        }

        /// <summary>
        /// 根据 User 状态获取学生资料进行编辑处理
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var boVM = new ApplicationUserVM();
            var passwordResetVM = new ApplicationUserPasswordResetVM();

            #region 获取当前的访问用户信息
            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                var user = await _userManager.FindByNameAsync(userIdentity.Name);
                boVM = await _boVMService.GetVM(user);
                passwordResetVM.UserId = user.Id;
            }
            #endregion

            ViewData["ApplicationUserPasswordResetVM"] = passwordResetVM;
            ViewData["ModuleName"] = "个人信息管理";
            ViewData["FunctionName"] = "个人系统信息";
            return View(boVM);
        }


        /// <summary>
        /// 保存重置的密码
        /// </summary>
        /// <param name="resetVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> ApplicationUserPasswordReset(ApplicationUserPasswordResetVM resetVM)
        {
            if (ModelState.IsValid)
            {
                // 更新密码
                await _boVMService.ResetApplicationUserPassword(resetVM);

            }
            return PartialView("_ApplicationUserPasswordReset", resetVM);
        }

        /// <summary>
        /// 根据用户 Id 更新用户显示名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valString"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDisplayName(Guid id, string valString)
        {
            var boVM = await _boVMService.GetVM(id);
            boVM.Name = valString;
            var updateStatus = await _boVMService.SaveBo(boVM);

            return Json(updateStatus);
        }

        /// <summary>
        /// 根据用户 Id 更新用户显示名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valString"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditMobileNumber(Guid id, string valString)
        {
            var boVM = await _boVMService.GetVM(id);
            boVM.MobileNumber = valString;
            // 更新用户相关的
            var updateStatus = await _boVMService.SaveBo(boVM);
            var studentService = new StudentVMService(_studentRepository,_gradeAndClassRepository,_employeeRepository, _departmentRepository, _businessImageRepository, _jobTitleRepository, _userManager, _roleManager);
            var studentVM = await _boVMService.GetStudentVMByUserId(id);
            if (studentVM != null)
            {
                studentVM.MobileNumber = valString;
                updateStatus = await studentService.SaveBo(studentVM);
            }

            return Json(updateStatus);
        }

        /// <summary>
        /// 根据用户 Id 更新用户显示名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valString"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDescription(Guid id, string valString)
        {
            var updateStatus = true;
            // 更新用户相关的
            var studentService = new StudentVMService(_studentRepository, _gradeAndClassRepository, _employeeRepository, _departmentRepository, _businessImageRepository, _jobTitleRepository, _userManager, _roleManager);
            var studentVM = await _boVMService.GetStudentVMByUserId(id);
            if (studentVM != null)
            {
                studentVM.Description = valString;
                updateStatus = await studentService.SaveBo(studentVM);
            }

            return Json(updateStatus);
        }

        /// <summary>
        /// 根据用户 Id 更新地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valString"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditPersonAddress(Guid id, string valString)
        {
            var updateStatus = true;
            // 更新用户相关的
            var studentService = new StudentVMService(_studentRepository, _gradeAndClassRepository, _employeeRepository, _departmentRepository, _businessImageRepository, _jobTitleRepository, _userManager, _roleManager);
            var studentVM = await _boVMService.GetStudentVMByUserId(id);
            if (studentVM != null)
            {
                studentVM.Address = valString;
                updateStatus = await studentService.SaveBo(studentVM);
            }

            return Json(updateStatus);
        }


        /// <summary>
        /// 学生系统操作日志
        /// </summary>
        /// <returns></returns>
        public IActionResult OperationLog()
        {
            return View();
        }
    }
}