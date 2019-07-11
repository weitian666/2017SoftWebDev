using ELearning.DataAccess.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.News
{
    public class ArticleTopicVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "名称不能为空值。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "编码不能为空值。")]
        public string SortCode { get; set; }

        public int ArticleAmount { get; set; }
    }
}
