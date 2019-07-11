using ELearning.Entities.Tools.AdditionalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    /// <summary>
    /// 用于为课程授权处理提供的一个简单视图模型
    /// </summary>
    public class AuthorizationTypeForCourseVM
    {
        public AuthorizationTypeEnum AuthorizationType { get; set; }
        public string AuthorizationTypeEnumName { get; set; }

        public string AuthorizationTypeDisplayName { get; set; }
        public string AuthorizationDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
