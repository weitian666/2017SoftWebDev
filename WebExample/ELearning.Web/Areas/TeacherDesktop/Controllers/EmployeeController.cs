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

namespace ELearning.Web.Areas.TeacherDesktop.Controllers
{
    /// <summary>
    /// 员工数据管理控制器
    /// </summary>
    [Area("TeacherDesktop")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Employee> _boRepository;
        private readonly IEntityRepository<Student> _studentpository;
        private readonly IEntityRepository<GradeAndClass> _gradeRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<BusinessImage> _imageRepository;

        private int _pageSize = 18;                               // 列表单页显示元素的条数
        private int _pageIndex = 1;                               // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1, 18);             // 列表页处理所需要的参数
        private EmployeeVMService _boVMService;

        public EmployeeController(
            IEntityRepository<Employee> repository,
            IEntityRepository<Student> studentpository,
            IEntityRepository<GradeAndClass> gradeRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<BusinessImage> imageRepository,
            IEntityRepository<JobTitle> jobTitleRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository = repository;
            _studentpository = studentpository;
            _departmentRepository = departmentRepository;
            _gradeRepository = gradeRepository;
            _imageRepository = imageRepository;
            _jobTitleRepository = jobTitleRepository;
            _roleManager = roleManager;
            _userManager = userManager;

            _boVMService = new EmployeeVMService(_boRepository,_studentpository,_gradeRepository,_departmentRepository,_imageRepository,_jobTitleRepository,_userManager,_roleManager);
        }

        public async Task<IActionResult> Index()
        {

            _listPageParameter.SortProperty = "EmployeeCode";
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "员工数据列表：所有部门人员";
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
            ViewData["FunctionName"] = "员工数据列表：所有部门人员";

            return PartialView("_List",boVMCollection);
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
            ViewData["FunctionName"] = "员工数据列表：" + _listPageParameter.TypeName;

            return PartialView("_List", boVMCollection);
        }

        public IActionResult Detail(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "组织与人员管理";
            ViewData["FunctionName"] = "员工明细数据";
            return PartialView("_Detail", boVM);
        }


        /// <summary>
        /// 供前端使用的部门列表数节点数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TreeViewData()
        {
            var departmentTreeViewNodes = TreeViewFactory.GetTreeNodes<Department>(_departmentRepository);
            return Json(departmentTreeViewNodes);
        }
    }
}