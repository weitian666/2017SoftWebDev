using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.News;
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

namespace ELearning.ViewModels.News
{
    public class ArticleVMService
    {
        private readonly IEntityRepository<Article>             _boRepository;
        private readonly IEntityRepository<ArticleType>         _articleTypeRepository;
        private readonly IEntityRepository<ArticleTopic>        _articleTopicRepository;
        private readonly IEntityRepository<ArticleInType>       _articleInTypeRepository;
        private readonly IEntityRepository<ArticleInTopic>      _articleInTopicRepository;
        private readonly IEntityRepository<ArticleWithFile>     _articleWithFileRepository;
        private readonly IEntityRepository<ArticleComment>      _articleCommentRepository;
        private readonly IEntityRepository<ArticleCommentTag>   _articleCommentTagRepository;
        private readonly IEntityRepository<ArticleRelevance>    _articleRelevanceRepository;
        private readonly IEntityRepository<ArticleRelevanceTag> _articleRelevanceTagRepository;
        private readonly IEntityRepository<BusinessImage>       _businessImageService;
        private readonly IEntityRepository<BusinessFile>        _businessFileService;
        private readonly IEntityRepository<BusinessVideo>       _businessVideoService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ArticleVMService(
            IEntityRepository<Article> boRepository,
            IEntityRepository<ArticleType> articleTypeRepository,
            IEntityRepository<ArticleTopic> articleTopicRepository,
            IEntityRepository<ArticleInType> articleInTypeRepository,
            IEntityRepository<ArticleInTopic> articleInTopicRepository,
            IEntityRepository<ArticleWithFile> articleWithFileRepository,
            IEntityRepository<ArticleComment> articleCommentRepository,
            IEntityRepository<ArticleCommentTag> articleCommentTagRepository,
            IEntityRepository<ArticleRelevance> articleRelevanceRepository,
            IEntityRepository<ArticleRelevanceTag> articleRelevanceTagRepository,
            IEntityRepository<BusinessImage> image,
            IEntityRepository<BusinessFile> file,
            IEntityRepository<BusinessVideo> video,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _boRepository                  = boRepository ;
            _articleTypeRepository         = articleTypeRepository;
            _articleTopicRepository        = articleTopicRepository;
            _articleInTypeRepository       = articleInTypeRepository;
            _articleInTopicRepository      = articleInTopicRepository;
            _articleWithFileRepository     = articleWithFileRepository;
            _articleCommentRepository      = articleCommentRepository;
            _articleCommentTagRepository   = articleCommentTagRepository;
            _articleRelevanceRepository    = articleRelevanceRepository;
            _articleRelevanceTagRepository = articleRelevanceTagRepository;
            _businessFileService           = file;
            _businessImageService          = image;
            _businessVideoService          = video;


            _userManager = userManager;
            _roleManager = roleManager;

        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public ArticleVM GetVM()
        {
            return new ArticleVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public async Task<ArticleVM> GetVM(Guid id)
        {
            var boVM = new ArticleVM();
            // 初始化数据对象
            var bo = await _boRepository.GetSingleAsyn(id);
            if (bo == null)
            {
                // 缺省的直接创建
                bo = new Article();
                boVM.Id = bo.Id;
                boVM.Name = "";
                await SaveBo(boVM);
                bo = await _boRepository.GetSingleAsyn(boVM.Id);
            }

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            await SetRelevanceItems(boVM);

            return boVM;
        }


        public async Task SetRelevanceItems(ArticleVM boVM)
        {
            // 设置关联的文章附件
            var files = await _businessFileService.GetAllAsyn(x => x.RelevanceObjectID == boVM.Id);
            boVM.FileCollection = new List<BusinessFileVM>();
            foreach (var item in files)
            {
                boVM.FileCollection.Add(new BusinessFileVM(item));
            }

            // 设置关联的栏目
            var tempCollection = await _articleInTopicRepository.GetAllIncludingAsyn(x => x.ArticleTopic, y => y.MasterArticle);
            var articleTopicCollection = from x in tempCollection
                                    where x.MasterArticle.Id == boVM.Id
                                    select x.ArticleTopic;

            boVM.TopicItemIdCollection = new List<string>();
            boVM.TopicItemCollection = new List<ControlModels.PlainFacadeItem>();
            var topicCollection = await _articleTopicRepository.GetAllAsyn();
            foreach (var topic in topicCollection.OrderBy(x => x.SortCode))
            {
                var item = new PlainFacadeItem() { ID = topic.Id.ToString(), Name = topic.Name, IsActive = false };
                if (articleTopicCollection.FirstOrDefault(x => x.Id == topic.Id) != null)
                {
                    item.IsActive = true;
                }
                boVM.TopicItemCollection.Add(item);
            }

        }

        /// <summary>
        /// 根据业务实体对象返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public ArticleVM GetVM(Article bo)
        {
            var boVM = new ArticleVM();
            _BoMapToVM(bo, boVM);
            return boVM;
        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型，返回的视图模型根据要求做了层次化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<ArticleVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllIncludingAsyn(x=>x.CreatorUser);
            var boVMCollection = new List<ArticleVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = GetVM(bo);
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
        public async Task<List<ArticleVM>> GetboVMCollectionAsyn(ListPageParameter listPageParameter)
        {
            var pageIndex = Int16.Parse(listPageParameter.PageIndex);
            var pageSize = Int16.Parse(listPageParameter.PageSize);

            var typeID = "";
            var keyword = "";
            var typeName = "所有文章";
            var objectAmount = 0;

            if (!String.IsNullOrEmpty(listPageParameter.ObjectTypeID))
                typeID = listPageParameter.ObjectTypeID;
            if (!String.IsNullOrEmpty(listPageParameter.Keyword))
                keyword = listPageParameter.Keyword;

            #region 1.构建与 keyword 相关的查询 lambda 表达式，用于对查询结果的过滤（给 Where 使用）
            Expression<Func<Article, bool>> predicateExpession = x =>
                 x.Name.Contains(keyword) ||
                 x.ArticleSecondTitle.Contains(keyword) ||
                 x.ArticleContent.Contains(keyword) ||
                 x.Description.Contains(keyword);
            #endregion

            #region 2.根据属性名称确定排序的属性的 lambda 表达式
            var sortPropertyName = listPageParameter.SortProperty;
            var type = typeof(Article);
            var target = Expression.Parameter(typeof(object));
            var castTarget = Expression.Convert(target, type);
            var getPropertyValue = Expression.Property(castTarget, sortPropertyName);
            var sortExpession = Expression.Lambda<Func<Article, object>>(getPropertyValue, target);
            #endregion

            PaginatedList<Article> boCollection = new PaginatedList<Article>();

            //var roleId = listPageParameter.ObjectTypeID;
            if (String.IsNullOrEmpty(typeID))
            {
                var tempCollection = await _boRepository.GetAllIncludingAsyn(x=>x.CreatorUser);

                objectAmount = _boRepository.GetAll().Count();

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
                var tempCollection = await _articleInTopicRepository.GetAllIncludingAsyn(x => x.ArticleTopic, y => y.MasterArticle);
                var articleTopic = _articleTopicRepository.GetSingle(Guid.Parse(typeID));
                var articleCollection = from x in tempCollection
                                        where x.ArticleTopic.Id == articleTopic.Id
                                        select x.MasterArticle;

                typeName = articleTopic.Name;
                objectAmount = tempCollection.Count();

                if (listPageParameter.SortDesc == "")
                {
                    boCollection = articleCollection.AsQueryable().OrderByDescending(sortExpession).ToPaginatedList(pageIndex, pageSize);
                }
                else
                {
                    boCollection = articleCollection.AsQueryable().OrderBy(sortExpession).ToPaginatedList(pageIndex, pageSize);
                }
            }

            var boVMCollection = new List<ArticleVM>();
            var counter = 0;
            foreach (var bo in boCollection)
            {
                var boVM = new ArticleVM();
                 _BoMapToVM(bo, boVM);
                boVM.OrderNumber = (++counter + (pageIndex - 1) * pageSize).ToString();
                boVMCollection.Add(boVM);
            }


            listPageParameter.PageAmount = boCollection.TotalPageCount.ToString();
            listPageParameter.ObjectAmount = boCollection.TotalCount.ToString();
            listPageParameter.PagenateGroup = PagenateGroupRepository.GetItem<Article>(boCollection, 10, pageIndex);
            listPageParameter.Keyword = keyword;
            listPageParameter.TypeName = typeName;

            return boVMCollection;
        }

        public async Task<bool> SaveBo(ArticleVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id);
            if (bo == null)
                bo = new Article();

            _VMMapToBo(bo, boVM);

            //处理作者用户信息
            if (!String.IsNullOrEmpty(boVM.CreateUserName))
            {
                bo.CreatorUser = await _userManager.FindByNameAsync(boVM.CreateUserName);
            }

            var result = await _boRepository.AddOrEditAndSaveAsyn(bo);

            //处理关联栏目
            if (boVM.TopicItemIdCollection != null)
            {
                var topicCollection = await _articleTopicRepository.GetAllAsyn();
                foreach (var topic in topicCollection)
                {
                    _articleInTopicRepository.DeleteAndSaveBy(x => x.ArticleTopic.Id == topic.Id && x.MasterArticle.Id == bo.Id);
                }
                foreach (var topicIdItem in boVM.TopicItemIdCollection)
                {
                    var topic = await _articleTopicRepository.GetSingleAsyn(Guid.Parse(topicIdItem));
                    var articleInTopic = new ArticleInTopic() { ArticleTopic = topic, MasterArticle = bo };
                    await _articleInTopicRepository.AddOrEditAndSaveAsyn(articleInTopic);
                }
            }

            return result;

        }
        /// <summary>
        /// 提取全部的栏目文章的数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleTopicVM>> GetArticleTopicVMCollection()
        {
            var boCollection = await _articleTopicRepository.GetAllAsyn();
            var boVMCollection = new List<ArticleTopicVM>();

            var counter = 0;
            foreach (var item in boCollection.OrderBy(x=>x.SortCode))
            {
                var boVM = new ArticleTopicVM();
                boVM.Id = item.Id;
                boVM.Name = item.Name;
                boVM.Description = item.Description;

                boVM.OrderNumber = (++counter).ToString();
                boVM.ArticleAmount = await _GetArticleInTopicAmount(item.Id);
                boVMCollection.Add(boVM);
            }

            return boVMCollection;
        }

        /// <summary>
        /// 根据传入 CourseItemContent 的 Id 获取 CourseItemContentVM
        /// </summary>
        /// <returns></returns>
        public async Task<ArticleVM> DeleteAttachmentFiles(Guid id, Guid businessFileId)
        {
            _businessFileService.DeleteAndSave(businessFileId);

            var boVM = await GetVM(id);

            // 处理附件
            var video = _businessVideoService.GetSingleBy(x => x.RelevanceObjectID == id);
            if (video == null)
                video = new BusinessVideo();
            boVM.Video = new BusinessVideoVM(video);

            var files = await _businessFileService.GetAllAsyn(x => x.RelevanceObjectID == id);
            boVM.FileCollection = new List<BusinessFileVM>();
            foreach (var item in files)
            {
                boVM.FileCollection.Add(new BusinessFileVM(item));
            }

            return boVM;
        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            // 首先清理关联栏目
            _articleInTopicRepository.DeleteAndSaveBy(x => x.MasterArticle.Id == id);

            // todo 还需要补充处理如果课程单元内容不为空值的处理情况 
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }


        private async Task<int> _GetArticleInTopicAmount(Guid topicId)
        {
            var tempCollection = await _articleInTopicRepository.GetAllIncludingAsyn(x => x.ArticleTopic, y => y.MasterArticle);
            var articleCollection = from x in tempCollection
                                    where x.ArticleTopic.Id == topicId
                                    select x.MasterArticle;
            return articleCollection.Count();
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(Article bo, ArticleVM boVM)
        {
            boVM.Id                 = bo.Id;
            boVM.Name               = bo.Name;
            boVM.Description        = bo.Description;
            boVM.SortCode           = bo.SortCode;
            boVM.ArticleSecondTitle = bo.ArticleSecondTitle;
            boVM.ArticleSource      = bo.ArticleSource;
            boVM.ArticleContent     = bo.ArticleContent;
            boVM.PublishDate        = bo.PublishDate.ToString("yyyy-MM-dd");
            boVM.UpVoteNumber       = bo.UpVoteNumber;
            if (bo.CreatorUser != null)
                boVM.CreateUserName = bo.CreatorUser.ChineseFullName;
        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _VMMapToBo(Article bo, ArticleVM boVM)
        {
            bo.Id                 = boVM.Id;
            bo.Name               = boVM.Name;
            bo.Description        = boVM.Description;
            bo.SortCode           = boVM.SortCode;
            bo.ArticleSecondTitle = boVM.ArticleSecondTitle;
            bo.ArticleSource      = boVM.ArticleSource;
            bo.ArticleContent     = boVM.ArticleContent;
        }
    }
}
