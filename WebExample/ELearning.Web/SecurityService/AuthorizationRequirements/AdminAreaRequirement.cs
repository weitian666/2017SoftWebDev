using ELearning.UserAndRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirements
{
    /// <summary>
    /// 管理员区域限制要求
    /// </summary>
    public class AdminAreaRequirement : IAuthorizationRequirement
    {
        public ApplicationRoleTypeEnum RoleType { get; }

        public AdminAreaRequirement(ApplicationRoleTypeEnum roleType)
        {
            this.RoleType = roleType;
        }
    }
}
