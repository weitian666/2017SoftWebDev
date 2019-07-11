using ELearning.Entities.TeachingCourse;
using ELearning.Web.SecurityService.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirementHandlers
{
    public class CourseAdministratorAuthorizationHandler : AuthorizationHandler<CourseAdministratorRequirement, Course>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CourseAdministratorRequirement requirement,
            Course resource
            )
        {
            if (context.User.Identity?.Name == resource.CourseAdministrator.UserName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
