using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Organization
{
    /// <summary>
    /// 组织之下的层级结构部门
    /// </summary>
    public class Department : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(100)]
        public string SortCode { get; set; }

        public DepartmentTypeEnum DepartmentType { get; set; }       // 部门种类

        public virtual Organ Organization { get; set; }              // 归属组织，当部门元素为根节点时再处理这个值
        public virtual Department ParentDepartment { get; set; }     // 上级部门
        public virtual ApplicationRole ApplicationRole { get; set; } // 关联角色组

        public Department()
        {
            Name = Description = SortCode = "";
            this.Id = Guid.NewGuid();
        }
    }
}
