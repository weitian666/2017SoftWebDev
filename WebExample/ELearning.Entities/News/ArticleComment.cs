using ELearning.Entities.Tools;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章反馈意见
    /// </summary>
    public class ArticleComment:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }                               // 评论人别名 
        [StringLength(200)]
        public string Title { get; set; }                              // 标题
        [StringLength(10000)]
        public string Description { get; set; }                        // 评论内容
        [StringLength(150)]
        public string SortCode { get; set; }
        public DateTime CommentDate { get; set; }                     // 评论发表时间

        [ForeignKey("ParentCommentID")]
        public virtual ArticleComment ParentComment { get; set; }      // 评论的上一层，如果是与自身一样，则认为是开头话题评论
        public virtual Article MasterArticle { get; set; }             // 关联的文章
        public virtual ApplicationUser CommentWritor { get; set; }     // 评论人

        public ArticleComment()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<ArticleComment>();
        }
    }
}
