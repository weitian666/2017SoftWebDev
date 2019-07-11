using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.SecurityService.AuthorizationRequirements
{
    /// <summary>
    /// 针对 TeacherDesktopArea 的是否是作者授权请求处理策略
    /// </summary>
    public class CourseCreatorRequirement : IAuthorizationRequirement
    {
    }
}
