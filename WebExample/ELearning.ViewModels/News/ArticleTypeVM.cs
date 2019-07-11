using ELearning.DataAccess.Tools;
using ELearning.Entities.News;
using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.News
{
    /// <summary>
    /// 文章类型视图模型
    /// </summary>
    public class ArticleTypeVM: IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Display(Name = "类型名称")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        [Required(ErrorMessage = "名称不能为空值。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "类型编码")]
        [StringLength(10, ErrorMessage = "你输入的数据超出限制10个字符的长度。")]
        [Required(ErrorMessage = "编码不能为空值。")]
        public string SortCode { get; set; }

        [Display(Name = "上级类型")]
        public string ParentItemID { get; set; }                       // //    // 这个用于编辑、新建时候选择项的绑定属性
        public string ParentItemName { get; set; }                          // 这个用于在明细或者列表时候呈现关联信息
        public List<SelfReferentialItem> ParentItemCollection { get; set; } // 这个用于提供给前端实现时所需要的下拉或者列表选择的时候的元素

        public ArticleTypeVM()
        {
        }
    }
}
