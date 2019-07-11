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
using ELearning.ViewModels.Common;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统用户管理控制器
    /// </summary>
    [Area("Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private int _pageSize = 18;                                                   // 列表单页显示元素的条数
        private int _pageIndex = 1;                                                   // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1, 18);  // 列表页处理所需要的参数
        private ApplicationUserVMService _boVMService;

        public ApplicationUserController(
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

            _boVMService = new ApplicationUserVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _businessImageRepository,_departmentRepository,_jobTitleRepository,_gradeAndClassRepository);
        }

        [Area("Admin")]
        public async Task<IActionResult>Index()
        {
            // todo:目前的提取分页中，带上了过多的关联数据处理，需要简化处理一下！

            _listPageParameter.SortProperty = "UserName";
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);//new List<ApplicationUserVM>();

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;
            ViewData["RoleVMCollection"] = await _boVMService.GetRoleVMCollection();// _GetRoleVMCollection();

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = "用户数据列表:所有用户组";
            return View(boVMCollection);
        }

        public async Task<PartialViewResult> Navigator()
        {
            var itemCollection = await _boVMService.GetRoleVMCollection();
            return PartialView("_Navigator", itemCollection);
        }

        public async Task<PartialViewResult> CommonList()
        {
            _listPageParameter.SortProperty = "UserName";
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;
            ViewData["Keyword"] = "";
            ViewData["RoleVMCollection"] = await _boVMService.GetRoleVMCollection();

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = "用户数据列表:所有用户组";
            return PartialView("_List", boVMCollection);
        }

        [HttpGet]
        [HttpPost]
        public async Task<PartialViewResult> List(string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(listPagePara);

            ViewData["PageGroup"] = listPagePara.PagenateGroup;
            ViewData["ItemAmount"] = listPagePara.ObjectAmount;
            ViewData["ListPageParameter"] = listPagePara;
            ViewData["Keyword"] = listPagePara.Keyword;

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = "用户数据列表：" + listPagePara.TypeName;

            return PartialView("_List", boVMCollection);
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateOrEdit(Guid id)
        {
            var functionName = "新建系统用户数据";
            var boVM = await _boVMService.GetVM(id);
            if (!boVM.IsNew)
            {
                functionName = "编辑系统用户数据：" + boVM.UserName + "(" + boVM.Name + ")";
            }

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = functionName;

            if (boVM.IsNew)
                return PartialView("_Create", boVM);
            else
                return PartialView("_Edit", boVM);

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(ApplicationUserVM boVM)
        {
            var functionName = "新建系统用户数据";
            if (ModelState.IsValid)
            {
                if (boVM.IsNew)
                {
                    // 检验用户名的唯一性
                    var isUniquelyForUserName = await _boVMService.IsUniquelyForUserName(boVM.UserName);
                    if (!isUniquelyForUserName)
                    {
                        ModelState.AddModelError("UserName", "用户名重复，请重新输入一个。");
                    }
                    else
                    {
                        // 保存数据
                        var x = await _boVMService.SaveBo(boVM);
                        if (x)
                            return RedirectToAction("CommonList");
                    }
                }
                else
                {
                    // 保存数据
                    var x = await _boVMService.SaveBo(boVM);
                    if (x)
                        return RedirectToAction("CommonList");
                }
            }


            if (!boVM.IsNew)
                functionName = "编辑系统用户数据：" + boVM.UserName + "(" + boVM.Name + ")";

            await _boVMService.SetApplicationRoleItemCollection(boVM);

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = functionName;
            if (boVM.IsNew)
                return PartialView("_Create", boVM);
            else
                return PartialView("_Edit", boVM);
        }

        public async Task<PartialViewResult> Detail(Guid id)
        {
            var boVM = await _boVMService.GetVM(id);

            ViewData["ModuleName"] = "系统用户管理";
            ViewData["FunctionName"] = "用户明细数据：" + boVM.UserName + "(" + boVM.Name + ")";

            return PartialView("_Detail", boVM);

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.Delete(id);
            return Json(status);
        }

        public IActionResult RefreshUserAvatar(Guid id)
        {
            var path = _boVMService.GetAvatarPath(id);

            return Json(path);
        }
    }
}