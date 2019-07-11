using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    public class ArticleInTopic:IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Article MasterArticle { get; set; }
        public virtual ArticleTopic ArticleTopic { get; set; }

        public ArticleInTopic()
        {
            Id = Guid.NewGuid();
        }
    }
}
