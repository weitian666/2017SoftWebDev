using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities;
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
    public class StudentVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Student> _boRepository;
        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<JobTitle> _jobTitleRepository;
        private readonly IEntityRepository<BusinessImage> _imageRepository;

        public StudentVMService(
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
        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public StudentVM GetVM()
        {
            return new StudentVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public StudentVM GetVM(Guid boId)
        {
            var boVM = new StudentVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, x => x.GradeAndClass, z => z.User);
            if (bo == null)
            {
                bo = new Student();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo,boVM);

            // 设置供前端下拉选项所需要的数据集合
            SetTypeItems(boVM);

            return boVM;

        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<StudentVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllAsyn();
            var boVMCollection = new List<StudentVM>();
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
        public async Task<List<StudentVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
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
            Expression<Func<Student, bool>> predicateExpession = ExpressionFactoryMethod.GetConditionExpression<Student>(keyword);


            // 获取排序属性所需要的表达式
            var sortExpession = ExpressionFactoryMethod.GetPropertyExpression<Student, object>(listPageParameter.SortProperty);

            // 获取数据集合
            var tempCollection = _boRepository.GetAllIncluding(x => x.GradeAndClass, y => y.User).Where(predicateExpession);

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            // 按照类型获取业务对象数据
            if (!String.IsNullOrEmpty(typeID))
            {
                tempCollection = tempCollection.Where(y => y.GradeAndClass.Id == Guid.Parse(typeID));
                typeName = _gradeRepository.GetSingle(Guid.Parse(typeID)).Name;
            }

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var boCollection = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            var boVMCollection = new List<StudentVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<Student>(boCollection, 10, pageIndex);
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
        public async Task<bool> SaveBo(StudentVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id);
            if (bo == null)
                bo = new Student();

            _VMMapToBo(bo,boVM);

            if (!String.IsNullOrEmpty(boVM.GradeAndClassId))
                bo.GradeAndClass = _gradeRepository.GetSingle(Guid.Parse(boVM.GradeAndClassId));

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
        /// <param name="Id"></param>
        /// <param name="roleName"></param>
        public async Task<EntityProcessResult> CreateOrEditRelevaneceUser(Guid id, string roleName)
        {
            var bo = await _boRepository.GetSingleAsyn(id, x => x.User);
            var userService = new ApplicationUserVMService(_userManager,_roleManager,_employeeRepository,_boRepository,_imageRepository,_departmentRepository,_jobTitleRepository,_gradeRepository);

            var result = await userService.CreateOrEditByStudent(bo, roleName);
            return result;

        }

        /// <summary>
        /// 设置用于前端页面需要的下拉数据选项
        /// </summary>
        public void SetTypeItems(StudentVM boVM)
        {
            boVM.GradeAndClassItemCollection = SelfReferentialItemFactory<GradeAndClass>.GetCollection(_gradeRepository, true);
        }

        /// <summary>
        /// 获取导航树所需要的节点集合
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetNavigatorItems()
        {
            return TreeViewFactory.GetTreeNodes<GradeAndClass>(_gradeRepository);
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(Student bo, StudentVM boVM)
        {
            boVM.Id              = bo.Id;
            boVM.Name            = bo.Name;
            boVM.Description     = bo.Description;
            boVM.SortCode        = bo.SortCode;
            boVM.EmployeeCode    = bo.EmployeeCode;
            boVM.TelephoneNumber = bo.TelephoneNumber;
            boVM.MobileNumber    = bo.Mobile;
            boVM.Email           = bo.Email;
            boVM.Birthday        = bo.Birthday.ToString("yyyy-MM-dd");
            boVM.CredentialsCode = bo.CredentialsCode;
            boVM.Address         = bo.Address;
            boVM.CreateDateTime  = bo.CreateDateTime.ToString("yyyy-MM-dd");
            boVM.ExpiredDateTime = bo.ExpiredDateTime.ToString("yyyy-MM-dd");
            boVM.AvatarPath      = bo.AvatarPath;

            if (bo.GradeAndClass != null)
            {
                boVM.GradeAndClassId = bo.GradeAndClass.Id.ToString();
                boVM.GradeAndClassName = bo.GradeAndClass.Name;
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
        private void _VMMapToBo(Student bo, StudentVM boVM)
        {
            bo.Id              = boVM.Id;
            bo.Name            = boVM.Name;
            bo.Description     = boVM.Description;
            bo.EmployeeCode    = boVM.EmployeeCode;
            bo.TelephoneNumber = boVM.TelephoneNumber;
            bo.Mobile          = boVM.MobileNumber;
            bo.Email           = boVM.Email;
            bo.Birthday        = DateTime.Parse(boVM.Birthday);
            bo.CredentialsCode = boVM.CredentialsCode;
            bo.Address         = boVM.Address;
            bo.CreateDateTime  = DateTime.Parse(boVM.CreateDateTime);
            bo.ExpiredDateTime = DateTime.Parse(boVM.ExpiredDateTime);
        }


    }
}
