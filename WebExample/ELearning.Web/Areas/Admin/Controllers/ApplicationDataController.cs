﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationDataController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["ModuleName"] = "系统活动数据统计与分析";
            ViewData["FunctionName"] = "系统访问日志";
            return View();
        }

        public IActionResult BusisnessAnalysis()
        {
            ViewData["ModuleName"] = "系统活动数据统计与分析";
            ViewData["FunctionName"] = "业务数据统计分析";
            return View();
        }

        public IActionResult CourseActivitiesAnalysis()
        {
            ViewData["ModuleName"] = "系统活动数据统计与分析";
            ViewData["FunctionName"] = "课程数据统计分析";
            return View();
        }
    }
}
