using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Organization
{
    /// <summary>
    /// 工作职位
    /// </summary>
    public class JobTitle : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }                    // 工作职位名称
        [StringLength(500)]
        public string Description { get; set; }             // 工作职位要求说明
        [StringLength(150)]
        public string SortCode { get; set; }                // 业务编码
        public JobTitleTypeEnum JobTitleType { get; set; }  // 职位责任类型


        public JobTitle()
        {
            Name = Description = SortCode = "";
            this.Id = Guid.NewGuid();
        }
    }
}
