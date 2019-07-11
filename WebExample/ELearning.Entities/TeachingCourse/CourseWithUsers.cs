using ELearning.Entities.Tools.AdditionalItems;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    /// <summary>
    /// 课程与用户
    /// </summary>
    public class CourseWithUsers:IEntityBase
    {
        public Guid Id { get; set; }
        public AuthorizationTypeEnum AuthorizationTypeEnum { get; set; }  // 授权类型

        public virtual Course Course { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public CourseWithUsers()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
