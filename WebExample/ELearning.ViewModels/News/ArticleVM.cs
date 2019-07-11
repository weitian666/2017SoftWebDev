using ELearning.DataAccess.Tools;
using ELearning.Entities.News;
using ELearning.ViewModels.Common;
using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.News
{
    /// <summary>
    /// 文章视图模型
    /// </summary>
    public class ArticleVM: IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [StringLength(100, ErrorMessage = "你输入的文章标题超过了100字符。")]
        [Required(ErrorMessage = "文章标题不能为空值。")]
        public string Name { get; set; }

        #region 这两个属性在系统业务数据处理中，有系统自己处理
        public string Description { get; set; }
        public string SortCode { get; set; }
        #endregion

        [StringLength(250, ErrorMessage = "你输入的文章副标题超过了250字符。")]
        public string ArticleSecondTitle { get; set; }            // 副标题
        [StringLength(250, ErrorMessage = "你输入的文章来源信息超过了250字符。")]
        public string ArticleSource { get; set; }                 // 文章来源
        public string ArticleContent { get; set; }                // 内容

        public string PublishDate { get; set; }
        public int UpVoteNumber { get; set; }

        public BusinessVideoVM Video { get; set; }

        public List<BusinessFileVM> FileCollection { get; set; }

        [Display(Name = "归属专栏")]
        public List<string> TopicItemIdCollection { get; set; }        
        public List<PlainFacadeItem> TopicItemCollection { get; set; }

        [Display(Name = "归属类别")]
        public List<string> TypeItemIdCollection { get; set; }
        public List<SelfReferentialItem> TypeItemCollection { get; set; }

        public string SaveStatus { get; set; }
        public string TopicItemId { get; set; }

        public string CreateUserName { get; set; }
        public string CategoryName { get; set; }
    }
}
