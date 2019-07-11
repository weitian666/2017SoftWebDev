using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.UserAndRole;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.Areas.Admin.ViewComponents
{
    public class LoginUserInfoViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<BusinessImage> _businessImageService;
        private readonly IHostingEnvironment _hostingEnvironment;  // 系统驻留环境

        public LoginUserInfoViewComponent(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<BusinessImage> businessImageService, 
            IHostingEnvironment hostingEnvironment
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _businessImageService = businessImageService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAvatarPath = "/images/user.png";
            var userName = "";

            var userIdentity = User.Identity;
            if(!String.IsNullOrEmpty(userIdentity.Name))
            {
                var user = await _userManager.FindByNameAsync(userIdentity.Name);
                var userImage = _businessImageService.GetSingleBy(x => x.RelevanceObjectID == user.Id);
                if(userImage!=null)
                    userAvatarPath = _businessImageService.GetSingleBy(x => x.RelevanceObjectID == user.Id).UploadPath;
                userName = user.ChineseFullName;
            }

            ViewData["LoginUserAvatarPath"] = userAvatarPath;
            ViewData["LoginUserName"] = userName;

            return View("Default");
        }

    }
}
