using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.Organization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.UserAndRole
{
    /// <summary>
    /// 用户模型交互处理：负责在业务实体模型和业务实体视图模型之间的所有与持久层交互的处理操作实现
    /// </summary>
    public class ApplicationUserVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private ApplicationRoleVMService _roleService;


        public ApplicationUserVMService(
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

            _roleService = new ApplicationRoleVMService(_userManager, _roleManager, _departmentRepository, _gradeAndClassRepository);
        }

        /// <summary>
        /// 返回一个新的视图模型
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetVM()
        {
            var boVM = new ApplicationUserVM();
            await SetTypeItems(boVM);
            return boVM;
        }

        /// <summary>
        /// 根据系统用户对象 Id 返回一个系统用户视图模型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetVM(Guid id)
        {
            var boVM = new ApplicationUserVM();
            var bo = await _userManager.FindByIdAsync(id.ToString());
            if (bo != null)
            {
                boVM.IsNew = false;
                _BoMapToVM(bo, boVM);
            }

            await SetTypeItems(boVM);

            return boVM;
        }

        /// <summary>
        /// 根据系统用户对象返回一个系统用户视图模型对象
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetVM(ApplicationUser bo)
        {
            var boVM = new ApplicationUserVM();
            _BoMapToVM(bo, boVM);
            await SetTypeItems(boVM);

            return boVM;
        }

        /// <summary>
        /// 根据系统用户对象返回一个系统用户视图模型对象
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public ApplicationUserVM GetVMForList(ApplicationUser bo)
        {
            var boVM = new ApplicationUserVM();
            _BoMapToVM(bo, boVM);
            //await SetTypeItems(boVM);

            return boVM;
        }

        /// <summary>
        /// 根据学生 Id 返回系统用户视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUserVM> GetUserByStudentId(Guid id)
        {
            var boVM = new ApplicationUserVM();


            await SetTypeItems(boVM);
            return boVM;
        }

        /// <summary>
        /// 根据员工 Id 返回系统用户视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationUserVM GetUserByEmployeetId(Guid id)
        {
            var userVM = new ApplicationUserVM();
            return userVM;
        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<ApplicationUserVM>> GetboVMCollectionAsyn()
        {
            var boCollection = _userManager.Users.AsQueryable().OrderBy(x => x.UserName);
            var boVMCollection = new List<ApplicationUserVM>();

            var counter = 0;
            foreach (var user in boCollection)
            {
                var boVM = await GetVM(user);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 根据列表分页器的参数
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <param name="boService"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<List<ApplicationUserVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);

            var typeID = "";
            var keyword = "";
            var typeName = "所有用户";
            var objectAmount = _userManager.Users.Count();

            if (!String.IsNullOrEmpty(listPageParameter.ObjectTypeID))
                typeID = listPageParameter.ObjectTypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            #region 1.构建与 keyword 相关的查询 lambda 表达式，用于对查询结果的过滤（给 Where 使用）
            Expression<Func<ApplicationUser, bool>> predicateExpession = x =>
                 x.UserName.Contains(keyword) ||
                 x.ChineseFullName.Contains(keyword) ||
                 x.FirstName.Contains(keyword) ||
                 x.LastName.Contains(keyword) ||
                 x.MobileNumber.Contains(keyword);
            #endregion

            #region 2.根据属性名称确定排序的属性的 lambda 表达式
            var sortPropertyName = listPageParameter.SortProperty;
            var type = typeof(ApplicationUser);
            var target = Expression.Parameter(typeof(object));
            var castTarget = Expression.Convert(target, type);
            var getPropertyValue = Expression.Property(castTarget, sortPropertyName);
            var sortExpession = Expression.Lambda<Func<ApplicationUser, object>>(getPropertyValue, target);
            #endregion

            PaginatedList<ApplicationUser> boCollection = new PaginatedList<ApplicationUser>();
            var roleId = listPageParameter.ObjectTypeID;
            if (String.IsNullOrEmpty(roleId))
            {
                var tempCollection = _userManager.Users.Where(predicateExpession);
                objectAmount = tempCollection.Count();

                if (listPageParameter.SortDesc == "")
                {
                    boCollection = tempCollection.OrderByDescending(sortExpession).AsQueryable().ToPaginatedList(pageIndex, pageSize);
                }
                else
                {
                    boCollection = tempCollection.OrderBy(sortExpession).AsQueryable().ToPaginatedList(pageIndex, pageSize);
                }
            }
            else
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                var tempCollection = await _userManager.GetUsersInRoleAsync(role.Name);
                typeName = role.Name;
                objectAmount = tempCollection.Count();

                if (listPageParameter.SortDesc == "")
                {
                    boCollection = tempCollection.AsQueryable().OrderByDescending(sortExpession).ToPaginatedList(pageIndex, pageSize);
                }
                else
                {
                    boCollection = tempCollection.AsQueryable().OrderBy(sortExpession).ToPaginatedList(pageIndex, pageSize);
                }
            }

            var boVMCollection = new List<ApplicationUserVM>();
            var counter = 0;
            foreach (var user in boCollection)
            {
                var boVM = GetVMForList(user);
                boVM.OrderNumber = (++counter + (pageIndex - 1) * pageSize).ToString();
                boVMCollection.Add(boVM);
            }


            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<ApplicationUser>(boCollection, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return boVMCollection;
        }

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsUniquelyForUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return (user == null) ? true : false;
        }

        /// <summary>
        /// 保存系统用户数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public async Task<bool> SaveBo(ApplicationUserVM boVM)
        {
            var bo = await _userManager.FindByIdAsync(boVM.Id.ToString());
            if (bo == null)
            {
                #region 新建用户
                bo = new ApplicationUser();
                _VMMapToBo(bo, boVM);


                var result = await _userManager.CreateAsync(bo, boVM.Password);
                if (result.Succeeded)
                {
                    if (boVM.ApplicationRoleItemIdCollection != null)
                    {
                        foreach (var item in boVM.ApplicationRoleItemIdCollection)
                        {
                            var role = await _roleManager.FindByIdAsync(item);
                            var addToRole = await _userManager.AddToRoleAsync(bo, role.Name);

                            // 检查用户关联角色组类型声明
                            var userClaims = await _userManager.GetClaimsAsync(bo);
                            var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == role.ApplicationRoleType.ToString());
                            // 添加角色组类型声明
                            if (userRoleTypeClaim == null)
                            {
                                // 为用户创建一个归属系统角色类型的身份声明
                                var claim = new Claim(ClaimTypes.Name, role.ApplicationRoleType.ToString());
                                await _userManager.AddClaimAsync(bo, claim);
                            }

                            //if (!addToRole.Succeeded)
                            //    return false;
                        }
                    }
                    else
                    {
                        // 添加到缺省组
                        var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.ApplicationRoleType == ApplicationRoleTypeEnum.适用于普通注册用户);
                        if (defaultRole != null)
                            await _userManager.AddToRoleAsync(bo, defaultRole.Name);

                    }
                }
                else
                    return false;
                #endregion
            }
            else
            {
                _VMMapToBo(bo, boVM);
                var result = await _userManager.UpdateAsync(bo);
                if (result.Succeeded)
                {
                    // 清空归属角色组
                    var userRoles = await _userManager.GetRolesAsync(bo);
                    foreach (var item in userRoles)
                        await _userManager.RemoveFromRoleAsync(bo, item);

                    // 重新根据选择项添加用户归属的角色
                    if (boVM.ApplicationRoleItemIdCollection != null)
                    {
                        foreach (var item in boVM.ApplicationRoleItemIdCollection)
                        {
                            var role = await _roleManager.FindByIdAsync(item);
                            var isInRole = await _userManager.IsInRoleAsync(bo, role.Name);
                            if(!isInRole)
                                await _userManager.AddToRoleAsync(bo, role.Name);

                            // 检查用户关联角色组类型声明
                            var userClaims = await _userManager.GetClaimsAsync(bo);
                            var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == role.ApplicationRoleType.ToString());
                            // 添加角色组类型声明
                            if (userRoleTypeClaim == null)
                            {
                                // 为用户创建一个归属系统角色类型的身份声明
                                var claim = new Claim(ClaimTypes.Name, role.ApplicationRoleType.ToString());
                                await _userManager.AddClaimAsync(bo, claim);
                            }
                        }
                    }
                    else
                    {
                        // 添加到缺省组
                        var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.ApplicationRoleType == ApplicationRoleTypeEnum.适用于普通注册用户);
                        if(defaultRole!=null)
                            await _userManager.AddToRoleAsync(bo, defaultRole.Name);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteStatusModel> Delete(Guid id)
        {
            var bo = await _userManager.FindByIdAsync(id.ToString());
            var status = new DeleteStatusModel() { DeleteSatus = true, Message = "数据删除成功" };
            try
            {
                await _userManager.DeleteAsync(bo);
            }
            catch
            {
                status.DeleteSatus = false;
                status.Message = "删除操作出现意外，主要原因是关联数据没有处理干净活者是其他原因。";
            }

            return status;
        }

        /// <summary>
        /// 获取系统用户的头像
        /// </summary>
        /// <returns></returns>
        public string GetAvatarPath(Guid id)
        {
            // 设置头像路径
            Func<Guid, string> avatarPath = (itemId) =>
            {
                var path = "";
                var imageBo = _businessImageRepository.GetSingleBy(x => x.RelevanceObjectID == itemId);
                if (imageBo != null)
                    path = imageBo.UploadPath;
                return path;
            };

            return avatarPath(id);
        }

        /// <summary>
        /// 根据用户 Id 返回员工视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmployeeVM> GetEmployeeVMByUserId(Guid id)
        {
            EmployeeVM employeeVM = null;
            var employeeService = new EmployeeVMService(_employeeRepository, _studentRepository, _gradeAndClassRepository, _departmentRepository, _businessImageRepository, _jobTitleRepository, _userManager, _roleManager);
            var employeeUserItems = await _employeeRepository.GetAllIncludingAsyn(x => x.User);
            var employeeUser = employeeUserItems.Where(x => x.User != null).FirstOrDefault(x => x.User.Id == id);
            if (employeeUser != null)
            {
                employeeVM = employeeService.GetVM(employeeUser.Id);
            }
            return employeeVM;
        }

        /// <summary>
        /// 根据用户 Id 返回学生视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StudentVM> GetStudentVMByUserId(Guid id)
        {
            StudentVM studentVM = null;
            var studentService = new StudentVMService(_studentRepository,_gradeAndClassRepository,_employeeRepository,_departmentRepository,_businessImageRepository,_jobTitleRepository,_userManager,_roleManager);
            var studentUserItems = await _employeeRepository.GetAllIncludingAsyn(x => x.User);
            var studentUser = studentUserItems.Where(x => x.User != null).FirstOrDefault(x => x.User.Id == id);
            if (studentUser != null)
            {
                studentVM = studentService.GetVM(studentUser.Id);
            }

            return studentVM;
        }

        /// <summary>
        /// 根据学生视图模型创建或者维护与之匹配的用户数据，并进行持久化处理
        ///   1.缺省用户名采用学号数据
        ///   2.缺省密码：1234@Abcd
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<EntityProcessResult> CreateOrEditByStudent(Student student,string userName = null)
        {
            var result = new EntityProcessResult() { Succeeded = true, Messages = new List<string> { } };
            if (student.User == null)
            {
                // 检查重名
                var checkName = student.EmployeeCode;
                if (!String.IsNullOrEmpty(userName))
                    checkName = userName;

                var isUniquelyForName = await IsUniquelyForUserName(checkName);
                if (isUniquelyForName)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = student.EmployeeCode,
                        ChineseFullName = student.Name,
                        Email = student.Email,
                        MobileNumber = student.Mobile
                    };
                    if (!String.IsNullOrEmpty(userName))
                        user.UserName = userName;

                    var createResult = await _userManager.CreateAsync(user, "1234@Abcd");
                    result.BusinessObject = user;
                    if (createResult.Succeeded)
                    {
                        student.User = user;
                        await _studentRepository.AddOrEditAndSaveAsyn(student);
                        
                        // 提取用户缺省归属角色组（班级角色组）
                        if (student.GradeAndClass != null)
                        {
                            var gradeAndClass = await _gradeAndClassRepository.GetSingleAsyn(student.GradeAndClass.Id, x => x.ApplicationRole);
                            if (gradeAndClass.ApplicationRole != null)
                            {
                                await _userManager.AddToRoleAsync(student.User, gradeAndClass.ApplicationRole.Name);

                                // 检查用户关联角色组类型声明
                                var userClaims = await _userManager.GetClaimsAsync(student.User);
                                var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == gradeAndClass.ApplicationRole.ApplicationRoleType.ToString());
                                // 添加角色组类型声明
                                if (userRoleTypeClaim == null)
                                {
                                    // 为用户创建一个归属系统角色类型的身份声明
                                    var claim = new Claim(ClaimTypes.Name, gradeAndClass.ApplicationRole.ApplicationRoleType.ToString());
                                    await _userManager.AddClaimAsync(student.User, claim);
                                }
                            }
                        }

                        // 加入缺省的角色组
                        var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.ApplicationRoleType == ApplicationRoleTypeEnum.适用于普通注册用户);
                        if (defaultRole != null)
                            await _userManager.AddToRoleAsync(student.User, defaultRole.Name);
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("提交的用户名已经存在，请检查相关的数据！");
                    return result;
                }
            }
            else
            {
                student.User.UserName = student.EmployeeCode;
                student.User.ChineseFullName = student.Name;
                student.User.Email = student.Email;
                student.User.MobileNumber = student.Mobile;
                if (!String.IsNullOrEmpty(userName))
                    student.User.UserName = userName;

                var editResult = await _userManager.UpdateAsync(student.User);
                result.BusinessObject = student.User;

                if (editResult.Succeeded)
                {
                    await _studentRepository.AddOrEditAndSaveAsyn(student);
                    // 提取用户缺省归属角色组（班级角色组）
                    if (student.GradeAndClass != null)
                    {
                        var gradeAndClass = await _gradeAndClassRepository.GetSingleAsyn(student.GradeAndClass.Id, x => x.ApplicationRole);
                        if (gradeAndClass.ApplicationRole != null)
                        {
                            await _userManager.AddToRoleAsync(student.User, gradeAndClass.ApplicationRole.Name);
                            
                            // 检查用户关联角色组类型声明
                            var userClaims = await _userManager.GetClaimsAsync(student.User);
                            var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == gradeAndClass.ApplicationRole.ApplicationRoleType.ToString());
                            // 添加角色组类型声明
                            if (userRoleTypeClaim == null)
                            {
                                // 为用户创建一个归属系统角色类型的身份声明
                                var claim = new Claim(ClaimTypes.Name, gradeAndClass.ApplicationRole.ApplicationRoleType.ToString());
                                await _userManager.AddClaimAsync(student.User, claim);
                            }
                        }
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("更新用户组数据保存出现异常，请联系相关人员处理！");
                    foreach (var err in editResult.Errors)
                        result.Messages.Add(err.Description);

                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据员工视图模型创建或者维护与之匹配的用户数据，并进行持久化处理
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<EntityProcessResult> CreateOrEditByEmployee(Employee employee,string userName = null)
        {
            // 初始化处理结果
            var result = new EntityProcessResult() { Succeeded = true, Messages = new List<string> { } };
            if (employee.User == null)
            {
                // 检查重名
                var newUserName = employee.EmployeeCode;  // 待检查重名，缺省直接使用员工工号
                if (!String.IsNullOrEmpty(userName))
                    newUserName = userName;

                var isUniquelyForName = await IsUniquelyForUserName(newUserName);   // 是否是唯一名称
                if (isUniquelyForName)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = employee.EmployeeCode,
                        ChineseFullName = employee.Name,
                        Email = employee.Email,
                        MobileNumber = employee.Mobile
                    };

                    user.UserName = newUserName;
                    // 使用缺省密码创建用户
                    var createResult = await _userManager.CreateAsync(user, "1234@Abcd");
                    result.BusinessObject = user;

                    if (createResult.Succeeded)
                    {
                        // 更新和持久化员工与用户关联关系
                        employee.User = user;
                        await _employeeRepository.AddOrEditAndSaveAsyn(employee);  

                        // 提取用户缺省归属角色组（部门角色组）
                        if (employee.Department != null)
                        {
                            var department = await _departmentRepository.GetSingleAsyn(employee.Department.Id, x => x.ApplicationRole);
                            if (department.ApplicationRole != null)
                            {
                                // 将当前创建的用户添加到对应部门的角色组中
                                await _userManager.AddToRoleAsync(employee.User, department.ApplicationRole.Name);
                                
                                // 检查用户关联角色组类型声明
                                var userClaims = await _userManager.GetClaimsAsync(employee.User);
                                var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == department.ApplicationRole.ApplicationRoleType.ToString());
                                // 添加角色组类型声明
                                if (userRoleTypeClaim == null)
                                {
                                    // 为用户创建一个归属系统角色类型的身份声明
                                    var claim = new Claim(ClaimTypes.Name, department.ApplicationRole.ApplicationRoleType.ToString());
                                    await _userManager.AddClaimAsync(employee.User, claim);
                                }
                            }
                        }
                        
                        // 再加入缺省的用户组
                        var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.ApplicationRoleType == ApplicationRoleTypeEnum.适用于普通注册用户);
                        if (defaultRole != null)
                            await _userManager.AddToRoleAsync(employee.User, defaultRole.Name);
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("提交的用户名已经存在，请检查相关的数据！");
                    return result;
                }
            }
            else
            {
                employee.User.UserName = employee.EmployeeCode;
                employee.User.ChineseFullName = employee.Name;
                employee.User.Email = employee.Email;
                employee.User.MobileNumber = employee.Mobile;
                if (!String.IsNullOrEmpty(userName))
                    employee.User.UserName = userName;

                // 更新用户数据
                var editResult = await _userManager.UpdateAsync(employee.User);
                result.BusinessObject = employee.User;

                if (editResult.Succeeded)
                {
                    // 持久化员工与用户的关联关系
                    await _employeeRepository.AddOrEditAndSaveAsyn(employee);

                    // 提取用户缺省归属角色组（部门角色组）
                    if (employee.Department != null)
                    {
                        var department = await _departmentRepository.GetSingleAsyn(employee.Department.Id, x => x.ApplicationRole);
                        if (department.ApplicationRole != null)
                        {
                            await _userManager.AddToRoleAsync(employee.User, department.ApplicationRole.Name);

                            // 检查用户关联角色组类型声明
                            var userClaims = await _userManager.GetClaimsAsync(employee.User);
                            var userRoleTypeClaim = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == department.ApplicationRole.ApplicationRoleType.ToString());
                            // 添加角色组类型声明
                            if (userRoleTypeClaim == null)
                            {
                                // 为用户创建一个归属系统角色类型的身份声明
                                var claim = new Claim(ClaimTypes.Name, department.ApplicationRole.ApplicationRoleType.ToString());
                                await _userManager.AddClaimAsync(employee.User, claim);
                            }
                        }
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("更新用户组数据保存出现异常，请联系相关人员处理！");
                    foreach (var err in editResult.Errors)
                        result.Messages.Add(err.Description);

                    return result;
                }
            }
            return result;
        }

        public async Task<bool> ResetApplicationUserPassword(ApplicationUserPasswordResetVM resetVM)
        {
            var user = await _userManager.FindByIdAsync(resetVM.UserId.ToString());
            if (user != null)
            {
                string tokenString = await _userManager.GeneratePasswordResetTokenAsync(user);
                var updateResult = await _userManager.ResetPasswordAsync(user,tokenString, resetVM.Password);
                if (updateResult.Succeeded)
                {
                    resetVM.ResetStatus = "重置密码成功";
                }
                else
                {
                    resetVM.ResetStatus="重置密码失败";
                }

            }

            return true;
        }

        /// <summary>
        /// 设置与传入的视图模型相关的关联元素的集合值
        /// </summary>
        /// <param name="boVM"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public async Task SetTypeItems(ApplicationUserVM boVM)
        {
            var user = await _userManager.FindByIdAsync(boVM.Id.ToString());

            #region 设置头像相关的数据
            Func<Guid, string> avatarPath = (itemId) =>
                {
                    var path = "";
                    var imageBo = _businessImageRepository.GetSingleBy(x => x.RelevanceObjectID == itemId);
                    if (imageBo != null)
                        path = imageBo.UploadPath;
                    return path;
                };
            if (user != null)
                boVM.AvatarPath = avatarPath(user.Id);
            else
                boVM.AvatarPath = "";
            #endregion

            #region 设置系统角色相关额数据
            boVM.ApplicationRoleItemIdCollection = new List<string>();
            boVM.ApplicationRoleItemCollection = new List<PlainFacadeItem>();
            if (user != null)
            {
                var userInRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in _roleManager.Roles)
                {
                    var item = new PlainFacadeItem() { ID = role.Id.ToString(), Name = role.Name, IsActive = false };
                    if (userInRoles.Contains(role.Name))
                    {
                        item.IsActive = true;
                        boVM.ApplicationRoleItemIdCollection.Add(role.Id.ToString());
                    }

                    boVM.ApplicationRoleItemCollection.Add(item);
                }
            }
            else
            {
                foreach (var role in _roleManager.Roles)
                {
                    var item = new PlainFacadeItem() { ID = role.Id.ToString(), Name = role.Name, IsActive = false };
                    boVM.ApplicationRoleItemCollection.Add(item);
                }
            }
            #endregion

            #region 设置人员相关的数据
            var employeeVM = await GetEmployeeVMByUserId(boVM.Id);
            if (employeeVM != null)
            {
                boVM.PersonId = employeeVM.Id;
                boVM.PersonName = employeeVM.Name;
                boVM.PersonCredentialsCode = employeeVM.CredentialsCode;
                boVM.PersonAddress = employeeVM.Address;
                boVM.PersonOrganizationName = employeeVM.ParentDepartmentName;
            }
            var studentVM = await GetStudentVMByUserId(boVM.Id);
            if (studentVM != null)
            {
                boVM.PersonId               = studentVM.Id;
                boVM.PersonName             = studentVM.Name;
                boVM.PersonCredentialsCode  = studentVM.CredentialsCode;
                boVM.PersonAddress          = studentVM.Address;
                boVM.PersonOrganizationName = studentVM.GradeAndClassName;
            }
            #endregion
        }

        public async Task SetApplicationRoleItemCollection(ApplicationUserVM boVM)
        {
            var user = await _userManager.FindByIdAsync(boVM.Id.ToString());

            var resultItems = new List<PlainFacadeItem>();
            if (user != null)
            {
                var userInRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in _roleManager.Roles)
                {
                    var item = new PlainFacadeItem() { ID = role.Id.ToString(), Name = role.Name, IsActive = false };
                    if (userInRoles.Contains(role.Name))
                    {
                        item.IsActive = true;
                    }

                    resultItems.Add(item);
                }
            }
            else
            {
                foreach (var role in _roleManager.Roles)
                {
                    var item = new PlainFacadeItem() { ID = role.Id.ToString(), Name = role.Name, IsActive = false };
                    resultItems.Add(item);
                }
            }

            boVM.ApplicationRoleItemCollection = resultItems;
            if (boVM.ApplicationRoleItemIdCollection != null)
            {
                foreach (var item in boVM.ApplicationRoleItemCollection)
                {
                    if (boVM.ApplicationRoleItemIdCollection.Contains(item.ID))
                        item.IsActive = true;
                }
            }
        }

        /// <summary>
        /// 提取全部的角色视图集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRoleVM>> GetRoleVMCollection()
        {
            var boCollection = _roleManager.Roles;
            var boVMCollection = new List<ApplicationRoleVM>();

            var counter = 0;
            foreach (var item in boCollection)
            {
                var boVM = await _roleService.GetVM(item.Id);
                boVM.OrderNumber = (++counter).ToString();
                var users = await _userManager.GetUsersInRoleAsync(item.Name);
                boVM.UserAmount = users.Count();
                boVMCollection.Add(boVM);
            }

            return boVMCollection;
        }

        /// <summary>
        /// 提取所有的角色组，供前端使用处理
        /// </summary>
        /// <returns></returns>
        public List<PlainFacadeItem> GetApplicationRoleItemCollection()
        {
            var items = new List<PlainFacadeItem>();
            foreach (var role in _roleManager.Roles)
            {
                var item = new PlainFacadeItem() { ID = role.Id.ToString(), Name = role.Name, IsActive = false };
                items.Add(item);
            }
            return items;
        }

        #region 业务对象和视图模型映射处理
        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(ApplicationUser bo, ApplicationUserVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.UserName = bo.UserName;
            boVM.MobileNumber = bo.MobileNumber;
            boVM.Email = bo.Email;
            boVM.Name = bo.ChineseFullName;
            boVM.LockoutEnabled = bo.LockoutEnabled;
        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _VMMapToBo(ApplicationUser bo, ApplicationUserVM boVM)
        {
            bo.ChineseFullName = boVM.Name;
            bo.UserName        = boVM.UserName;
            bo.Email           = boVM.Email;
            bo.MobileNumber    = boVM.MobileNumber;
        }
        #endregion
    }
}
