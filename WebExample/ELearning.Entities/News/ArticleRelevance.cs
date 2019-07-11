using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 关联文章定义
    /// </summary>
    public class ArticleRelevance:IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Article MasterArticle { get; set; }
        public virtual Article RelevanceArticle { get; set; }

        public ArticleRelevance()
        {
            Id = Guid.NewGuid();
        }
    }
}
