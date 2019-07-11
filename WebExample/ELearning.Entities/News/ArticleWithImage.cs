using ELearning.Entities.Common;
using ELearning.Entities.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章管理的图片
    /// 关于图片置顶处理：如果是置顶图片，则需要在存储时做缩小处理，置顶图片一般在文字的第一自然段下方，
    ///                  其它图片，按照编码次序，在文章末尾排布。 
    /// </summary>
    public class ArticleWithImage:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }         // 图片标题
        [StringLength(500)]
        public string Description { get; set; }  // 图片描述
        [StringLength(150)]
        public string SortCode { get; set; }     // 图片编码

        public bool IsTop { get; set; }          // 是否是置顶图片

        public virtual Article MasterArticle { get; set; }
        public virtual BusinessImage Image { get; set; }

        public ArticleWithImage()
        {
            Id = Guid.NewGuid();
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleWithImage>();
        }
    }
}
