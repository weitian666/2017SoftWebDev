using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearning.ORM.Migrations
{
    public partial class ELearning010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCommentTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    RefrenceCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommentTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 550, nullable: true),
                    SortCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsDefaultRole = table.Column<bool>(nullable: false),
                    ApplicationRoleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    ChineseFullName = table.Column<string>(maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AvatarPath = table.Column<string>(nullable: true),
                    IsDefaultUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    AttachmentTimeUploaded = table.Column<DateTime>(nullable: false),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    UploadPath = table.Column<string>(maxLength: 500, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    UploadFileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 256, nullable: true),
                    UploadedTime = table.Column<DateTime>(nullable: false),
                    UploadPath = table.Column<string>(maxLength: 256, nullable: true),
                    UploadFileSuffix = table.Column<string>(maxLength: 256, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    IsForTitle = table.Column<bool>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    SortCode = table.Column<string>(maxLength: 250, nullable: true),
                    AttachmentTimeUploaded = table.Column<DateTime>(nullable: false),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    UploadPath = table.Column<string>(maxLength: 500, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    UploadFileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseItemContentWithFileses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessFileId = table.Column<Guid>(nullable: false),
                    BusinessImageId = table.Column<Guid>(nullable: false),
                    BusinessVideoId = table.Column<Guid>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemContentWithFileses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    JobTitleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeAndClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    ExpiredDateTime = table.Column<DateTime>(nullable: false),
                    ParentDepartmentId = table.Column<Guid>(nullable: true),
                    ApplicationRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeAndClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeAndClasses_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeAndClasses_GradeAndClasses_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "GradeAndClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    ArticleSecondTitle = table.Column<string>(maxLength: 200, nullable: true),
                    ArticleSource = table.Column<string>(maxLength: 250, nullable: true),
                    ArticleContent = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: false),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    IsPassed = table.Column<bool>(nullable: false),
                    IsPublishedByHtml = table.Column<bool>(nullable: false),
                    IsOriented = table.Column<bool>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    SourceType = table.Column<int>(nullable: false),
                    ArticleStatus = table.Column<int>(nullable: false),
                    UpVoteNumber = table.Column<int>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseItemContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SortCode = table.Column<string>(maxLength: 200, nullable: true),
                    SecondTitle = table.Column<string>(maxLength: 200, nullable: true),
                    HeadContent = table.Column<string>(maxLength: 500, nullable: true),
                    FootContent = table.Column<string>(maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    EditorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseItemContents_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 5000, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    CourseAdministratorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_CourseAdministratorId",
                        column: x => x.CourseAdministratorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    TopicImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTopics_BusinessImages_TopicImageId",
                        column: x => x.TopicImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    ParentTypeId = table.Column<Guid>(nullable: true),
                    ArticleTypeImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_BusinessImages_ArticleTypeImageId",
                        column: x => x.ArticleTypeImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_ArticleTypes_ParentTypeId",
                        column: x => x.ParentTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    DepartmentType = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ParentDepartmentId = table.Column<Guid>(nullable: true),
                    ApplicationRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Organs_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    ExpiredDateTime = table.Column<DateTime>(nullable: false),
                    EmployeeCode = table.Column<string>(maxLength: 50, nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    TelephoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CredentialsCode = table.Column<string>(maxLength: 26, nullable: true),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true),
                    GradeAndClassId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_GradeAndClasses_GradeAndClassId",
                        column: x => x.GradeAndClassId,
                        principalTable: "GradeAndClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 10000, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    ParentCommentID = table.Column<Guid>(nullable: true),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    CommentWritorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComments_AspNetUsers_CommentWritorId",
                        column: x => x.CommentWritorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleComments_ArticleComments_ParentCommentID",
                        column: x => x.ParentCommentID,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleRelevances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    RelevanceArticleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleRelevances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleRelevances_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleRelevances_Articles_RelevanceArticleId",
                        column: x => x.RelevanceArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleRelevanceTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ContentTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleRelevanceTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleRelevanceTags_ArticleCommentTag_ContentTagId",
                        column: x => x.ContentTagId,
                        principalTable: "ArticleCommentTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleRelevanceTags_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWithFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWithFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleWithFiles_BusinessFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "BusinessFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWithFiles_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWithImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    IsTop = table.Column<bool>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWithImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleWithImages_BusinessImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWithImages_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWithVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 150, nullable: true),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    VideoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWithVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleWithVideos_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWithVideos_BusinessVideos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "BusinessVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilesInCourseItemContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseItemContentId = table.Column<Guid>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesInCourseItemContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilesInCourseItemContents_CourseItemContents_CourseItemContentId",
                        column: x => x.CourseItemContentId,
                        principalTable: "CourseItemContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FilesInCourseItemContents_BusinessFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "BusinessFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesInCourseItemContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseItemContentId = table.Column<Guid>(nullable: true),
                    ImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesInCourseItemContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesInCourseItemContents_CourseItemContents_CourseItemContentId",
                        column: x => x.CourseItemContentId,
                        principalTable: "CourseItemContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesInCourseItemContents_BusinessImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ViedosInCourseItemContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseItemContentId = table.Column<Guid>(nullable: true),
                    VideoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViedosInCourseItemContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViedosInCourseItemContents_CourseItemContents_CourseItemContentId",
                        column: x => x.CourseItemContentId,
                        principalTable: "CourseItemContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ViedosInCourseItemContents_BusinessVideos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "BusinessVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    ParentCourseItemId = table.Column<Guid>(nullable: true),
                    CourseId = table.Column<Guid>(nullable: true),
                    CourseItemContentId = table.Column<Guid>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseItems_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_CourseItemContents_CourseItemContentId",
                        column: x => x.CourseItemContentId,
                        principalTable: "CourseItemContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_CourseItems_ParentCourseItemId",
                        column: x => x.ParentCourseItemId,
                        principalTable: "CourseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseWithRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorizationTypeEnum = table.Column<int>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: true),
                    ApplicationRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseWithRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseWithRoles_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseWithRoles_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseWithUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorizationTypeEnum = table.Column<int>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseWithUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseWithUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseWithUsers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleInTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ArticleTopicId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleInTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleInTopics_ArticleTopics_ArticleTopicId",
                        column: x => x.ArticleTopicId,
                        principalTable: "ArticleTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleInTopics_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleInTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ArticleTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleInTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleInTypes_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleInTypes_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    ExpiredDateTime = table.Column<DateTime>(nullable: false),
                    EmployeeCode = table.Column<string>(maxLength: 50, nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    TelephoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CredentialsCode = table.Column<string>(maxLength: 26, nullable: true),
                    Address = table.Column<string>(maxLength: 260, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    AvatarPath = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    JobTitleId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_CommentWritorId",
                table: "ArticleComments",
                column: "CommentWritorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_MasterArticleId",
                table: "ArticleComments",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ParentCommentID",
                table: "ArticleComments",
                column: "ParentCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTopics_ArticleTopicId",
                table: "ArticleInTopics",
                column: "ArticleTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTopics_MasterArticleId",
                table: "ArticleInTopics",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTypes_ArticleTypeId",
                table: "ArticleInTypes",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTypes_MasterArticleId",
                table: "ArticleInTypes",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevances_MasterArticleId",
                table: "ArticleRelevances",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevances_RelevanceArticleId",
                table: "ArticleRelevances",
                column: "RelevanceArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevanceTags_ContentTagId",
                table: "ArticleRelevanceTags",
                column: "ContentTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevanceTags_MasterArticleId",
                table: "ArticleRelevanceTags",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatorUserId",
                table: "Articles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTopics_TopicImageId",
                table: "ArticleTopics",
                column: "TopicImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_ArticleTypeImageId",
                table: "ArticleTypes",
                column: "ArticleTypeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_ParentTypeId",
                table: "ArticleTypes",
                column: "ParentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithFiles_FileId",
                table: "ArticleWithFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithFiles_MasterArticleId",
                table: "ArticleWithFiles",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithImages_ImageId",
                table: "ArticleWithImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithImages_MasterArticleId",
                table: "ArticleWithImages",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithVideos_MasterArticleId",
                table: "ArticleWithVideos",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithVideos_VideoId",
                table: "ArticleWithVideos",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItemContents_EditorId",
                table: "CourseItemContents",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseId",
                table: "CourseItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseItemContentId",
                table: "CourseItems",
                column: "CourseItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CreatorId",
                table: "CourseItems",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_ParentCourseItemId",
                table: "CourseItems",
                column: "ParentCourseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseAdministratorId",
                table: "Courses",
                column: "CourseAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatorId",
                table: "Courses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseWithRoles_ApplicationRoleId",
                table: "CourseWithRoles",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseWithRoles_CourseId",
                table: "CourseWithRoles",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseWithUsers_ApplicationUserId",
                table: "CourseWithUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseWithUsers_CourseId",
                table: "CourseWithUsers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ApplicationRoleId",
                table: "Departments",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitleId",
                table: "Employees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilesInCourseItemContents_CourseItemContentId",
                table: "FilesInCourseItemContents",
                column: "CourseItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_FilesInCourseItemContents_FileId",
                table: "FilesInCourseItemContents",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeAndClasses_ApplicationRoleId",
                table: "GradeAndClasses",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeAndClasses_ParentDepartmentId",
                table: "GradeAndClasses",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesInCourseItemContents_CourseItemContentId",
                table: "ImagesInCourseItemContents",
                column: "CourseItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesInCourseItemContents_ImageId",
                table: "ImagesInCourseItemContents",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GradeAndClassId",
                table: "Students",
                column: "GradeAndClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ViedosInCourseItemContents_CourseItemContentId",
                table: "ViedosInCourseItemContents",
                column: "CourseItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ViedosInCourseItemContents_VideoId",
                table: "ViedosInCourseItemContents",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleComments");

            migrationBuilder.DropTable(
                name: "ArticleInTopics");

            migrationBuilder.DropTable(
                name: "ArticleInTypes");

            migrationBuilder.DropTable(
                name: "ArticleRelevances");

            migrationBuilder.DropTable(
                name: "ArticleRelevanceTags");

            migrationBuilder.DropTable(
                name: "ArticleWithFiles");

            migrationBuilder.DropTable(
                name: "ArticleWithImages");

            migrationBuilder.DropTable(
                name: "ArticleWithVideos");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CourseItemContentWithFileses");

            migrationBuilder.DropTable(
                name: "CourseItems");

            migrationBuilder.DropTable(
                name: "CourseWithRoles");

            migrationBuilder.DropTable(
                name: "CourseWithUsers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "FilesInCourseItemContents");

            migrationBuilder.DropTable(
                name: "ImagesInCourseItemContents");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ViedosInCourseItemContents");

            migrationBuilder.DropTable(
                name: "ArticleTopics");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropTable(
                name: "ArticleCommentTag");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "BusinessFiles");

            migrationBuilder.DropTable(
                name: "GradeAndClasses");

            migrationBuilder.DropTable(
                name: "CourseItemContents");

            migrationBuilder.DropTable(
                name: "BusinessVideos");

            migrationBuilder.DropTable(
                name: "BusinessImages");

            migrationBuilder.DropTable(
                name: "Organs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
