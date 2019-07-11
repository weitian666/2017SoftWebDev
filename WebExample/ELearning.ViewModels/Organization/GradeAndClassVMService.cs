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
    public class GradeAndClassVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<Organ> _orgRepository;
        private readonly IEntityRepository<GradeAndClass> _boRepository;

        public GradeAndClassVMService(
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
        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public GradeAndClassVM GetVM()
        {
            return new GradeAndClassVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public GradeAndClassVM GetVM(Guid id)
        {
            var boVM = new GradeAndClassVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(id, x => x.ParentDepartment, z => z.ApplicationRole);
            if (bo == null)
            {
                bo = new GradeAndClass();
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
        public async Task<GradeAndClassVM> GetVM(GradeAndClass bo)
        {
            var boVM = new GradeAndClassVM();
            _BoMapToVM(bo, boVM);
            var collection = await _studentRepository.GetAllAsyn(x => x.GradeAndClass.Id == bo.Id);
            boVM.PersonAmount = collection.Count();
            return boVM;
        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型，返回的视图模型根据要求做了层次化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<GradeAndClassVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllIncludingAsyn(role => role.ApplicationRole, dept => dept.ParentDepartment);
            var boVMCollection = new List<GradeAndClassVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = await GetVM(bo);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            #region 为部门数据呈现处理名称缩进
            var deptItems = SelfReferentialItemFactory<GradeAndClass>.GetCollection(boCollection.ToList(), true);
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
        public async Task<bool> SaveBo(GradeAndClassVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id, r => r.ApplicationRole);
            if (bo == null)
                bo = new GradeAndClass();

            await _VMMapToBo(bo, boVM);

            // 处理创建用户组
            if (boVM.IsCreateRoleAuto)
            {
                var roleFactory = new ApplicationRoleVMService(_userManager, _roleManager, _departmentRepository, _boRepository);
                var result = await roleFactory.CreateOrEditSaveByGradeAndClass(bo);
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
        public async Task<EntityProcessResult> CreateOrEditRelevaneceRole(Guid id, string roleName)
        {
            var bo = await _boRepository.GetSingleAsyn(id, x => x.ApplicationRole);
            var roleFactory = new ApplicationRoleVMService(_userManager, _roleManager, _departmentRepository, _boRepository);

            var result = await roleFactory.CreateOrEditSaveByGradeAndClass(bo, roleName);
            return result;

        }

        /// <summary>
        /// 设置用于前端页面需要的关联数据选项
        /// </summary>
        public void SetRelevanceItems(GradeAndClassVM boVM)
        {
            boVM.ParentDepartmentItemCollection = SelfReferentialItemFactory<GradeAndClass>.GetCollection(_boRepository, true);
            boVM.ApplicationRoleItemCollection = _GetApplicationRoleItemCollection(_roleManager.Roles.ToList());
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(GradeAndClass bo, GradeAndClassVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;

            if (bo.ParentDepartment != null)
            {
                boVM.ParentDepartmentId = bo.ParentDepartment.Id.ToString();
                boVM.ParentDepartmentName = bo.ParentDepartment.Name;
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
        private async Task _VMMapToBo(GradeAndClass bo, GradeAndClassVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.Description = boVM.Description;
            bo.SortCode = boVM.SortCode;

            if (!String.IsNullOrEmpty(boVM.ParentDepartmentId))
                bo.ParentDepartment = await _boRepository.GetSingleAsyn(Guid.Parse(boVM.ParentDepartmentId));
            else
            {
                bo.ParentDepartment = bo;
            }
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
