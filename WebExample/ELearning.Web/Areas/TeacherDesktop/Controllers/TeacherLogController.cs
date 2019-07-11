using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ELearning.Web.Areas.TeacherDesktop.Controllers
{
    [Area("TeacherDesktop")]
    public class TeacherLogController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            ViewData["ModuleName"] = "个人系统操作日志";
            ViewData["FunctionName"] = "";
            return View();
        }
    }
}
