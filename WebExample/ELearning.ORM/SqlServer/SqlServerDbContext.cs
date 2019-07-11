using ELearning.Entities.Common;
using ELearning.Entities.News;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ORM.SqlServer
{
    public class SqlServerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }

        #region 公共对象相关
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }
        #endregion

        #region 用户与角色相关
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion

        #region 新闻信息数据模型映射
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleInTopic> ArticleInTopics { get; set; }
        public DbSet<ArticleInType> ArticleInTypes { get; set; }
        public DbSet<ArticleRelevance> ArticleRelevances { get; set; }
        public DbSet<ArticleRelevanceTag> ArticleRelevanceTags { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleWithFile> ArticleWithFiles { get; set; }
        public DbSet<ArticleWithImage> ArticleWithImages { get; set; }
        public DbSet<ArticleWithVideo> ArticleWithVideos { get; set; }
        #endregion

        #region 组织机构与人员管理数据处理
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organ> Organs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<GradeAndClass> GradeAndClasses { get; set; }
        #endregion

        #region 课程内容相关
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseItem> CourseItems { get; set; }
        public DbSet<CourseItemContent> CourseItemContents { get; set; }
        public DbSet<CourseWithRoles> CourseWithRoles { get; set; }
        public DbSet<CourseWithUsers> CourseWithUsers { get; set; }
        public DbSet<CourseItemContentWithFiles> CourseItemContentWithFileses { get; set; }
        public DbSet<FilesInCourseItemContent> FilesInCourseItemContents { get; set; }
        public DbSet<ImagesInCourseItemContent> ImagesInCourseItemContents { get; set; }
        public DbSet<ViedosInCourseItemContent> ViedosInCourseItemContents { get; set; }
        #endregion

        /// <summary>
        /// 如果不需要 DbSet<T> 所定义的属性名称作为数据库表的名称，可以在下面的位置自己重新定义
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
