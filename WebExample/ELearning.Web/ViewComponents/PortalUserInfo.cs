using ELearning.DataAccess;
using ELearning.Entities.Common;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.ViewComponents
{
    /// <summary>
    /// 用于处理门户站点的用户登录信息的动态显示的视图组件
    /// </summary>
    public class PortalUserInfoViewComponent : ViewComponent
    {
        #region 需要处理的数据
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<BusinessImage> _businessImageRepository;
        private readonly IEntityRepository<Course> _courseRepository;
        #endregion

        public PortalUserInfoViewComponent(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<BusinessImage> businessImageRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _businessImageRepository = businessImageRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAvatarPath = "/images/user.png";
            var userName = "";
            var userRole = "";

            var userIdentity = User.Identity;
            if (!String.IsNullOrEmpty(userIdentity.Name))
            {
                var user = await _userManager.FindByNameAsync(userIdentity.Name);
                var userImage = _businessImageRepository.GetSingleBy(x => x.RelevanceObjectID == user.Id);
                if (userImage != null)
                    userAvatarPath = _businessImageRepository.GetSingleBy(x => x.RelevanceObjectID == user.Id).UploadPath;
                userName = user.ChineseFullName;
                var roles = await _userManager.GetRolesAsync(user);
                userRole = roles.FirstOrDefault();
            }

            ViewData["LoginUserAvatarPath"] = userAvatarPath;
            ViewData["LoginUserName"] = userName;
            ViewData["LoginUserRoleName"] = userName;

            return View("Default");
        }

    }


}
