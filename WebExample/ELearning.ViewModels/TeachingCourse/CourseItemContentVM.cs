using ELearning.DataAccess.Tools;
using ELearning.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    public class CourseItemContentVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "标题不能为空值。")]
        [Display(Name = "标题")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "正文内容")]
        public string Description { get; set; }
        public string SortCode { get; set; }

        [Display(Name = "副标题")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制200个字符的长度。")]
        public string SecondTitle { get; set; }  

        [Display(Name = "页眉")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制200个字符的长度。")]
        public string HeadContent { get; set; }

        [Display(Name = "页脚")]
        [StringLength(200, ErrorMessage = "你输入的数据超出限制200个字符的长度。")]
        public string FootContent { get; set; }

        public string UpdateDate { get; set; }

        public string CourseItemID { get; set; }
        public string CourseItemName { get; set; }

        public string CourseID { get; set; }
        public string CourseName { get; set; }

        public string CreatorUserID { get; set; }
        public string CreatorUserName { get; set; }

        public BusinessVideoVM Video { get; set; }
        public List<BusinessFileVM> FileCollection { get; set; }
        public List<BusinessImageVM> ImageCollection { get; set; }

        public string SaveStatus { get; set; }

        public CourseItemContentVM()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }

    }
}
