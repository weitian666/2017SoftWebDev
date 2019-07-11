using ELearning.Entities.Common;
using ELearning.Entities.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章关联的视频文件
    /// </summary>
    public class ArticleWithVideo : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }         // 视频标题
        [StringLength(500)]
        public string Description { get; set; }  // 视频描述
        [StringLength(150)]
        public string SortCode { get; set; }     // 视频编码

        public virtual Article MasterArticle { get; set; }
        public virtual BusinessVideo Video { get; set; }

        public ArticleWithVideo()
        {
            Id = Guid.NewGuid();
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleWithVideo>();
        }
    }
}
