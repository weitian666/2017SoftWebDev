using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
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
    /// 学生数据管理控制器
    /// </summary>
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Student> _boRepository;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<BusinessImage> _imageRepository;

        private int _pageSize = 18;                               // 列表单页显示元素的条数
        private int _pageIndex = 1;                               // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1,18);             // 列表页处理所需要的参数
        private StudentVMService _boVMService;

        public StudentController(
            IEntityRepository<Student> repository,
            IEntityRepository<GradeAndClass> gradeRepository,
            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<BusinessImage> imageRepository,
            IEntityRepository<JobTitle> jobTitleRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository = repository;
            _gradeRepository = gradeRepository;
            _imageRepository = imageRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _jobTitleRepository = jobTitleRepository;
            _roleManager = roleManager;
            _userManager = userManager;

            _boVMService = new StudentVMService(_boRepository,_gradeRepository,_employeeRepository,_departmentRepository,_imageRepository,_jobTitleRepository,_userManager,_roleManager);
        }

        public async Task<IActionResult> Index()
        {
            _listPageParameter.SortProperty = "EmployeeCode";

            var boVMCollection =await  _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "学生数据列表：所有班级人员";

            return View(boVMCollection);
        }

        public async Task<IActionResult> CommonList()
        {

            _listPageParameter.SortProperty = "EmployeeCode";

            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "学生数据列表：所有班级人员";

            return PartialView("_List", boVMCollection);
        }

        public async Task<IActionResult> List(string listPageParaJson)
        {
            _listPageParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);

            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;
            ViewData["Keyword"] = _listPageParameter.Keyword;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "学生数据列表：" + _listPageParameter.TypeName;

            return PartialView("_List", boVMCollection);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "编辑学生数据";
            return PartialView("_CreateOrEdit",boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(StudentVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction("CommonList");
                else
                {
                    _boVMService.SetTypeItems(boVM);

                    ViewData["ModuleName"] = "组织与人员管理";
                    ViewData["FunctionName"] = "编辑学生数据";
                    return PartialView("_CreateOrEdit", boVM);
                }
            }

            _boVMService.SetTypeItems(boVM);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "编辑学生数据";
            return PartialView("_CreateOrEdit", boVM);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "编辑学生数据";
            return PartialView("_Detail", boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

        public IActionResult SetApplicationUser(Guid id)
        {
            var boVM = _boVMService.GetVM(id);
            var tempVM = new CreateOrEditUserWithPersonVM();

            tempVM.PersonId   = boVM.Id;
            tempVM.PersonName = boVM.Name;
            tempVM.UserId     = boVM.UserId;
            tempVM.UserName   = boVM.UserName;
            tempVM.SaveStatus = "OK";

            if (String.IsNullOrEmpty(tempVM.UserName))
                tempVM.UserName = boVM.Name;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "设置用户";
            return PartialView("_SetApplicationUser", tempVM);

        }

        [HttpPost]
        public async Task<IActionResult> SetApplicationUser(CreateOrEditUserWithPersonVM tempVM)
        {
            if (ModelState.IsValid)
            {
                var saveResult = await _boVMService.CreateOrEditRelevaneceUser(tempVM.PersonId, tempVM.UserName);
                if (saveResult.Succeeded)
                {
                    tempVM.SaveStatus = "OK";
                    var user = saveResult.BusinessObject as ApplicationUser;
                    tempVM.UserId = user.Id;
                    tempVM.UserName = user.UserName;
                }
                else
                    tempVM.SaveStatus = "Failed";
            }

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "设置用户";
            return PartialView("_SetApplicationUser", tempVM);
        }

        /// <summary>
        /// 供前端使用的部门列表数节点数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TreeViewData()
        {
            var treeViewNodes = _boVMService.GetNavigatorItems();
            return Json(treeViewNodes);
        }

    }
}