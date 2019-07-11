using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course:IEntity
    {
        [Key]
        public Guid Id { get; set; }            // 标识码
        [StringLength(100)]
        public string Name { get; set; }        // 课目名称
        [StringLength(5000)]
        public string Description { get; set; } // 课目简要说明 
        [StringLength(100)]
        public string SortCode { get; set; }    // 业务编码

        public DateTime OpenDate { get; set; }  // 公开日期
        public DateTime CloseDate { get; set; } // 关闭日期

        public virtual ApplicationUser Creator { get; set; }             // 课程创建人

        public virtual ApplicationUser CourseAdministrator { get; set; } // 课程管理员

        public Course()
        {
            this.Id = Guid.NewGuid();
            Name = Description = "";
            OpenDate = CloseDate = DateTime.Now;
        }
    }
}
