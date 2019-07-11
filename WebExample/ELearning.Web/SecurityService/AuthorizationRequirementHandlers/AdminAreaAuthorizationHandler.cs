using ELearning.UserAndRole;
using ELearning.Web.SecurityService.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirementHandlers
{
    /// <summary>
    /// 管理员区域访问授权限制
    /// </summary>
    public class AdminAreaAuthorizationHandler : AuthorizationHandler<AdminAreaRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminAreaRequirement requirement
            )
        {
            // 检查是否有 ClaimTypes.Name 声明类型的元素
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return Task.CompletedTask;
            }

            // 检查是否存在
            var adminClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Value == requirement.RoleType.ToString());
            if(adminClaim != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
