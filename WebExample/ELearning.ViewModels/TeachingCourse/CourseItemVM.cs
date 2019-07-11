using ELearning.DataAccess.Tools;
using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    public class CourseItemVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "单元名称不能为空值。")]
        [Display(Name = "单元名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Required(ErrorMessage = "单元编码不能为空值。")]
        [Display(Name = "单元编码")]
        [StringLength(30, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string SortCode { get; set; }

        [Display(Name = "上级单元")]
        public string ParentCourseItemId { get; set; }
        public string ParentCourseItemName { get; set; }
        public List<SelfReferentialItem> ParentCourseItemCollection { get; set; }

        [Display(Name = "归属课程")]
        [Required(ErrorMessage = "归属课程是必须选择的。")]
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public List<PlainFacadeItem> CourseItemCollection { get; set; }

        public string CourseItemContentID { get; set; } 
        public string CourseItemContentName { get; set; }

        public string CreatorUserID { get; set; } 
        public string CreatorUserName { get; set; }

        public string SaveStatus { get; set; }

        public CourseItemVM()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }

    }
}
