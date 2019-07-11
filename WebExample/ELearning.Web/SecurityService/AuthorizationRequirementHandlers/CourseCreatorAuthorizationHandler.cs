using ELearning.Web.SecurityService.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirementHandlers
{ 
    public class CourseCreatorAuthorizationHandler : AuthorizationHandler<CourseCreatorRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CourseCreatorRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "http://contoso.com"))
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
