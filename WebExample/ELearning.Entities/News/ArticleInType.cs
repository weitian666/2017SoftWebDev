using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    public class ArticleInType:IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Article MasterArticle { get; set; }
        public virtual ArticleType ArticleType { get; set; }

        public ArticleInType()
        {
            Id = Guid.NewGuid();
        }
    }
}
