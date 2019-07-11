using ELearning.Entities.Tools;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    /// <summary>
    /// 课程结构单元的配置内容
    /// </summary>
    public class CourseItemContent : IEntity
    {
        [Key]
        public Guid Id { get; set; }                    // 标识码
        [StringLength(100)]
        public string Name { get; set; }                // 配置内容名称
        public string Description { get; set; }         // 配置内容简要说明 
        [StringLength(200)]
        public string SortCode { get; set; }            // 配置内容业务编码
        [StringLength(200)]
        public string SecondTitle { get; set; }         // 副标题
        [StringLength(500)]
        public string HeadContent { get; set; }         // 页眉内容
        [StringLength(500)]
        public string FootContent { get; set; }         // 页脚内容
        public DateTime UpdateDate { get; set; }        // 更新日期

        public virtual ApplicationUser Editor { get; set; }  // 创建人

        public CourseItemContent()
        {
            this.Id = Guid.NewGuid();
            Name = Description = SecondTitle = HeadContent = FootContent = "";
            UpdateDate = DateTime.Now;
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<CourseItemContent>();
        }
    }
}
