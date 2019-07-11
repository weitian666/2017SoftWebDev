using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.Entities.Tools.AdditionalItems;
using ELearning.UserAndRole;
using ELearning.ViewModels.Common;
using ELearning.ViewModels.ControlModels;
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
    /// 课程实体模型和视图模型交互处理：负责在业务实体模型和业务实体视图模型之间的所有与持久层交互的处理操作实现
    /// </summary>
    public class CourseItemVMService
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

        public CourseItemVMService(
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
            IEntityRepository<BusinessVideo> video
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

            _boRepository = boRepository;
            _courseRepository = courseRepository;
            _courseItemContentRepository = courseItemContentRepository;
            _businessFileService = file;
            _businessImageService = image;
            _businessVideoService = video;
        }

        public CourseItemVM GetVM()
        {
            return new CourseItemVM();
        }

        public async Task<CourseItemVM> GetVM(Guid boId)
        {
            var boVM = new CourseItemVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, x => x.ParentCourseItem, y=>y.Course,w=>w.CourseItemContent, z => z.Creator);
            if (bo == null)
            {
                bo = new CourseItem();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            await SetTypeItems(boVM,bo.Course.Id);

            return boVM;
        }

        public async Task<CourseItemVM> GetVM(Guid boId, Guid courseID)
        {
            var boVM = new CourseItemVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, x => x.ParentCourseItem, y => y.Course, w => w.CourseItemContent, z => z.Creator);
            if (bo == null)
            {
                bo = new CourseItem();
                bo.Course = _courseRepository.GetSingle(courseID);
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            await SetTypeItems(boVM,courseID);

            return boVM;
        }

        /// <summary>
        /// 提取缺省的课程（按 SortCode 排序的第一个课程）和之下的课程单元集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseItemVM>> GetboVMCollectionAsyn()
        {
            var boVMCollection = new List<CourseItemVM>();
            var course = _courseRepository.GetAll().OrderBy(x=>x.SortCode).FirstOrDefault();
            if(course !=null)
                boVMCollection = await GetboVMCollectionAsyn(course.Id);

            return boVMCollection;
        }

        /// <summary>
        /// 根据课程 Id 返回课程相关的课程单元视图模型的对象集合
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<List<CourseItemVM>> GetboVMCollectionAsyn(Guid courseId)
        {
            var course = _courseRepository.GetSingle(courseId);
            if (course == null)
            {
                course = _courseRepository.GetAll().FirstOrDefault();
                if (course != null)
                    courseId = course.Id;
            }

            var boCollection = await _boRepository.GetAllAsyn(x => x.Course.Id == courseId);
            var boVMCollection = new List<CourseItemVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = await GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            // 做层次化处理
            var tempItems = SelfReferentialItemFactory<CourseItem>.GetCollection(boCollection.ToList(), true);
            foreach (var item in tempItems)
            {
                var dID = Guid.Parse(item.ID);
                var boVM = boVMCollection.FirstOrDefault(x => x.Id == dID);
                boVM.Name = item.DisplayName;
            }

            return boVMCollection;
        }

        /// <summary>
        /// 返回全部课程视图对象集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoursesVM>> GetCoursesVMCollectionAsyn()
        {
            var boCollection = await _courseRepository.GetAllAsyn();
            var boVMCollection = new List<CoursesVM>();
            var courseFactory = new CourseVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _departmentRepository, _gradeAndClassRepository, _courseWithRolesRepository, _courseWithUsersRepository, _courseRepository);
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = courseFactory.GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }
            return boVMCollection;
        }

        /// <summary>
        /// 返回全部课程视图对象集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoursesVM>> GetCoursesVMCollectionAsyn(string userName, AuthorizationTypeEnum authorizationType)
        {
            var boVMCollection = new List<CoursesVM>();
            var courseService = new CourseVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _departmentRepository, _gradeAndClassRepository, _courseWithRolesRepository, _courseWithUsersRepository, _courseRepository);
            boVMCollection = await courseService.GetboVMCollectionWithUser(userName, authorizationType);
            return boVMCollection;
        }

        /// <summary>
        /// 根据传入的课程的 Id 返回一个课程的视图模型，
        ///     如果不存在指定 Id 的课程，则判断课程集合中的第一个
        ///         如果不存在，则直接返回 null；
        ///         否则返回第一个课程的视图模型。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CoursesVM GetCoursesVM(Guid id)
        {
            var course = _courseRepository.GetSingle(id);
            if (course==null)
            {
                course = _courseRepository.GetAll().OrderBy(x=>x.SortCode).FirstOrDefault();
                if (course == null)
                    return null;
            }
            var courseFactory = new CourseVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _departmentRepository, _gradeAndClassRepository, _courseWithRolesRepository, _courseWithUsersRepository, _courseRepository);

            var courseVM = courseFactory.GetVM(course.Id);
            return courseVM;
        }

        /// <summary>
        /// 根据分页器对象数据，返回分页器约束之下的课程单元集合
        /// </summary>
        /// <param name="listPageParameter"></param>
        /// <returns></returns>
        public async Task<List<CourseItemVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
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
            Expression<Func<CourseItem, bool>> predicateExpession = ExpressionFactoryMethod.GetConditionExpression<CourseItem>(keyword);


            // 获取排序属性所需要的表达式
            var sortExpession = ExpressionFactoryMethod.GetPropertyExpression<CourseItem, object>(listPageParameter.SortProperty);

            // 获取数据集合
            var tempCollection = _boRepository.GetAllIncluding(x => x.ParentCourseItem, y=>y.Course,w=>w.CourseItemContent, z => z.Creator).Where(predicateExpession);

            // 排序
            if (String.IsNullOrEmpty(listPageParameter.SortProperty))
                tempCollection = tempCollection.OrderBy(sortExpession);
            else
                tempCollection = tempCollection.OrderByDescending(sortExpession);

            // 按照类型获取业务对象数据
            if (!String.IsNullOrEmpty(typeID))
            {
                tempCollection = tempCollection.Where(y => y.Course.Id == Guid.Parse(typeID));
                typeName = _courseRepository.GetSingle(Guid.Parse(typeID)).Name;
            }

            var isDescend = String.IsNullOrEmpty(listPageParameter.SortProperty);

            // 分页
            var boCollection = await tempCollection.ToPaginatedListAsync(pageIndex, pageSize);

            var boVMCollection = new List<CourseItemVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = await GetVM(bo.Id);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<CourseItem>(boCollection, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return boVMCollection;
        }

        public async Task<bool> SaveBo(CourseItemVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id, y=>y.Course,z=>z.CourseItemContent);
            if (bo == null)
                bo = new CourseItem();

            _VMMapToBo(bo, boVM);

            if (!String.IsNullOrEmpty(boVM.ParentCourseItemId))
                bo.ParentCourseItem = _boRepository.GetSingle(Guid.Parse(boVM.ParentCourseItemId));
            else
                bo.ParentCourseItem = bo;

            if (!String.IsNullOrEmpty(boVM.CourseId))
                bo.Course = _courseRepository.GetSingle(Guid.Parse(boVM.CourseId));
            else
                bo.Course = _courseRepository.GetAll().FirstOrDefault();

            if (!String.IsNullOrEmpty(boVM.CourseItemContentID) && bo.CourseItemContent == null)
                bo.CourseItemContent = _courseItemContentRepository.GetSingle(Guid.Parse(boVM.CourseItemContentID));

            if (!String.IsNullOrEmpty(boVM.CreatorUserID))
                bo.Creator = await _userManager.FindByIdAsync(boVM.CreatorUserID);

            // 检查和创建缺省的单元内容
            if (bo.CourseItemContent == null)
                bo.CourseItemContent = new CourseItemContent() { Name = bo.Name };
            else
                bo.CourseItemContent.Name = bo.Name;

            var saveResult = await _boRepository.AddOrEditAndSaveAsyn(bo);

            return saveResult;

        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {

            // todo 还需要补充处理如果课程单元内容不为空值的处理情况 
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }

        /// <summary>
        /// 设置与传入的视图模型相关的关联元素的集合值
        /// </summary>
        /// <param name="boVM"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public async Task SetTypeItems(CourseItemVM boVM, Guid courseID)
        {
            var boCollection = await _boRepository.GetAllAsyn(y => y.Course.Id == courseID);

            boVM.ParentCourseItemCollection = SelfReferentialItemFactory<CourseItem>.GetCollection(boCollection.OrderBy(x=>x.SortCode).ToList(), true);
            boVM.CourseItemCollection = PlainFacadeItemFactory<Course>.Get(_courseRepository);
        }

        /// <summary>
        /// 获取课程单元导航树所需要的节点集合
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetNavigatorItems(Guid courseId)
        {
            var items = TreeViewFactory.GetTreeNodes<CourseItem>(_boRepository,x=>x.Course.Id==courseId);
            return items;
        }

        public async Task<bool> SaveCourseItemContent(CourseItemContentVM boVM)
        {
            var contentFactory = new CourseItemContentVMService(_userManager,_courseItemContentRepository,_boRepository);
            var saveStatus = await contentFactory.SaveBo(boVM);

            return saveStatus;

        }

        /// <summary>
        /// 根据传入 CourseItem 的 Id 获取 CourseItemContentVM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CourseItemContentVM> GetCourseItemContentVM(Guid id)
        {
            // 初始化数据对象
            var bo = _boRepository.GetSingle(id, x => x.ParentCourseItem, y => y.Course, w => w.CourseItemContent, z => z.Creator);
            var content = bo.CourseItemContent;
            var contentFactory = new CourseItemContentVMService(_userManager, _courseItemContentRepository, _boRepository);
            var contentVM = contentFactory.GetVM(content.Id);
            if (contentVM == null)
            {
                contentVM = new CourseItemContentVM();
            }
            else
            {
                contentVM.CourseID = bo.Course.Id.ToString();
                contentVM.CourseName = bo.Course.Name;
                contentVM.CourseItemID = bo.Id.ToString();
                contentVM.CourseItemName = bo.Name;
            }

            // 处理附件
            var video = _businessVideoService.GetSingleBy(x => x.RelevanceObjectID == content.Id);
            if (video == null)
                video = new BusinessVideo();
            contentVM.Video = new BusinessVideoVM(video);

            var images =await _businessImageService.GetAllAsyn(x => x.RelevanceObjectID == content.Id);
            contentVM.ImageCollection = new List<BusinessImageVM>();
            foreach (var item in images)
            {
                contentVM.ImageCollection.Add(new BusinessImageVM(item));
            }

            var files = await _businessFileService.GetAllAsyn(x => x.RelevanceObjectID == content.Id);
            contentVM.FileCollection = new List<BusinessFileVM>();
            foreach (var item in files)
            {
                contentVM.FileCollection.Add(new BusinessFileVM(item));
            }

            return contentVM;
        }

        /// <summary>
        /// 根据传入 CourseItemContent 的 Id 获取 CourseItemContentVM
        /// </summary>
        /// <returns></returns>
        public async Task<CourseItemContentVM> GetCourseItemContentVMBySelfId(Guid id)
        {
            var contentFactory = new CourseItemContentVMService(_userManager, _courseItemContentRepository, _boRepository);
            var contentVM = contentFactory.GetVM(id);
            var bo = _boRepository.GetAllIncluding(x => x.ParentCourseItem, y => y.Course, w => w.CourseItemContent, z => z.Creator).Where(x=>x.CourseItemContent.Id==id).FirstOrDefault();

            contentVM.CourseID = bo.Course.Id.ToString();
            contentVM.CourseName = bo.Course.Name;
            contentVM.CourseItemID = bo.Id.ToString();
            contentVM.CourseItemName = bo.Name;

            // 处理附件
            var video = _businessVideoService.GetSingleBy(x => x.RelevanceObjectID == id);
            if (video == null)
                video = new BusinessVideo();
            contentVM.Video = new BusinessVideoVM(video);

            var images = await _businessImageService.GetAllAsyn(x => x.RelevanceObjectID == id);
            contentVM.ImageCollection = new List<BusinessImageVM>();
            foreach (var item in images)
            {
                contentVM.ImageCollection.Add(new BusinessImageVM(item));
            }

            var files = await _businessFileService.GetAllAsyn(x => x.RelevanceObjectID == id);
            contentVM.FileCollection = new List<BusinessFileVM>();
            foreach (var item in files)
            {
                contentVM.FileCollection.Add(new BusinessFileVM(item));
            }

            return contentVM;
        }

        /// <summary>
        /// 根据传入 CourseItemContent 的 Id 获取 CourseItemContentVM
        /// </summary>
        /// <returns></returns>
        public async Task<CourseItemContentVM> DeletCourseItemContentFile(Guid id, Guid businessFileId)
        {
            _businessFileService.DeleteAndSave(businessFileId);

            var contentFactory = new CourseItemContentVMService(_userManager, _courseItemContentRepository, _boRepository);
            var contentVM = contentFactory.GetVM(id);
            var bo = _boRepository.GetAllIncluding(x => x.ParentCourseItem, y => y.Course, w => w.CourseItemContent, z => z.Creator).Where(x => x.CourseItemContent.Id == id).FirstOrDefault();

            contentVM.CourseID = bo.Course.Id.ToString();
            contentVM.CourseName = bo.Course.Name;
            contentVM.CourseItemID = bo.Id.ToString();
            contentVM.CourseItemName = bo.Name;

            // 处理附件
            var video = _businessVideoService.GetSingleBy(x => x.RelevanceObjectID == id);
            if (video == null)
                video = new BusinessVideo();
            contentVM.Video = new BusinessVideoVM(video);

            var images = await _businessImageService.GetAllAsyn(x => x.RelevanceObjectID == id);
            contentVM.ImageCollection = new List<BusinessImageVM>();
            foreach (var item in images)
            {
                contentVM.ImageCollection.Add(new BusinessImageVM(item));
            }

            var files = await _businessFileService.GetAllAsyn(x => x.RelevanceObjectID == id);
            contentVM.FileCollection = new List<BusinessFileVM>();
            foreach (var item in files)
            {
                contentVM.FileCollection.Add(new BusinessFileVM(item));
            }

            return contentVM;
        }

        private void _BoMapToVM(CourseItem bo, CourseItemVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;

            if (bo.ParentCourseItem != null)
            {
                boVM.ParentCourseItemId = bo.ParentCourseItem.Id.ToString();
                boVM.ParentCourseItemName = bo.ParentCourseItem.Name;
            }

            if (bo.Course != null)
            {
                boVM.CourseId = bo.Course.Id.ToString();
                boVM.CourseName = bo.Course.Name;
            }

            if (bo.CourseItemContent != null)
            {
                boVM.CourseItemContentID = bo.CourseItemContent.Id.ToString();
                boVM.CourseItemContentName = bo.CourseItemContent.Name;
            }

            if (bo.Creator != null)
            {
                boVM.CreatorUserID = bo.Creator.Id.ToString();
                boVM.CreatorUserName = bo.Creator.UserName;
            }

        }

        private void _VMMapToBo(CourseItem bo, CourseItemVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.Description = boVM.Description;
            bo.SortCode = boVM.SortCode;
        }

    }
}
