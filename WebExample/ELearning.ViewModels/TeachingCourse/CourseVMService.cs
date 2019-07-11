using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.Entities.Tools.AdditionalItems;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.Organization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.TeachingCourse
{
    /// <summary>
    /// 课程视图模型处理服务
    /// </summary>
    public class CourseVMService
    {
        private readonly UserManager<ApplicationUser>       _userManager;
        private readonly RoleManager<ApplicationRole>       _roleManager;

        private readonly IEntityRepository<Employee>        _employeeRepository;
        private readonly IEntityRepository<Student>         _studentRepository;
        private readonly IEntityRepository<Department>      _departmentRepository;
        private readonly IEntityRepository<GradeAndClass>   _gradeAndClassRepository;

        private readonly IEntityRepository<CourseWithRoles> _courseWithRolesRepository;
        private readonly IEntityRepository<CourseWithUsers> _courseWithUsersRepository;
        private readonly IEntityRepository<Course>          _boRepository;

        public CourseVMService(
           UserManager<ApplicationUser> userManager,
           RoleManager<ApplicationRole> roleManager,

           IEntityRepository<Employee> employeeRepository,
           IEntityRepository<Student> studentRepository,
           IEntityRepository<Department> departmentRepository,
           IEntityRepository<GradeAndClass> gradeAndClassRepository,
           IEntityRepository<CourseWithRoles> courseWithRolesRepository,
           IEntityRepository<CourseWithUsers> courseWithUsersRepository,
           IEntityRepository<Course> repository
           )
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _courseWithRolesRepository = courseWithRolesRepository;
            _courseWithUsersRepository = courseWithUsersRepository;
            _boRepository = repository;
        }

        public CoursesVM GetVM()
        {
            return new CoursesVM();
        }

        public CoursesVM GetVM(Guid boId)
        {
            var boVM = new CoursesVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, z => z.CourseAdministrator,y=>y.Creator);
            if (bo == null)
            {
                bo = new Course();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            return boVM;
        }

        public async Task<List<CoursesVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllIncludingAsyn(x=>x.CourseAdministrator,y=>y.Creator);
            var boVMCollection = new List<CoursesVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 根据角色和授权类型获取课程集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<CoursesVM>> GetboVMCollectionByRoleAsyn(string roleName,AuthorizationTypeEnum authorizationType)
        {
            var courseWithRolesItems = await _courseWithRolesRepository.GetAllIncludingAsyn(x => x.Course, y => y.ApplicationRole);

            var boCollection = from item in courseWithRolesItems
                               where item.ApplicationRole.Name == roleName && item.AuthorizationTypeEnum == authorizationType
                               select item.Course;

            var boVMCollection = new List<CoursesVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }


        public async Task<List<CoursesVM>> GetboVMCollectionByUserAsyn(string userName, AuthorizationTypeEnum authorizationType)
        {
            var boVMCollection = new List<CoursesVM>();

            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var item in userRoles)
            {
                var tempBoCollection = await GetboVMCollectionByRoleAsyn(item, authorizationType);
                foreach (var courseItem in tempBoCollection)
                {
                    if (boVMCollection.FirstOrDefault(x => x.Id == courseItem.Id) == null)
                    {
                        courseItem.IsCreatedByMe = false;
                        boVMCollection.Add(courseItem);
                    }
                }
            }

            return boVMCollection;
        }

        /// <summary>
        /// 根据创建人获取课程集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<CoursesVM>> GetboVMCollectionByCreatorAsyn(string userName)
        {
            var tempCollection = await _boRepository.GetAllIncludingAsyn(x => x.CourseAdministrator, y => y.Creator);
            var boCollection = tempCollection.Where(x => x.Creator != null).Where(y => y.Creator.UserName == userName);
            var boVMCollection = new List<CoursesVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVM.IsCreatedByMe = true;
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        public async Task<List<CoursesVM>> GetboVMCollectionWithUser(string userName, AuthorizationTypeEnum authorizationType)
        {
            var boVMCollection = await GetboVMCollectionByCreatorAsyn(userName);
            var tempCollection = await GetboVMCollectionByUserAsyn(userName,authorizationType);
            foreach (var item in tempCollection)
            {
                if (boVMCollection.FirstOrDefault(x => x.Id == item.Id) == null)
                    boVMCollection.Add(item);
            }

            return boVMCollection;
        }

        public async Task<List<CoursesVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);

            var typeID = "";
            var keyword = "";
            var typeName = "所有部门人员";

            if (!String.IsNullOrEmpty(listPageParameter.ObjectTypeID))
                typeID = listPageParameter.ObjectTypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            // 根据查询创建的表达式
            Expression<Func<Course, bool>> predicateExpession = ExpressionFactoryMethod.GetConditionExpression<Course>(keyword);


            // 获取排序属性所需要的表达式
            var sortExpession = ExpressionFactoryMethod.GetPropertyExpression<Course, object>(listPageParameter.SortProperty);

            // 获取数据集合
            var tempCollection = _boRepository.GetAllIncluding(y => y.CourseAdministrator).Where(predicateExpession);

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            // 按照类型获取业务对象数据
            if (!String.IsNullOrEmpty(typeID))
            {
                //tempCollection = tempCollection.Where(y => y.GradeAndClass.Id == Guid.Parse(typeID));
                //typeName = _gradeRepository.GetSingle(Guid.Parse(typeID)).Name;
            }

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var boCollection = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            var boVMCollection = new List<CoursesVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<Course>(boCollection, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return boVMCollection;
        }

        public async Task<bool> SaveBo(CoursesVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id);
            if (bo == null)
                bo = new Course();

            _VMMapToBo(bo, boVM);

            if (!String.IsNullOrEmpty(boVM.CourseAdministrtorName))
                bo.CourseAdministrator = await _userManager.FindByNameAsync(boVM.CourseAdministrtorName);

            if (!String.IsNullOrEmpty(boVM.CourseCreatorName))
                bo.Creator = await _userManager.FindByNameAsync(boVM.CourseCreatorName);

            var x = await _boRepository.AddOrEditAndSaveAsyn(bo);
            return x;

        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }

        /// <summary>
        /// 根据课程 Id ，获取全部获得授权的用户或者用户组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="getTobeItems">是否同时获取待选元素集合</param>
        /// <returns></returns>
        public async Task<CourseAuthorizationSelectorVM> GetCourseAuthorizationSelectorVMAsync(Guid id, bool getTobeItems)
        {
            var course = await _boRepository.GetSingleAsyn(id);

            var authorizedCollection = new List<CourseAuthorizationVM>();
            var authorizedRoleCollection = new List<ApplicationRole>();
            var authorizedUserCollection = new List<ApplicationUser>();

            // 授权角色组
            var courseWithRoleCollection = await _courseWithRolesRepository.GetAllIncludingAsyn(x => x.ApplicationRole, y => y.Course);
            foreach (var item in courseWithRoleCollection.Where(x => x.Course.Id == id))
            {
                var courseAuthorizationViewModel = new CourseAuthorizationVM();
                courseAuthorizationViewModel.Id = item.ApplicationRole.Id;
                courseAuthorizationViewModel.Name = item.ApplicationRole.Name;
                courseAuthorizationViewModel.DisplayName = item.ApplicationRole.DisplayName;
                courseAuthorizationViewModel.SortCode = item.ApplicationRole.SortCode;
                courseAuthorizationViewModel.IsUser = false;

                authorizedRoleCollection.Add(item.ApplicationRole);
                authorizedCollection.Add(courseAuthorizationViewModel);
            }

            // 授权用户
            var courseWithUserCollection = await _courseWithUsersRepository.GetAllIncludingAsyn(x => x.ApplicationUser, y => y.Course);
            foreach (var item in courseWithUserCollection.Where(x => x.Course.Id == id))
            {
                var courseAuthorizationViewModel = new CourseAuthorizationVM();
                courseAuthorizationViewModel.Id = item.ApplicationUser.Id;
                courseAuthorizationViewModel.Name = item.ApplicationUser.UserName;
                courseAuthorizationViewModel.DisplayName = item.ApplicationUser.ChineseFullName;
                courseAuthorizationViewModel.SortCode = "";
                courseAuthorizationViewModel.IsUser = true;

                authorizedUserCollection.Add(item.ApplicationUser);
                authorizedCollection.Add(courseAuthorizationViewModel);
            }

            var courseAuthorizationSelectorVM = new CourseAuthorizationSelectorVM();
            courseAuthorizationSelectorVM.CourseID = course.Id;
            courseAuthorizationSelectorVM.CourseName = course.Name;

            // 合成
            int count = 0;
            foreach (var item in authorizedCollection.OrderBy(x => x.Name))
            {
                item.OrderNumber = (++count).ToString();
                courseAuthorizationSelectorVM.BeAuthorizationedItemCollection.Add(item);
            }

            if (getTobeItems)
            {
                var tobeAuthorizedCollection = GetToBeAuthorizeCollection(authorizedRoleCollection,authorizedUserCollection);
                foreach(var item in tobeAuthorizedCollection.OrderBy(x=>x.Name))
                    courseAuthorizationSelectorVM.ToBeAuthorizationedItemCollection.Add(item);

            }

            // 附带权限清单
            courseAuthorizationSelectorVM.AuthorizationTypeForCourseCollection = GetAuthorizationTypeForCourseCollection();
            courseAuthorizationSelectorVM.AuthorizationTypeForCourseCollection.FirstOrDefault().IsActive = true;

            return courseAuthorizationSelectorVM;

        }

        public async Task<CourseAuthorizationSelectorVM> GetCourseAuthorizationSelectorVMAsync(Guid id, bool getTobeItems, AuthorizationTypeEnum authorizationType)
        {
            var course = await _boRepository.GetSingleAsyn(id);

            var authorizedCollection = new List<CourseAuthorizationVM>();
            var authorizedRoleCollection = new List<ApplicationRole>();
            var authorizedUserCollection = new List<ApplicationUser>();

            // 授权角色组
            var courseWithRoleCollection = await _courseWithRolesRepository.GetAllIncludingAsyn(x => x.ApplicationRole, y => y.Course);
            foreach (var item in courseWithRoleCollection.Where(x => x.Course.Id == id && x.AuthorizationTypeEnum == authorizationType))
            {
                var courseAuthorizationViewModel = new CourseAuthorizationVM();
                courseAuthorizationViewModel.Id = item.ApplicationRole.Id;
                courseAuthorizationViewModel.Name = item.ApplicationRole.Name;
                courseAuthorizationViewModel.DisplayName = item.ApplicationRole.DisplayName;
                courseAuthorizationViewModel.SortCode = item.ApplicationRole.SortCode;
                courseAuthorizationViewModel.IsUser = false;
                authorizedRoleCollection.Add(item.ApplicationRole);
                authorizedCollection.Add(courseAuthorizationViewModel);
            }

            // 授权用户
            var courseWithUserCollection = await _courseWithUsersRepository.GetAllIncludingAsyn(x => x.ApplicationUser, y => y.Course);
            foreach (var item in courseWithUserCollection.Where(x => x.Course.Id == id && x.AuthorizationTypeEnum == authorizationType))
            {
                var courseAuthorizationViewModel = new CourseAuthorizationVM();
                courseAuthorizationViewModel.Id = item.ApplicationUser.Id;
                courseAuthorizationViewModel.Name = item.ApplicationUser.UserName;
                courseAuthorizationViewModel.DisplayName = item.ApplicationUser.ChineseFullName;
                courseAuthorizationViewModel.SortCode = "";
                courseAuthorizationViewModel.IsUser = true;
                courseAuthorizationViewModel.Description = await _GetCourseAuthorizationVMDescriptionByUser(item.ApplicationUser);

                authorizedUserCollection.Add(item.ApplicationUser);
                authorizedCollection.Add(courseAuthorizationViewModel);
            }

            var courseAuthorizationSelectorVM = new CourseAuthorizationSelectorVM();
            courseAuthorizationSelectorVM.CourseID = course.Id;
            courseAuthorizationSelectorVM.CourseName = course.Name;

            // 合成
            int count = 0;
            foreach (var item in authorizedCollection.OrderBy(x => x.Name))
            {
                item.OrderNumber = (++count).ToString();
                courseAuthorizationSelectorVM.BeAuthorizationedItemCollection.Add(item);
            }

            if (getTobeItems)
            {
                var tobeAuthorizedCollection = GetToBeAuthorizeCollection(authorizedRoleCollection, authorizedUserCollection);
                foreach (var item in tobeAuthorizedCollection.OrderBy(x => x.Name))
                    courseAuthorizationSelectorVM.ToBeAuthorizationedItemCollection.Add(item);

            }

            // 附带权限清单
            courseAuthorizationSelectorVM.AuthorizationType = authorizationType;
            courseAuthorizationSelectorVM.AuthorizationTypeForCourseCollection = GetAuthorizationTypeForCourseCollection();
            foreach(var item in courseAuthorizationSelectorVM.AuthorizationTypeForCourseCollection)
                if(item.AuthorizationType==authorizationType)
                    item.IsActive = true;

            return courseAuthorizationSelectorVM;

        }

        /// <summary>
        /// 根据课程 Id ，获取全部等待授权的用户或者用户组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CourseAuthorizationVM> GetToBeAuthorizeCollection(List<ApplicationRole> authorizedRoleCollection,List<ApplicationUser> authorizedUserCollection)
        {
            var roleCollection = _roleManager.Roles;
            var userCollection = _userManager.Users;
            var tobeAuthorizedItemCollection = new List<CourseAuthorizationVM>();

            foreach (var item in roleCollection)
            {
                if (!authorizedRoleCollection.Contains(item))
                {
                    var courseAuthorizationViewModel         = new CourseAuthorizationVM();
                    courseAuthorizationViewModel.Id          = item.Id;
                    courseAuthorizationViewModel.Name        = item.Name;
                    courseAuthorizationViewModel.DisplayName = item.DisplayName;
                    courseAuthorizationViewModel.SortCode    = item.SortCode;
                    courseAuthorizationViewModel.IsUser      = false;

                    tobeAuthorizedItemCollection.Add(courseAuthorizationViewModel);
                }
            }

            // 关闭待选授权元素的用户，在选择用户时，通过关键词来选
            //foreach (var item in userCollection)
            //{
            //    if (!authorizedUserCollection.Contains(item))
            //    {
            //        var courseAuthorizationViewModel = new CourseAuthorizationVM();
            //        courseAuthorizationViewModel.Id          = item.Id;
            //        courseAuthorizationViewModel.Name        = item.UserName;
            //        courseAuthorizationViewModel.DisplayName = item.ChineseFullName;
            //        courseAuthorizationViewModel.SortCode    = "";
            //        courseAuthorizationViewModel.IsUser      = true;

            //        tobeAuthorizedItemCollection.Add(courseAuthorizationViewModel);
            //    }
            //}

            return tobeAuthorizedItemCollection;
        }

        /// <summary>
        /// 根据前端权限配置的请求参数，重新配置分配清单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="isUser"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task SetAuthorizedAsync(Guid id, Guid itemId, bool isUser, bool isAdd, AuthorizationTypeEnum authorizationType)
        {
            var course = await _boRepository.GetSingleAsyn(id);

            if (isUser)
            {
                var user = await _userManager.FindByIdAsync(itemId.ToString());
                if (isAdd)
                {
                    var addUserItem = new CourseWithUsers() { ApplicationUser = user, Course = course, AuthorizationTypeEnum = authorizationType};
                    await _courseWithUsersRepository.AddOrEditAndSaveAsyn(addUserItem);
                }
                else
                {
                    var removeUserItem = await _courseWithUsersRepository.GetSingleAsyn(x => x.Course.Id == id && x.ApplicationUser.Id == user.Id && x.AuthorizationTypeEnum == authorizationType);
                    if (removeUserItem != null)
                        await _courseWithUsersRepository.DeleteAndSaveAsyn(removeUserItem.Id);
                }
            }
            else
            {
                var role = await _roleManager.FindByIdAsync(itemId.ToString());
                if (isAdd)
                {
                    var addRoleItem = new CourseWithRoles() { ApplicationRole = role, Course = course, AuthorizationTypeEnum = authorizationType };
                    await _courseWithRolesRepository.AddOrEditAndSaveAsyn(addRoleItem);
                }
                else
                {
                    var removeRoleItem = await _courseWithRolesRepository.GetSingleAsyn(x => x.Course.Id == id && x.ApplicationRole.Id == role.Id && x.AuthorizationTypeEnum == authorizationType);
                    if (removeRoleItem != null)
                        await _courseWithRolesRepository.DeleteAndSaveAsyn(removeRoleItem.Id);
                }
            }
        }

        /// <summary>
        /// 权限清单
        /// </summary>
        /// <returns></returns>
        public List<AuthorizationTypeForCourseVM> GetAuthorizationTypeForCourseCollection()
        {
            var resultItems = new List<AuthorizationTypeForCourseVM>();
            var enumItems = Enum.GetValues(typeof(AuthorizationTypeEnum));
            foreach (var eItem in enumItems)
            {
                var item = new AuthorizationTypeForCourseVM()
                {
                    AuthorizationType = (AuthorizationTypeEnum)eItem,
                    AuthorizationTypeEnumName = eItem.ToString(),
                };
                switch (item.AuthorizationType)
                {
                    //case AuthorizationTypeEnum.完全权限:  // 完全权限, 包含可以分配的最高权限
                    //    item.AuthorizationTypeDisplayName = "课程维护";
                    //    item.AuthorizationDescription = "拥有最高权限的个人或者用户组，可以处理包括课程创建，课程编辑、分配课程给教师权限等。";
                    //    break;
                    case AuthorizationTypeEnum.管理权限:  // 管理权限, 包含对资源 CRUD 全部操作
                        item.AuthorizationTypeDisplayName = "课程管理";
                        item.AuthorizationDescription = "拥有对于所分配到的课程的进行内容编辑，可以自行创建课程，对于自行创建的课程具有删除权限，以及对课程只读访问对象（例如学生）具有授权分配的权限。";
                        break;
                    case AuthorizationTypeEnum.编辑权限:  // 编辑权限, 包含对于自己创建的资源对象的全部 CRUD，别人分配的资源的 RU 权限
                        item.AuthorizationTypeDisplayName = "课程编辑";
                        item.AuthorizationDescription = "拥有对所分配的课程进行内容编辑，但不能新建课程，以及对课程只读访问对象（例如学生）具有授权分配的权限。";
                        break;
                    case AuthorizationTypeEnum.完全阅读权限:  // 完全阅读权限, 包含对授权资源对象的完整的 R 权限
                        item.AuthorizationTypeDisplayName = "课程读者";
                        item.AuthorizationDescription = "拥有对课程进行完整内容访问阅读的权限";
                        break;
                    case AuthorizationTypeEnum.局部阅读权限:  // 局部阅读权限, 根据业务逻辑指定阅读内容范围的 R 权限
                        item.AuthorizationTypeDisplayName = "预览读者";
                        item.AuthorizationDescription = "拥有对课程进行预览限制内容访问阅读的权限。";
                        break;
                    default:
                        item.AuthorizationTypeDisplayName = "";
                        item.AuthorizationDescription = "";
                        break;
                }
                if(!String.IsNullOrEmpty(item.AuthorizationTypeDisplayName))
                    resultItems.Add(item);
            }

            return resultItems;
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(Course bo, CoursesVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;
            boVM.OpenDate = bo.OpenDate.ToString("yyyy-MM-dd");
            boVM.CloseDate = bo.CloseDate.ToString("yyyy-MM-dd");

            if (bo.CourseAdministrator != null)
                boVM.CourseAdministrtorName = bo.CourseAdministrator.ChineseFullName;
            if (bo.Creator != null)
                boVM.CourseCreatorName = bo.Creator.ChineseFullName;

        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _VMMapToBo(Course bo, CoursesVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.Description = boVM.Description;
            bo.SortCode = boVM.SortCode;
            bo.OpenDate = DateTime.Parse(boVM.OpenDate);
            bo.CloseDate = DateTime.Parse(boVM.CloseDate);
        }

        private async Task<string> _GetCourseAuthorizationVMDescriptionByUser(ApplicationUser user)
        {
            var description = "";
            var students = await _studentRepository.GetAllIncludingAsyn(y => y.User, z => z.GradeAndClass);
            var student = students.Where(x => x.User != null && x.User.Id == user.Id).FirstOrDefault();
            if (student == null)
            {
                var employee = await _employeeRepository.GetSingleAsyn(x => x.User == user);
                if (employee == null)
                {
                }
            }
            else
            {
                if (student.GradeAndClass != null)
                {
                    var items = SelfReferentialItemFactory<GradeAndClass>.GetCollectionToRoot(_gradeAndClassRepository, student.GradeAndClass).OrderBy(x=>x.SortCode);
                    foreach (var item in items)
                        description = description + " " + item.DisplayName;

                    return description;
                }
            }

            return description;
        }
    }
}
