using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Common;
using ELearning.Entities.News;
using ELearning.UserAndRole;
using ELearning.ViewModels.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Controllers
{
    /// <summary>
    /// 公司新闻信息
    /// </summary>
    public class NewsController : Controller
    {
        private readonly IEntityRepository<Article> _boRepository;
        private readonly IEntityRepository<ArticleType> _articleTypeRepository;
        private readonly IEntityRepository<ArticleTopic> _articleTopicRepository;
        private readonly IEntityRepository<ArticleInType> _articleInTypeRepository;
        private readonly IEntityRepository<ArticleInTopic> _articleInTopicRepository;
        private readonly IEntityRepository<ArticleWithFile> _articleWithFileRepository;
        private readonly IEntityRepository<ArticleComment> _articleCommentRepository;
        private readonly IEntityRepository<ArticleCommentTag> _articleCommentTagRepository;
        private readonly IEntityRepository<ArticleRelevance> _articleRelevanceRepository;
        private readonly IEntityRepository<ArticleRelevanceTag> _articleRelevanceTagRepository;
        private readonly IEntityRepository<BusinessImage> _businessImageService;
        private readonly IEntityRepository<BusinessFile> _businessFileService;
        private readonly IEntityRepository<BusinessVideo> _businessVideoService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private int _pageSize = 18;                                                   // 列表单页显示元素的条数
        private int _pageIndex = 1;                                                   // 列表页的当前页码
        private ListPageParameter _listPageParameter = new ListPageParameter(1, 18);  // 列表页处理所需要的参数
        private ArticleVMService _boVMService;

        public NewsController(
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
            _boRepository = boRepository;
            _articleTypeRepository = articleTypeRepository;
            _articleTopicRepository = articleTopicRepository;
            _articleInTypeRepository = articleInTypeRepository;
            _articleInTopicRepository = articleInTopicRepository;
            _articleWithFileRepository = articleWithFileRepository;
            _articleCommentRepository = articleCommentRepository;
            _articleCommentTagRepository = articleCommentTagRepository;
            _articleRelevanceRepository = articleRelevanceRepository;
            _articleRelevanceTagRepository = articleRelevanceTagRepository;
            _businessFileService = file;
            _businessImageService = image;
            _businessVideoService = video;


            _userManager = userManager;
            _roleManager = roleManager;

            _boVMService = new ArticleVMService(
                _boRepository,
                _articleTypeRepository,
                _articleTopicRepository,
                _articleInTypeRepository,
                _articleInTopicRepository,
                _articleWithFileRepository,
                _articleCommentRepository,
                _articleCommentTagRepository,
                _articleRelevanceRepository,
                _articleRelevanceTagRepository,
                _businessImageService,
                _businessFileService,
                _businessVideoService,
                _userManager,
                _roleManager);

        }
        
        public async Task<IActionResult> Index()
        {
            _listPageParameter.SortProperty = "SortCode";
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;
            ViewData["ArticleTopicVMCollection"] = await _boVMService.GetArticleTopicVMCollection();

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "文章数据列表:所有栏目";
            return View(boVMCollection);
        }

        public async Task<IActionResult> ArticleContent(Guid id)
        {
            var boVM = await _boVMService.GetVM(id);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "预览：" + boVM.Name;
            ViewData["BoVMId"] = boVM.Id;

            return View("ArticleContent",boVM);

        }
    }
}
