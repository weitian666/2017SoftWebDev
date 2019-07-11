using ELearning.Entities.TeachingCourse;
using ELearning.Web.SecurityService.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirementHandlers
{
    public class CourseAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, Course>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Course resource
            )
        {
            if (context.User.Identity?.Name == resource.Creator.UserName && requirement.Name == CommonOperationRequirements.Read.Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
