using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.UserAndRole
{
    public class ApplicationRoleVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        public ApplicationRoleVMService(
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
        }

        /// <summary>
        /// 返回一个空的视图模型
        /// </summary>
        /// <returns></returns>
        public ApplicationRoleVM GetVM()
        {
            return new ApplicationRoleVM();
        }

        /// <summary>
        /// 根据角色组对象 Id 返回一个角色组实体视图模型，如果传入的 Id 找不到对应的角色对象，则返回一个新的视图模型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationRoleVM> GetVM(Guid id)
        {
            var boVM = new ApplicationRoleVM();
            var bo = await _roleManager.FindByIdAsync(id.ToString());

            if (bo == null)
            {
                bo = new ApplicationRole();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            _BoMapToVM(bo, boVM);

            return boVM;
        }

        /// <summary>
        /// 返回所有角色组对象的视图集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationRoleVM>> GetboVMCollectionAsyn()
        {
            var boCollection = _roleManager.Roles;
            var boVMCollection = new List<ApplicationRoleVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x=>x.Name))
            {
                var boVM = await GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 根据关键词，返回所有满足条件的角色组对象的视图集合
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<ApplicationRoleVM>> GetboVMCollectionAsyn(string keyword)
        {
            Expression<Func<ApplicationRole, bool>> expression =
                x =>
                x.Name.Contains(keyword) ||
                x.Description.Contains(keyword);

            var boCollection = _roleManager.Roles.Where(expression);
            var boVMCollection = new List<ApplicationRoleVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = await GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 检查角色名是否重复
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsUniquelyForName(string name)
        {
            var bo = await _roleManager.FindByNameAsync(name);
            return (bo == null) ? true : false;
        }

        /// <summary>
        /// 保存角色组数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        public async Task<bool> SaveBo(ApplicationRoleVM boVM)
        {
            var bo = await _roleManager.FindByIdAsync(boVM.Id.ToString());
            if (bo != null)
            {
                _VMMapToBo(bo,boVM);
                var result = await _roleManager.UpdateAsync(bo);
                boVM.ApplicationRoleTypeItemCollection = _GetByEnum(boVM.ApplicationRoleType);

                if (result.Succeeded)
                    return true;
                else
                    return false;
            }
            else
            {
                bo = new ApplicationRole();
                _VMMapToBo(bo, boVM);
                var result = await _roleManager.CreateAsync(bo);
                boVM.ApplicationRoleTypeItemCollection = _GetByEnum(boVM.ApplicationRoleType);

                if (result.Succeeded)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 删除角色组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var bo = await _roleManager.FindByIdAsync(id.ToString());
            var status = new DeleteStatusModel() { DeleteSatus = true, Message = "数据删除成功" };
            try
            {
                await _roleManager.DeleteAsync(bo);
            }
            catch
            {
                status.DeleteSatus = false;
                status.Message = "删除操作出现意外，主要原因是关联数据没有处理干净活者是其他原因。";
            }
            return status;
        }

        /// <summary>
        /// 设置角色组与关联多项相关的视图模型属性
        /// </summary>
        /// <param name="boVM"></param>
        public void SetTypeItems(ApplicationRoleVM boVM)
        {
            boVM.ApplicationRoleTypeItemCollection = _GetByEnum(boVM.ApplicationRoleType);
        }

        /// <summary>
        /// 根据部门对象创建或者修改相应的角色组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EntityProcessResult> CreateOrEditSaveByDepeartment(Department department,string roleName = null) 
        {
            var result = new EntityProcessResult() { Succeeded=true, Messages = new List<string>{}};
            if (department.ApplicationRole == null)
            {
                // 检查重名
                var checkName = department.Name;
                if (!String.IsNullOrEmpty(roleName))
                    checkName = roleName;

                var isUniquelyForName = await IsUniquelyForName(checkName);
                if (isUniquelyForName)
                {
                    // 新建用户组
                    var role = new ApplicationRole()
                    {
                        Name                = department.Name,
                        DisplayName         = department.Name,
                        Description         = department.Description,
                        SortCode            = "D_" + department.SortCode,
                        ApplicationRoleType = ApplicationRoleTypeEnum.适用于教师
                    };

                    var createResult = await _roleManager.CreateAsync(role);
                    result.BusinessObject = role;

                    // 根据创建的结果返回处理状态
                    if (createResult.Succeeded)
                    {
                        department.ApplicationRole = role;
                        await _departmentRepository.AddOrEditAndSaveAsyn(department);
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Messages.Add("新建用户组数据保存出现异常，请联系相关人员处理！");
                        foreach (var err in createResult.Errors)
                            result.Messages.Add(err.Description);

                        return result;
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("提交的部门名称已经存在同名的用户组，请检查相关的数据！");
                    return result;
                }
            }
            else
            {
                department.ApplicationRole.Name = department.Name;
                department.ApplicationRole.DisplayName = department.Name;
                department.ApplicationRole.Description = department.Description;
                department.ApplicationRole.SortCode = "D_" + department.SortCode;
                if (!String.IsNullOrEmpty(roleName))
                    department.ApplicationRole.Name = roleName;

                var editResult = await _roleManager.UpdateAsync(department.ApplicationRole);
                result.BusinessObject = department.ApplicationRole;

                if (editResult.Succeeded)
                {
                    await _departmentRepository.AddOrEditAndSaveAsyn(department);
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
        /// 根据班级对象创建或者修改响应的角色组
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public async Task<EntityProcessResult> CreateOrEditSaveByGradeAndClass(GradeAndClass gradeAndClass,string roleName = null)
        {
            var result = new EntityProcessResult() { Succeeded = true, Messages = new List<string> { } };
            if (gradeAndClass.ApplicationRole == null)
            {
                // 检查重名
                var checkName = gradeAndClass.Name;
                if (!String.IsNullOrEmpty(roleName))
                    checkName = roleName;

                // 检查重名
                var isUniquelyForName = await IsUniquelyForName(checkName);
                if (isUniquelyForName)
                {
                    // 新建用户组
                    var role = new ApplicationRole()
                    {
                        Name                = gradeAndClass.Name,
                        DisplayName         = gradeAndClass.Name,
                        Description         = gradeAndClass.Description,
                        SortCode            = "G_" + gradeAndClass.SortCode,
                        ApplicationRoleType = ApplicationRoleTypeEnum.适用于教学班级学员
                    };
                    var createResult = await _roleManager.CreateAsync(role);

                    // 根据创建的结果返回处理状态（创建失败）
                    if (createResult.Succeeded)
                    {
                        gradeAndClass.ApplicationRole = role;
                        await _gradeAndClassRepository.AddOrEditAndSaveAsyn(gradeAndClass);
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Messages.Add("新建用户组数据保存出现异常，请联系相关人员处理！");
                        foreach (var err in createResult.Errors)
                            result.Messages.Add(err.Description);

                        return result;
                    }
                }
                else
                {
                    result.Succeeded = false;
                    result.Messages.Add("提交的部门名称已经存在同名的用户组，请检查相关的数据！");
                    return result;
                }
            }
            else
            {
                gradeAndClass.ApplicationRole.Name        = gradeAndClass.Name;
                gradeAndClass.ApplicationRole.DisplayName = gradeAndClass.Name;
                gradeAndClass.ApplicationRole.Description = gradeAndClass.Description;
                gradeAndClass.ApplicationRole.SortCode    = "G_" + gradeAndClass.SortCode;

                if (!String.IsNullOrEmpty(roleName))
                    gradeAndClass.ApplicationRole.Name = roleName;

                var editResult = await _roleManager.UpdateAsync(gradeAndClass.ApplicationRole);
                result.BusinessObject = gradeAndClass.ApplicationRole;

                if (editResult.Succeeded)
                {
                    await _gradeAndClassRepository.AddOrEditAndSaveAsyn(gradeAndClass);
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


        #region 映射处理
        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(ApplicationRole bo, ApplicationRoleVM boVM)
        {
            boVM.Id                                = bo.Id;
            boVM.Name                              = bo.Name;
            boVM.Description                       = bo.Description;
            boVM.SortCode                          = bo.SortCode;
            boVM.ApplicationRoleType               = bo.ApplicationRoleType;
            boVM.ApplicationRoleTypeName           = bo.ApplicationRoleType.ToString();
            boVM.ApplicationRoleTypeItemCollection = _GetByEnum(bo.ApplicationRoleType);
        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _VMMapToBo(ApplicationRole bo, ApplicationRoleVM boVM)
        {
            bo.Id                  = boVM.Id;
            bo.Name                = boVM.Name;
            bo.Description         = boVM.Description;
            bo.SortCode            = boVM.SortCode;
            bo.ApplicationRoleType = boVM.ApplicationRoleType;
        }

        #endregion

        /// <summary>
        /// 直接将泛型类型中指定的枚举类型转换为 PlainFacdeItem 集合
        /// </summary>
        /// <returns></returns>
        private List<PlainFacadeItem> _GetByEnum(Object enumObject)
        {
            var enumItems = Enum.GetValues(enumObject.GetType());
            var items = new List<PlainFacadeItem>();
            foreach (var eItem in enumItems)
            {
                var item = new PlainFacadeItem()
                {
                    ID = ((int)eItem).ToString(),
                    Name = eItem.ToString(),
                };
                items.Add(item);
            }
            return items;
        }

    }
}
