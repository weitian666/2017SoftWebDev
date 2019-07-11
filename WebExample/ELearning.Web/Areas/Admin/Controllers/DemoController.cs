using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "新闻信息发布用户")]
    public class DemoController : Controller
    {
        private List<TestVM> _BoCollction;

        public DemoController()
        {
            _SetBoCollctio();
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            ViewData["ModuleName"] = "演示的模块";
            ViewData["FunctionName"] = "数据列表";
            var testVMCollection = _BoCollction;

            int i = 0;
            foreach (var item in testVMCollection)
                item.OrderNumber = (++i).ToString();

            return View(testVMCollection);
        }

        [Area("Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var testVM = new TestVM();
            testVM.LeaderItemCollection = new List<ViewModels.ControlModels.PlainFacadeItem>();
            foreach (var item in _BoCollction)
            {
                testVM.LeaderItemCollection.Add(new ViewModels.ControlModels.PlainFacadeItem() { ID = item.ID.ToString(), Name = item.Name });
            }
            testVM.Name = "TestData";
            testVM.Birthday = DateTime.Now.ToString("yyyy-MM-dd");
            testVM.Description = "一大批的演示数据。";
            ViewData["ModuleName"] = "演示的模块";
            ViewData["FunctionName"] = "新建业务数据";
            return View(testVM);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult Create([Bind("ID,Name,Email,Mobile,Description, SortCode,Password,PasswordComfirm,Sex,Birthday,LeaderID")]TestVM testVM)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction(nameof(Index));
            }

            testVM.LeaderItemCollection = new List<ViewModels.ControlModels.PlainFacadeItem>();
            foreach (var item in _BoCollction)
            {
                testVM.LeaderItemCollection.Add(new ViewModels.ControlModels.PlainFacadeItem() { ID = item.ID.ToString(), Name = item.Name });
            }

            ViewData["Title"] = "新建员工数据";
            ViewData["ModuleName"] = "演示的模块";
            ViewData["FunctionName"] = "新建业务数据";
            return View(testVM);
        }

        private void _SetBoCollctio()
        {
            _BoCollction = new List<TestVM>()
            {
                new TestVM() {ID=Guid.NewGuid(), Name="刘虎军", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="魏小花", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="李文慧", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="张江的", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="萧可君", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="魏铜生", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="刘德华", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="魏星亮", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="潘家富", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="黎温德", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="邓淇升", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="谭希林", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="陈琳", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="祁华钰", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="胡德财", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介" },
                new TestVM() {ID=Guid.NewGuid(), Name="吴富贵", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介" }
            };
        }

    }
}