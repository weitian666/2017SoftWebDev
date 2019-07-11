using ELearning.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    public class ArticleRelevanceTag:IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Article MasterArticle { get; set; }
        public virtual ArticleCommentTag ContentTag { get; set; }

        public ArticleRelevanceTag()
        {
            Id = Guid.NewGuid();
        }
    }
}
