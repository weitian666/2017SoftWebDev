using ELearning.Entities.Tools;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Organization
{
    /// <summary>
    /// 班级定义管理
    /// </summary>
    public class GradeAndClass:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(100)]
        public string SortCode { get; set; }

        public DateTime CreateDateTime { get; set; }              // 开学日期
        public DateTime ExpiredDateTime { get; set; }             // 结学日期

        public virtual GradeAndClass ParentDepartment { get; set; }  // 上级
        public virtual ApplicationRole ApplicationRole { get; set; } // 关联角色组

        public GradeAndClass()
        {
            this.Id = Guid.NewGuid();
            CreateDateTime = ExpiredDateTime = DateTime.Now;
            SortCode = Name = Description = "";
            //SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<Employee>();
        }
    }
}
