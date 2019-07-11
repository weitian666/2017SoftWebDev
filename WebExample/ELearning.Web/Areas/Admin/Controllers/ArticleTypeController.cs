using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.News;
using ELearning.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleTypeController : Controller
    {
        private readonly IEntityRepository<ArticleType> _boRepository;
        private ArticleTypeVMService _boVMService;

        public ArticleTypeController(
            IEntityRepository<ArticleType> repository
            )
        {
            _boRepository = repository;
            _boVMService = new ArticleTypeVMService(_boRepository);
        }

        public async Task<IActionResult> Index()
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "站点文章类型管理";
            return View(boVMCollection);
        }

        [HttpGet]
        public IActionResult CreateOrEdit(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "编辑文章类型";
            return View(boVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(ArticleTypeVM boVM)
        {
            if (ModelState.IsValid)
            {
                var x = await _boVMService.SaveBo(boVM);
                if (x)
                    return RedirectToAction("Index");
            }

            _boVMService.SetRelevanceItems(boVM);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "编辑文章类型";
            return View(boVM);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var boVM = _boVMService.GetVM(id);

            ViewData["ModuleName"] = "站点文章信息管理";
            ViewData["FunctionName"] = "文章类型明细数据";
            return View(boVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _boVMService.DeletBoStatus(id);
            return Json(status);
        }

    }
}
