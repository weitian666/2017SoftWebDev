using ELearning.DataAccess.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    public class CoursesVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "课程名称不能为空值。")]
        [Display(Name = "课程名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Required(ErrorMessage = "课程编码不能为空值。")]
        [Display(Name = "课程编码")]
        [StringLength(10, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string SortCode { get; set; }

        [Display(Name = "开放日期")]
        [Required(ErrorMessage = "开放日期是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string OpenDate { get; set; }

        [Display(Name = "关闭日期")]
        [Required(ErrorMessage = "关闭日期日期是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string CloseDate { get; set; }

        public String CourseCreatorId { get; set; }
        public String CourseCreatorName { get; set; }

        public String CourseAdministrtorId { get; set; }
        public String CourseAdministrtorName { get; set; }

        public string CourseItemAmount { get; set; }

        public bool IsCreatedByMe { get; set; } // 用于处理是否是当前用户创建的标识

        public CoursesVM()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }
    }
}
