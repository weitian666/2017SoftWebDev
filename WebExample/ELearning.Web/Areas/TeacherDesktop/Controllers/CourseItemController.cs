using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using ELearning.ViewModels.TeachingCourse;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.TeacherDesktop.Controllers
{
    /// <summary>
    /// 课程单元管理控制器
    /// </summary>
    [Area("TeacherDesktop")]
    public class CourseItemController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private readonly IEntityRepository<CourseWithRoles> _courseWithRolesRepository;
        private readonly IEntityRepository<CourseWithUsers> _courseWithUsersRepository;

        private readonly IEntityRepository<CourseItem> _boRepository;
        private readonly IEntityRepository<Course> _courseRepository;
        private readonly IEntityRepository<CourseItemContent> _courseItemContentRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageService;
        private readonly IEntityRepository<BusinessFile> _businessFileService;
        private readonly IEntityRepository<BusinessVideo> _businessVideoService;

        private int _pageSize = 18;                               // 列表单页显示元素的条数
        private int _pageIndex = 1;                               // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1, 18); // 列表页处理所需要的参数

        private CourseItemVMService _boVMService;  

        private Guid _courseId;
        private string _courseName;

        public CourseItemController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,

            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Student> studentRepository,
            IEntityRepository<Department> departmentRepository,
            IEntityRepository<GradeAndClass> gradeAndClassRepository,
            IEntityRepository<CourseWithRoles> courseWithRolesRepository,
            IEntityRepository<CourseWithUsers> courseWithUsersRepository,

            IEntityRepository<CourseItem> boRepository,
            IEntityRepository<Course> courseRepository,
            IEntityRepository<CourseItemContent> courseItemContentRepository,
            IEntityRepository<BusinessImage> image,
            IEntityRepository<BusinessFile> file,
            IEntityRepository<BusinessVideo> video)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _courseWithRolesRepository = courseWithRolesRepository;
            _courseWithUsersRepository = courseWithUsersRepository;

            _boRepository = boRepository;
            _courseRepository = courseRepository;
            _courseItemContentRepository = courseItemContentRepository;
            _businessFileService = file;
            _businessImageService = image;
            _businessVideoService = video;

            _boVMService = new CourseItemVMService(
                _userManager,
                _roleManager,
                _employeeRepository,
                _studentRepository,
                _departmentRepository,
                _gradeAndClassRepository,
                _courseWithRolesRepository,
                _courseWithUsersRepository, 
                _boRepository, _courseRepository, 
                _courseItemContentRepository,
                _businessImageService,
                _businessFileService,
                _businessVideoService
                );
        }

        /// <summary>
        /// 入口，缺省的第一个课程及其课程单元
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // 提取缺省的课程（按 SortCode 排序的第一个课程）和之下的课程单元集合
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();
            
            // 提取缺省的第一个课程视图模型
            var courseVM = _boVMService.GetCoursesVM(Guid.NewGuid()); 
            if (courseVM != null)
            {
                _courseId = courseVM.Id;
                _courseName = courseVM.Name;
            }
            else
            {
                _courseName = "未创建任何课程，无法处理课程单元数据！";
            }

            // 获取课程清单，用于在左侧导航
            ViewData["CourseCollection"] = await _boVMService.GetCoursesVMCollectionAsyn();

            ViewData["CourseId"] = _courseId;
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = _courseName;

            return View(boVMCollection);
        }

        /// <summary>
        /// 根据指定的课程 Id，获取对应的课程及其单元
        /// </summary>
        /// <param name="id">指定课程的 id</param>
        /// <returns></returns>
        public async Task<IActionResult> CourseItemList(Guid id)
        {
            var courseVMCollection = new List<CoursesVM>();
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(id);
            var courseVM = _boVMService.GetCoursesVM(id);
            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                courseVMCollection = await _boVMService.GetCoursesVMCollectionAsyn(userIdentity.Name, Entities.Tools.AdditionalItems.AuthorizationTypeEnum.编辑权限);
            }

            _courseId = courseVM.Id;
            _courseName = courseVM.Name;

            ViewData["CourseCollection"] = courseVMCollection;

            ViewData["CourseId"] = _courseId;
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = _courseName;

            return View("CourseItemList", boVMCollection);
        }

        /// <summary>
        /// 根据传入的课程 Id，获取对应课程的全部单元数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> List(Guid id)
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(id);
            var courseVM = _boVMService.GetCoursesVM(id);
            _courseId = courseVM.Id;
            _courseName = courseVM.Name;

            ViewData["CourseId"] = _courseId;
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = _courseName;

            return PartialView("_List",boVMCollection);
        }

        /// <summary>
        /// 根据传入的课程单元 Id 和课程的 courseId，获取课程单元视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id, Guid courseId)
        {
            var boVM = await _boVMService.GetVM(id,courseId);

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "课程单元管理";
            return PartialView("_CreateOrEdit", boVM);
        }

        /// <summary>
        /// 根据当前 id 新建同级单元
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEditLateralItem(Guid id, Guid courseId)
        {
            var refrenceItem = await _boVMService.GetVM(id, courseId);

            var boVM = await _boVMService.GetVM(Guid.NewGuid(), courseId);
            if (refrenceItem.Id.ToString() != refrenceItem.ParentCourseItemId)
            {
                boVM.ParentCourseItemId = refrenceItem.ParentCourseItemId;
                boVM.ParentCourseItemName = refrenceItem.ParentCourseItemName;
            }
            else
            {
                boVM.ParentCourseItemId = "";
                boVM.ParentCourseItemName = "";
            }
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "课程单元管理";
            return PartialView("_CreateOrEditLateralItem", boVM);
        }

        /// <summary>
        /// 根据当前的 id 添加下级单元
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEditSubItem(Guid id, Guid courseId)
        {
            var refrenceItem = await _boVMService.GetVM(id, courseId);

            var boVM = await _boVMService.GetVM(Guid.NewGuid(), courseId);
            boVM.ParentCourseItemId = refrenceItem.Id.ToString();
            boVM.ParentCourseItemName = refrenceItem.Name;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "课程单元管理";
            return PartialView("_CreateOrEditLateralItem", boVM);
        }


        /// <summary>
        /// 保存课程单元数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(CourseItemVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    boVM.SaveStatus = "OK";
            }

            await _boVMService.SetTypeItems(boVM,Guid.Parse(boVM.CourseId));
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "课程单元管理";

            return PartialView("_CreateOrEdit", boVM);
        }

        /// <summary>
        /// 保存课程单元数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEditSubItem(CourseItemVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    boVM.SaveStatus = "OK";
            }

            await _boVMService.SetTypeItems(boVM, Guid.Parse(boVM.CourseId));
            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = "课程单元管理";

            return PartialView("_CreateOrEditLateralItem", boVM);
        }

        /// <summary>
        /// 根据课程单元 Id，获取单元视图模型，传给前端绑定的课程单元视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 编辑教学单元内容
        /// </summary>
        /// <param name="id">注意，这是 CourseItem 的 Id </param>
        /// <returns></returns>
        public async Task<IActionResult> CourseItemContentEdit(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return View(contentVM);
        }

        public  async Task<IActionResult> CourseItemContentEditPartial(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return PartialView("_CourseItemContentEdit", contentVM);
        }

        /// <summary>
        /// 保存教学单元内容数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CourseItemContentEdit(CourseItemContentVM contentVM)
        {

            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveCourseItemContent(contentVM);
                if (x)
                {
                    contentVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                    contentVM.SaveStatus = "数据保存成功！";
                    ViewData["CourseId"] = contentVM.CourseID;
                    ViewData["CourseName"] = contentVM.CourseName;

                    ViewData["ModuleName"] = "课程内容管理";
                    ViewData["FunctionName"] = contentVM.CourseItemName;

                    return PartialView("_CourseItemContentEdit", contentVM);
                }
                else
                {
                    var tempVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                    tempVM.Name = contentVM.Name;
                    tempVM.HeadContent = contentVM.HeadContent;
                    tempVM.SecondTitle = contentVM.SecondTitle;
                    tempVM.FootContent = contentVM.FootContent;
                    tempVM.Description = contentVM.Description;

                    contentVM.SaveStatus = "数据保存出现问题，请联系有关人员协助处理！";

                    ViewData["CourseId"] = contentVM.CourseID;
                    ViewData["CourseName"] = contentVM.CourseName;

                    ViewData["ModuleName"] = "课程内容管理";
                    ViewData["FunctionName"] = contentVM.CourseItemName;

                    return PartialView("_CourseItemContentEdit", tempVM);
                }
            }
            else
            {
                var tempVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                tempVM.Name = contentVM.Name;
                tempVM.HeadContent = contentVM.HeadContent;
                tempVM.SecondTitle = contentVM.SecondTitle;
                tempVM.FootContent = contentVM.FootContent;
                tempVM.Description = contentVM.Description;

                contentVM.SaveStatus = "";

                ViewData["CourseId"] = contentVM.CourseID;
                ViewData["CourseName"] = contentVM.CourseName;

                ViewData["ModuleName"] = "课程内容管理";
                ViewData["FunctionName"] = contentVM.CourseItemName;

                return PartialView("_CourseItemContentEdit", tempVM);
            }


        }

        /// <summary>
        /// 根据回传的 CourseItemVM 的 Id 处理获取相关的  CourseItemContent 视图对象 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CourseItemContentDetail(Guid id)
        {

            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return View(contentVM);
        }

        /// <summary>
        /// 根据回传的 CourseItemVM 的 Id 处理获取相关的视图对象 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CourseItemContentDetailPartial(Guid id)
        {

            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            contentVM.SaveStatus = "";

            return PartialView("_CourseItemContentDetail", contentVM);
        }

        /// <summary>
        /// 根据回传的 CourseItemContentVM 的 Id，获取相应的 BusinessVideoVM 返回前端局部页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RefreshCourseItemContentVideo(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVMBySelfId(id);

            return PartialView("_CourseItemContentVideo",contentVM.Video);
        }

        /// <summary>
        /// 根据回传的 CourseItemContentVM 的 Id，获取相应的 BusinessFileVM 集合返回前端局部页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RefreshCourseItemContentFiles(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVMBySelfId(id);

            return PartialView("_CourseItemContentFiles", contentVM.FileCollection);
        }

        /// <summary>
        /// 根据回传的 CourseItemContentVM 的 Id，获取相应的 BusinessFileVM 集合返回前端局部页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteFilesAndRefresh(Guid id, Guid businessFileId)
        {
            var contentVM = await _boVMService.DeletCourseItemContentFile(id,businessFileId);

            return PartialView("_CourseItemContentFiles", contentVM.FileCollection);
        }

        public IActionResult TreeViewData(Guid id)
        {
            var treeViewNodes = _boVMService.GetNavigatorItems(id);
            return Json(treeViewNodes);
        }

        /// <summary>
        /// 根据课程 Id ，提取课程单元，进入课程单元内容维护管理，缺省情况获取第一个单元的内容进行呈现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Maintenance(Guid id)
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(id);
            var courseVM = _boVMService.GetCoursesVM(id);
            _courseId = courseVM.Id;
            _courseName = courseVM.Name;
            var boVM = boVMCollection.FirstOrDefault();
            if (courseVM != null)
            {
                _courseId = courseVM.Id;
                _courseName = courseVM.Name;
            }
            else
            {
                _courseName = "未创建任何课程，无法处理课程单元数据！";
            }

            var contentVM = await _boVMService.GetCourseItemContentVM(boVM.Id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.Name;
            return View(contentVM);
        }

        /// <summary>
        /// 根据课程单元 Id，获取课程单元内容视图模型，返回到预览视图 _MaintenanceDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MaintenanceDetail(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.CourseItemName;
            contentVM.SaveStatus = "";

            return PartialView("_MaintenanceDetail", contentVM);
        }

        /// <summary>
        /// 根据课程单元 Id，获取课程单元内容视图模型，返回到预览视图 _MaintenanceEdit 进行编辑处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MaintenanceEdit(Guid id)
        {
            var contentVM = await _boVMService.GetCourseItemContentVM(id);

            ViewData["CourseId"] = contentVM.CourseID;
            ViewData["CourseName"] = contentVM.CourseName;

            ViewData["ModuleName"] = "课程内容管理";
            ViewData["FunctionName"] = contentVM.CourseItemName;
            contentVM.SaveStatus = "";

            return PartialView("_MaintenanceEdit", contentVM);

        }

        /// <summary>
        /// 保存教学单元内容数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MaintenanceEdit(CourseItemContentVM contentVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveCourseItemContent(contentVM);
                if (x)
                {
                    contentVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                    contentVM.SaveStatus = "数据保存成功！";
                    ViewData["CourseId"] = contentVM.CourseID;
                    ViewData["CourseName"] = contentVM.CourseName;

                    ViewData["ModuleName"] = "课程内容管理";
                    ViewData["FunctionName"] = contentVM.CourseItemName;

                    return PartialView("_MaintenanceEdit", contentVM);
                }
                else
                {
                    var tempVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                    tempVM.Name = contentVM.Name;
                    tempVM.HeadContent = contentVM.HeadContent;
                    tempVM.SecondTitle = contentVM.SecondTitle;
                    tempVM.FootContent = contentVM.FootContent;
                    tempVM.Description = contentVM.Description;

                    contentVM.SaveStatus = "数据保存出现问题，请联系有关人员协助处理！";

                    ViewData["CourseId"] = contentVM.CourseID;
                    ViewData["CourseName"] = contentVM.CourseName;

                    ViewData["ModuleName"] = "课程内容管理";
                    ViewData["FunctionName"] = contentVM.CourseItemName;

                    return PartialView("_MaintenanceEdit", tempVM);
                }
            }
            else
            {
                var tempVM = await _boVMService.GetCourseItemContentVMBySelfId(contentVM.Id);
                tempVM.Name = contentVM.Name;
                tempVM.HeadContent = contentVM.HeadContent;
                tempVM.SecondTitle = contentVM.SecondTitle;
                tempVM.FootContent = contentVM.FootContent;
                tempVM.Description = contentVM.Description;

                contentVM.SaveStatus = "";

                ViewData["CourseId"] = contentVM.CourseID;
                ViewData["CourseName"] = contentVM.CourseName;

                ViewData["ModuleName"] = "课程内容管理";
                ViewData["FunctionName"] = contentVM.CourseItemName;

                return PartialView("_MaintenanceEdit", tempVM);
            }

        }

    }
}