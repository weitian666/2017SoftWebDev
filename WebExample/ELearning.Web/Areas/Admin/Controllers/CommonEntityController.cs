using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities;
using ELearning.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 针对完全继承并且仅仅使用 IEntity 的业务实体类公共的常规数据处理控制器
    /// </summary>
    [Area("Admin")]
    public class CommonEntityController<T> : Controller where T : class, IEntity, new()
    {
        private readonly IEntityRepository<T> _boService;   // EF 数据库访问处理统一接口
        public string EntityModuleName;                     // 业务实体所对应的模块的名称
        public string EntityTitle;                          // 业务实体对应的实际名称，用于在前端显示相关的名称

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service"></param>
        public CommonEntityController(IEntityRepository<T> service)
        {
            _boService = service;
        }

        /// <summary>
        /// 控制器入口方法，处理一般的列表数据
        /// </summary>
        /// <returns></returns>
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var boCollection = await _boService.GetAllAsyn();
            var boVMCollection = new List<EntityViewModel>();
            var counter = 0;
            foreach (var item in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = new EntityViewModel();
                boVM.SetVM(item);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            ViewData["ModuleName"] = EntityModuleName;
            ViewData["FunctionName"] = EntityTitle + "数据列表";

            return View("../../Views/CommonEntity/Index", boVMCollection);
        }

        [Area("Admin")]
        public async Task<IActionResult> List()
        {
            var boCollection = await _boService.GetAllAsyn();
            var boVMCollection = new List<EntityViewModel>();
            var counter = 0;
            foreach (var item in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = new EntityViewModel();
                boVM.SetVM(item);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            ViewBag.EntityTitle = EntityTitle;
            return View("../../Views/CommonEntity/List", boVMCollection);
        }

        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var subTitle = "：编辑数据";
            var isNew = false;
            var bo = await _boService.GetSingleAsyn(id);
            if (bo == null)
            {
                bo = new T()
                {
                    Name = "",
                    Description = "",
                    SortCode = ""
                };
                isNew = true;
                subTitle = "：新建数据";
            }

            var boVM = new EntityViewModel();
            boVM.SetVM(bo);
            boVM.IsNew = isNew;

            ViewData["ModuleName"] = EntityModuleName;
            ViewData["FunctionName"] = EntityTitle + subTitle;

            return View("../../Views/CommonEntity/CreateOrEdit", boVM);
        }

        [Area("Admin")]
        public async Task<IActionResult> CreateOrEdit(EntityViewModel boVM)
        {
            if (ModelState.IsValid)
            {
                var bo = await _boService.GetSingleAsyn(boVM.Id);
                if (bo == null)
                    bo = new T();
                boVM.MapToBo(bo);
                await _boService.AddOrEditAndSaveAsyn(bo);
                return RedirectToAction("Index");
            }

            ViewData["ModuleName"] = EntityModuleName;
            ViewData["FunctionName"] = EntityTitle + "：编辑数据";
            return View("../../Views/CommonEntity/CreateOrEdit", boVM);
        }

        [Area("Admin")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var bo = await _boService.GetSingleAsyn(id);
            var boVM = new EntityViewModel();
            boVM.SetVM(bo);
            return PartialView("../../Views/CommonEntity/Detail", boVM);
        }

        [Area("Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = new DeleteStatusModel() { DeleteSatus = true, Message = "数据删除成功" };
            try
            {
                await _boService.DeleteAndSaveAsyn(id);
            }
            catch
            {
                status.DeleteSatus = false;
                status.Message = "删除操作出现意外，主要原因是关联数据没有处理干净活者是其他原因。";
            }
            return Json(status);
        }
    }
}