﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ELearning.Entities.Tools;
using ELearning.UserAndRole;

namespace ELearning.Entities.News
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }                          // 标题
        [StringLength(500)]
        public string Description { get; set; }                   // 文章摘要信息（存储时，如果不填写这部分内容，则系统提取正文前面若干数量的字符） 
        [StringLength(150)]
        public string SortCode { get; set; }                      // 文章编码（自动处理）

        [StringLength(200)]
        public string ArticleSecondTitle { get; set; }            // 副标题
        [StringLength(250)]
        public string ArticleSource { get; set; }                 // 文章来源
        public string ArticleContent { get; set; }                // 内容
        public DateTime CreateDate { get; set; }                  // 创建日期
        public DateTime UpdateDate { get; set; }                  // 更新日期
        public DateTime PublishDate { get; set; }                 // 发表日期
        public DateTime CloseDate { get; set; }                   // 关闭日期
        public DateTime OpenDate { get; set; }                    // 公开日期
        public bool IsPassed { get; set; }                        // 是否发布
        public bool IsPublishedByHtml { get; set; }               // 是否生成静态 Html 页面
        public bool IsOriented { get; set; }                      // 是否定向发布，非定向类消息可以向全体人员发布

        public Guid RelevanceObjectID { get; set; }               // 关联业务对象ID，通常用于处理当某个具体的业务对象状态发生变化的时候，产生页面文件相关的对象的ID
        public ArticleSourceTypeEnum SourceType { get; set; }     // 文章來源的類型
        public ArticleStatusEnum ArticleStatus { get; set; }      // 文章处理状态
        public Int32 UpVoteNumber { get; set; }                   // 点赞数

        public virtual ApplicationUser CreatorUser { get; set; }  // 创建人

        public Article()
        {
            Id          = Guid.NewGuid();
            Name = Description = ArticleSecondTitle = ArticleSource = "";
            IsOriented  = false;
            CreateDate  = DateTime.Now;
            PublishDate = DateTime.Now;
            OpenDate    = DateTime.Now;
            CloseDate   = DateTime.Now;
            UpdateDate  = DateTime.Now;
            SortCode    = UtilitiesForEntity.SortCodeByDefaultDateTime<Article>();
        }
    }
}
