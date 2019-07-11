using ELearning.Entities.Tools.AdditionalItems;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    /// <summary>
    /// 课程与角色组
    /// </summary>
    public class CourseWithRoles : IEntityBase
    {
        public Guid Id { get; set; }
        public AuthorizationTypeEnum AuthorizationTypeEnum { get; set; }  // 授权类型

        public virtual Course Course { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }

        public CourseWithRoles()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
