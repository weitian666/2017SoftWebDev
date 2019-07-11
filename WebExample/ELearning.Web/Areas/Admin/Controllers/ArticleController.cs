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

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
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

        public ArticleController(
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

        public async Task<PartialViewResult> Navigator()
        {
            var itemCollection = await _boVMService.GetArticleTopicVMCollection();
            return PartialView("_Navigator", itemCollection);
        }

        public async Task<PartialViewResult> CommonList()
        {
            _listPageParameter.SortProperty = "SortCode";
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(_listPageParameter);

            ViewData["PageGroup"] = _listPageParameter.PagenateGroup;
            ViewData["ItemAmount"] = _listPageParameter.ObjectAmount;
            ViewData["ListPageParameter"] = _listPageParameter;
            ViewData["Keyword"] = "";
            ViewData["ArticleTopicVMCollection"] = await _boVMService.GetArticleTopicVMCollection();

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "文章数据列表:所有栏目";
            return PartialView("_List", boVMCollection);
        }

        [HttpGet]
        [HttpPost]
        public async Task<PartialViewResult> List(string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn(listPagePara);

            ViewData["PageGroup"] = listPagePara.PagenateGroup;
            ViewData["ItemAmount"] = listPagePara.ObjectAmount;
            ViewData["ListPageParameter"] = listPagePara;
            ViewData["Keyword"] = listPagePara.Keyword;

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "文章数据列表：" + listPagePara.TypeName;

            return PartialView("_List", boVMCollection);
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateOrEdit(Guid id)
        {
            var functionName = "编辑文章数据";
            var boVM = await _boVMService.GetVM(id);
            if (!boVM.IsNew)
            {
                functionName = "编辑文章数据：" + boVM.Name;
            }

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = functionName;
            ViewData["BoVMId"] = boVM.Id;

            return PartialView("_CreateOrEdit", boVM);

        }

        /// <summary>
        /// 保存教学单元内容数据
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(ArticleVM boVM)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = User.Identity;
                if (!String.IsNullOrEmpty(userIdentity.Name))
                {
                    boVM.CreateUserName = userIdentity.Name;
                }
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                {
                    boVM = await  _boVMService.GetVM(boVM.Id);
                    boVM.SaveStatus = "数据保存成功！";
                    ViewData["ModuleName"] = "站点文章信息管理";
                    ViewData["FunctionName"] = "编辑文章数据：" + boVM.Name;
                    ViewData["BoVMId"] = boVM.Id;

                    return PartialView("_CreateOrEdit", boVM);
                }
                else
                {

                    boVM.SaveStatus = "数据保存出现问题，请联系有关人员协助处理！";
                    await _boVMService.SetRelevanceItems(boVM);
                    ViewData["ModuleName"] = "站点文章信息管理";
                    ViewData["FunctionName"] = "编辑文章数据：" + boVM.Name;
                    ViewData["BoVMId"] = boVM.Id;

                    return PartialView("_CreateOrEdit", boVM);
                }
            }

            await _boVMService.SetRelevanceItems(boVM);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "编辑文章数据：" + boVM.Name;
            ViewData["BoVMId"] = boVM.Id;
            return PartialView("_CreateOrEdit", boVM);
        }

        public async Task<PartialViewResult> Detail(Guid id)
        {
            var boVM = await _boVMService.GetVM(id);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "预览：" + boVM.Name;
            ViewData["BoVMId"] = boVM.Id;

            return PartialView("_Detail", boVM);

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

        public async Task<IActionResult> RefreshAttachmentFiles(Guid id)
        {
            var boVM = await _boVMService.GetVM(id);
            ViewData["BoVMId"] = boVM.Id;
            return PartialView("_ArticleAttachmentFiles", boVM.FileCollection);
        }

        public async Task<IActionResult> DeleteAttachmentFiles(Guid id, Guid businessFileId)
        {
            var boVM = await _boVMService.DeleteAttachmentFiles(id, businessFileId);
            ViewData["BoVMId"] = boVM.Id;
            return PartialView("_ArticleAttachmentFiles", boVM.FileCollection);
        }

    }
}
