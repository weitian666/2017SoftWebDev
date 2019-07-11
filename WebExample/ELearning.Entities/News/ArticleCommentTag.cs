using ELearning.Entities.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章内容标签定义，在本系统中，标签不允许包含空格等标点符号
    /// </summary>
    public class ArticleCommentTag:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }           // 标签名称
        [StringLength(100)]
        public string Description { get; set; }    // 标签说明
        [StringLength(150)]
        public string SortCode { get; set; }
        public Int32 RefrenceCount { get; set; }   // 标签引用次数

        public ArticleCommentTag()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleCommentTag>();
        }

    }
}
