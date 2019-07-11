using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using ELearning.ViewModels.UserAndRole;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.Organization
{
    public class EmployeeVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Employee> _boRepository;
        private readonly IEntityRepository<Student> _studentpository;
        private readonly IEntityRepository<GradeAndClass> _gradeRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<BusinessImage> _imageRepository;

        public EmployeeVMService(
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
        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public EmployeeVM GetVM()
        {
            return new EmployeeVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public EmployeeVM GetVM(Guid boId)
        {
            var boVM = new EmployeeVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, x => x.Department, z => z.User);
            if (bo == null)
            {
                bo = new Employee();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            SetTypeItems(boVM);

            return boVM;

        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<EmployeeVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllAsyn();
            var boVMCollection = new List<EmployeeVM>();
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
        /// 根据列表分页器的参数
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <param name="boService"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<List<EmployeeVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
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
            Expression<Func<Employee, bool>> predicateExpession = ExpressionFactoryMethod.GetConditionExpression<Employee>(keyword);


            // 获取排序属性所需要的表达式
            var sortExpession = ExpressionFactoryMethod.GetPropertyExpression<Employee, object>(listPageParameter.SortProperty);

            // 获取数据集合
            var tempCollection = _boRepository.GetAllIncluding(x => x.Department, y => y.User).Where(predicateExpession);

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            // 按照类型获取业务对象数据
            if (!String.IsNullOrEmpty(typeID))
            {
                tempCollection = tempCollection.Where(y => y.Department.Id == Guid.Parse(typeID));
                typeName = _departmentRepository.GetSingle(Guid.Parse(typeID)).Name;
            }

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var boCollection = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            var boVMCollection = new List<EmployeeVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<Employee>(boCollection, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return boVMCollection;
        }

        /// <summary>
        /// 将视图对象的数据，映射为业务对象数据，进行持久化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <param name="gradeService"></param>
        /// <returns></returns>
        public async Task<bool> SaveBo(EmployeeVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id);
            if (bo == null)
                bo = new Employee();

            _VMMapToBo(bo, boVM);

            if (!String.IsNullOrEmpty(boVM.ParentDepartmentId))
                bo.Department = _departmentRepository.GetSingle(Guid.Parse(boVM.ParentDepartmentId));

            var x = await _boRepository.AddOrEditAndSaveAsyn(bo);
            return x;

        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }

        /// <summary>
        /// 根据传入的人员 Id 和关联的用户名，创建或者编辑用户数据
        /// </summary>
        /// <param name="Id">员工 Id</param>
        /// <param name="userName">待处理的用户名</param>
        public async Task<EntityProcessResult> CreateOrEditRelevaneceUser(Guid id, string userName)
        {
            // 获取员工对象
            var bo = await _boRepository.GetSingleAsyn(id, x => x.User);

            var userService = new ApplicationUserVMService(_userManager, _roleManager,_boRepository,_studentpository,_imageRepository,_departmentRepository,_jobTitleRepository,_gradeRepository );
            // 创建或编辑员工用户数据
            var result = await userService.CreateOrEditByEmployee(bo, userName);
            return result;

        }

        /// <summary>
        /// 设置用于前端页面需要的下拉数据选项
        /// </summary>
        public void SetTypeItems(EmployeeVM boVM)
        {
            boVM.ParentDepartmentItemCollection = SelfReferentialItemFactory<Department>.GetCollection(_departmentRepository, true);
            boVM.JobTitleItemCollection = PlainFacadeItemFactory<JobTitle>.Get(_jobTitleRepository);
        }

        /// <summary>
        /// 获取导航树所需要的节点集合
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetNavigatorItems()
        {
            return TreeViewFactory.GetTreeNodes<Department>(_departmentRepository);
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(Employee bo, EmployeeVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;
            boVM.EmployeeCode = bo.EmployeeCode;
            boVM.TelephoneNumber = bo.TelephoneNumber;
            boVM.MobileNumber = bo.Mobile;
            boVM.Email = bo.Email;
            boVM.Birthday = bo.Birthday.ToString("yyyy-MM-dd");
            boVM.CredentialsCode = bo.CredentialsCode;
            boVM.Address = bo.Address;
            boVM.CreateDateTime = bo.CreateDateTime.ToString("yyyy-MM-dd");
            boVM.ExpiredDateTime = bo.ExpiredDateTime.ToString("yyyy-MM-dd");
            boVM.AvatarPath = bo.AvatarPath;

            if (bo.Department != null)
            {
                boVM.ParentDepartmentId = bo.Department.Id.ToString();
                boVM.ParentDepartmentName = bo.Department.Name;
            }

            if (bo.JobTitle != null)
            {
                boVM.JobTitleId = bo.JobTitle.Id.ToString();
                boVM.JobTitleName = bo.JobTitle.Name;
            }

            if (bo.User != null)
            {
                boVM.UserId = bo.User.Id;
                boVM.UserName = bo.User.UserName;
            }

        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _VMMapToBo(Employee bo, EmployeeVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.Description = boVM.Description;
            bo.EmployeeCode = boVM.EmployeeCode;
            bo.TelephoneNumber = boVM.TelephoneNumber;
            bo.Mobile = boVM.MobileNumber;
            bo.Email = boVM.Email;
            bo.Birthday = DateTime.Parse(boVM.Birthday);
            bo.CredentialsCode = boVM.CredentialsCode;
            bo.Address = boVM.Address;
            bo.CreateDateTime = DateTime.Parse(boVM.CreateDateTime);
            bo.ExpiredDateTime = DateTime.Parse(boVM.ExpiredDateTime);
        }


    }
}
