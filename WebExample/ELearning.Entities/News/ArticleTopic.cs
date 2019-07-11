using ELearning.Entities.Common;
using ELearning.Entities.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章关联主题
    /// </summary>
    public class ArticleTopic:Entity
    {
        public virtual BusinessImage TopicImage { get; set; }

        public ArticleTopic()
        {
            this.Id = Guid.NewGuid();
            this.Name = this.Description = "";
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleTopic>();
        }
    }
}
