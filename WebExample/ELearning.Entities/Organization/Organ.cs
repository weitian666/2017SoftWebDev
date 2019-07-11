using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Organization
{
    /// <summary>
    /// 组织，作为最大的组织机构的定义来使用
    /// </summary>
    public class Organ:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }   // 组织名称
        [StringLength(500)]
        public string Description { get; set; }  // 组织说明
        [StringLength(100)]
        public string SortCode { get; set; } // 组织编码

        public Organ()
        {
            Name = Description = SortCode = "";
            this.Id = Guid.NewGuid();
        }
    }
}
