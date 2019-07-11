using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using ELearning.Web.Middlewares;
using ELearning.ORM.SqlServer;
using ELearning.UserAndRole;
using ELearning.DataAccess;
using ELearning.Entities.Organization;
using ELearning.Entities.Common;
using ELearning.Entities.TeachingCourse;
using Microsoft.AspNetCore.Authorization;
using ELearning.Web.SecurityService.AuthorizationRequirements;
using ELearning.Web.SecurityService.AuthorizationRequirementHandlers;
using ELearning.Entities.News;

namespace ELearning.Web
{
    /// <summary>
    /// APP 启动配置文件：
    /// 1. 配置当前应用系统所需要的所有的 服务；
    /// 2. 定义 HTTP 访问请求处理 管道。
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务：
        /// 1.通过代码为当前应用系统容器添加和配置所需要的服务，这里的外部服务意指将用于系统的软件组件，
        ///   例如 EF Core 的 Context 对象就是一个服务。
        /// 2.运行时调用。  
        /// </summary>
        /// <param name="services">注入的服务集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置使用 Sql Server 的 EF Context
            services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ELearningConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<SqlServerDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters ="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                options.LoginPath = "/Account/Logon";
                options.AccessDeniedPath = "/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });

            // 限制表单上传字节大小，600 兆
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = 1000000000;   
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region 配置依赖注入映射服务
            services.AddScoped<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            services.AddScoped<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddScoped<IEntityRepository<BusinessVideo>, EntityRepository<BusinessVideo>>();

            services.AddScoped<IEntityRepository<Department>, EntityRepository<Department>>();
            services.AddScoped<IEntityRepository<Organ>, EntityRepository<Organ>>();
            services.AddScoped<IEntityRepository<Employee>, EntityRepository<Employee>>();
            services.AddScoped<IEntityRepository<Student>, EntityRepository<Student>>();
            services.AddScoped<IEntityRepository<JobTitle>, EntityRepository<JobTitle>>();
            services.AddScoped<IEntityRepository<GradeAndClass>, EntityRepository<GradeAndClass>>();

            services.AddScoped<IEntityRepository<Course>, EntityRepository<Course>>();
            services.AddScoped<IEntityRepository<CourseItem>, EntityRepository<CourseItem>>();
            services.AddScoped<IEntityRepository<CourseItemContent>, EntityRepository<CourseItemContent>>();
            services.AddScoped<IEntityRepository<CourseItemContentWithFiles>, EntityRepository<CourseItemContentWithFiles>>();
            services.AddScoped<IEntityRepository<CourseWithRoles>, EntityRepository<CourseWithRoles>>();
            services.AddScoped<IEntityRepository<CourseWithUsers>, EntityRepository<CourseWithUsers>>();

            services.AddScoped<IEntityRepository<FilesInCourseItemContent>, EntityRepository<FilesInCourseItemContent>>();
            services.AddScoped<IEntityRepository<ImagesInCourseItemContent>, EntityRepository<ImagesInCourseItemContent>>();
            services.AddScoped<IEntityRepository<ViedosInCourseItemContent>, EntityRepository<ViedosInCourseItemContent>>();

            #region 文章管理部分
            services.AddScoped<IEntityRepository<Article>, EntityRepository<Article>>();
            services.AddScoped<IEntityRepository<ArticleComment>, EntityRepository<ArticleComment>>();
            services.AddScoped<IEntityRepository<ArticleCommentTag>, EntityRepository<ArticleCommentTag>>();
            services.AddScoped<IEntityRepository<ArticleInTopic>, EntityRepository<ArticleInTopic>>();
            services.AddScoped<IEntityRepository<ArticleInType>, EntityRepository<ArticleInType>>();
            services.AddScoped<IEntityRepository<ArticleInType>, EntityRepository<ArticleInType>>();
            services.AddScoped<IEntityRepository<ArticleRelevance>, EntityRepository<ArticleRelevance>>();
            services.AddScoped<IEntityRepository<ArticleRelevanceTag>, EntityRepository<ArticleRelevanceTag>>();
            services.AddScoped<IEntityRepository<ArticleTopic>, EntityRepository<ArticleTopic>>();
            services.AddScoped<IEntityRepository<ArticleType>, EntityRepository<ArticleType>>();
            services.AddScoped<IEntityRepository<ArticleWithFile>, EntityRepository<ArticleWithFile>>();
            services.AddScoped<IEntityRepository<ArticleWithImage>, EntityRepository<ArticleWithImage>>();
            services.AddScoped<IEntityRepository<ArticleWithVideo>, EntityRepository<ArticleWithVideo>>();
            #endregion


            #endregion

            #region 配置定制的授权策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CourseEdit", policy => policy.Requirements.Add(new CourseCreatorRequirement()));
                options.AddPolicy("AdminPermission", policy => policy.Requirements.Add(new AdminAreaRequirement(ApplicationRoleTypeEnum.适用于系统管理人员)));
            });
            services.AddSingleton<IAuthorizationHandler, CourseCreatorAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CourseAdministratorAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CourseAuthorizationCrudHandler>();


            #endregion
        }

        /// <summary>
        /// 配置 HTTP 访问请求处理管道：
        /// 1.通过代码配置已经添加的访问请求处理管道，管道是由一系列的称为 中间件 的组件组成。例如
        ///   处理访问静态文件请求的中间件，将 HTTP 访问重新定向到 HTTPS 的中间件。
        /// 2.每个中间件在一个单一的 HttpContext 执行时，不管是调用管道中的下一个中间件，还是终
        ///   止访问请求，都是异步方式执行操作的。
        /// 3.运行时调用
        /// </summary>
        /// <param name="app">当前运行的 APP 实例。</param>
        /// <param name="env">APP 运行驻留环境。</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();  // 认证用的中间件

            app.UseMvc(routes =>
            {
                // 添加区域 Admin 路由
                routes.MapAreaRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    template: "Admin/{controller=Home}/{action=Index}/{id?}");

                // 添加区域 TeacherDesktop 路由
                routes.MapAreaRoute(
                    name: "TeacherDesktopArea",
                    areaName: "TeacherDesktop",
                    template: "TeacherDesktop/{controller=Home}/{action=Index}/{id?}");

                // 添加区域 TeacherDesktop 路由
                routes.MapAreaRoute(
                    name: "StudentDesktopArea",
                    areaName: "StudentDesktop",
                    template: "StudentDesktop/{controller=Home}/{action=Index}/{id?}");

                // 缺省路由
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
