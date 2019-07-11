using ELearning.Entities.Common;
using ELearning.Entities.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章关联的附件（除了图片、视频等已经规约过类型以外的其他文件）
    /// </summary>
    public class ArticleWithFile:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }         // 文件标题
        [StringLength(500)]
        public string Description { get; set; }  // 文件描述
        [StringLength(150)]
        public string SortCode { get; set; }     // 文件编码

        public virtual Article MasterArticle { get; set; }
        public virtual BusinessFile File { get; set; }

        public ArticleWithFile()
        {
            Id = Guid.NewGuid();
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleWithFile>();
        }
    }
}
