using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.Organization
{
    public class DepartmentVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Department> _boRepository;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Organ> _orgRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        public DepartmentVMService(
            IEntityRepository<Department> repository,
            IEntityRepository<Employee> employeeRepository,
            IEntityRepository<Organ> orgRepository, 
            IEntityRepository<GradeAndClass> gradeAndClassRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository = repository;
            _employeeRepository = employeeRepository;
            _orgRepository = orgRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public DepartmentVM GetVM()
        {
            return new DepartmentVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public DepartmentVM GetVM(Guid id)
        {
            var boVM = new DepartmentVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(id, x => x.ParentDepartment, y=>y.Organization, z => z.ApplicationRole);
            if (bo == null)
            {
                bo = new Department();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            SetRelevanceItems(boVM);

            return boVM;
        }

        /// <summary>
        /// 根据业务实体对象返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public async Task<DepartmentVM> GetVM(Department bo)
        {
            var boVM = new DepartmentVM();
            _BoMapToVM(bo, boVM);
            var collection = await _employeeRepository.GetAllAsyn(x=>x.Department.Id==bo.Id);
            boVM.PersonAmount = collection.Count();
            return boVM;
        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型，返回的视图模型根据要求做了层次化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<DepartmentVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllIncludingAsyn(role=>role.ApplicationRole,org=>org.Organization);
            var boVMCollection = new List<DepartmentVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = await GetVM(bo);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            #region 为部门数据呈现处理名称缩进
            var deptItems = SelfReferentialItemFactory<Department>.GetCollection(boCollection.ToList(), true);
            foreach (var item in deptItems)
            {
                var dID = Guid.Parse(item.ID);
                var dept = boVMCollection.FirstOrDefault(x => x.Id == dID);
                dept.Name = item.DisplayName;
            }
            #endregion

            return boVMCollection;
        }

        /// <summary>
        /// 将视图对象的数据，映射为业务对象数据，进行持久化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <param name="gradeService"></param>
        /// <returns></returns>
        public async Task<bool> SaveBo(DepartmentVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id, r => r.ApplicationRole);
            if (bo == null)
                bo = new Department();

            await _VMMapToBo(bo, boVM);

            // 处理创建用户组
            if (boVM.IsCreateRoleAuto)
            {
                var roleFactory = new ApplicationRoleVMService(_userManager,_roleManager,_boRepository,_gradeAndClassRepository);
                var result = await roleFactory.CreateOrEditSaveByDepeartment(bo);
            }

            var saveResult = await _boRepository.AddOrEditAndSaveAsyn(bo);
            if (saveResult)
            {
            }
            return saveResult;
        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }

        /// <summary>
        /// 根据传入的部门 Id 和关联的角色组的名称，创建或者用户组
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="roleName"></param>
        public async Task<EntityProcessResult> CreateOrEditRelevaneceRole(Guid id,string roleName)
        {
            var bo = await _boRepository.GetSingleAsyn(id, x => x.ApplicationRole);
            var roleFactory = new ApplicationRoleVMService(_userManager, _roleManager, _boRepository, _gradeAndClassRepository);

            var result = await roleFactory.CreateOrEditSaveByDepeartment(bo,roleName);
            return result;

        }

        /// <summary>
        /// 设置用于前端页面需要的关联数据选项
        /// </summary>
        public void SetRelevanceItems(DepartmentVM boVM)
        {
            boVM.DepartmentTypeItemCollection = PlainFacadeItemFactory<Department>.GetByEnum(boVM.DepartmentType);
            boVM.ParentDepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(_boRepository, true);
            boVM.OrganizationItemCollection = PlainFacadeItemFactory<Organ>.Get(_orgRepository);
            boVM.ApplicationRoleItemCollection = _GetApplicationRoleItemCollection(_roleManager.Roles.ToList());
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(Department bo, DepartmentVM boVM)
        {
            boVM.Id          = bo.Id;
            boVM.Name        = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode    = bo.SortCode;

            boVM.DepartmentType = bo.DepartmentType;
            boVM.DepartmentTypeName = bo.DepartmentType.ToString();

            if (bo.ParentDepartment != null)
            {
                boVM.ParentDepartmentId = bo.ParentDepartment.Id.ToString();
                boVM.ParentDepartmentName = bo.ParentDepartment.Name;
            }

            if (bo.Organization != null)
            {
                boVM.OrganizationId = bo.Organization.Id.ToString();
                boVM.OrganizationName = bo.Organization.Name;
            }

            if (bo.ApplicationRole != null)
            {
                boVM.ApplicationRoleId = bo.ApplicationRole.Id.ToString();
                boVM.ApplicationRoleName = bo.ApplicationRole.Name;
            }
        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private async Task _VMMapToBo(Department bo, DepartmentVM boVM)
        {
            bo.Id          = boVM.Id;
            bo.Name        = boVM.Name;
            bo.Description = boVM.Description;
            bo.SortCode    = boVM.SortCode;

            if (!String.IsNullOrEmpty(boVM.ParentDepartmentId))
                bo.ParentDepartment = await _boRepository.GetSingleAsyn(Guid.Parse(boVM.ParentDepartmentId));
            else
            {
                bo.ParentDepartment = bo;
            }
            if (!String.IsNullOrEmpty(boVM.OrganizationId))
                bo.Organization = await _orgRepository.GetSingleAsyn(Guid.Parse(boVM.OrganizationId));
            if (!String.IsNullOrEmpty(boVM.ApplicationRoleId))
                bo.ApplicationRole = await _roleManager.FindByIdAsync(boVM.ApplicationRoleId.ToString());

        }

        private List<PlainFacadeItem> _GetApplicationRoleItemCollection(List<ApplicationRole> applicationRoles)
        {
            var items = new List<PlainFacadeItem>();
            foreach (var pItem in applicationRoles)
            {
                var item = new PlainFacadeItem()
                {
                    ID = pItem.Id.ToString(),
                    Name = pItem.Name,
                    DisplayName = pItem.Name,
                    Description = pItem.Description,
                    SortCode = pItem.SortCode
                };
                items.Add(item);
            }
            return items;
        }

    }
}
