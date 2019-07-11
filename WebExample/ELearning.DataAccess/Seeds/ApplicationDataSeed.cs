using ELearning.Entities.Organization;
using ELearning.ORM.SqlServer;
using ELearning.UserAndRole;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.DataAccess.Seeds
{
    public static class ApplicationDataSeed
    {
        static SqlServerDbContext _dbContext;

        public static void ForEntities(SqlServerDbContext context)
        {
            _dbContext = context;
            _ForOrganAndJobTitle();
            _ForDepartmentAndEmployeeAndStudent();
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public static async Task ForRolesAndUsers(RoleManager<ApplicationRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            #region 创建角色
            var role1 = new ApplicationRole()
            {
                Name = "普通注册用户",
                DisplayName = "普通注册用户",
                Description = "具备普通注册用户数据处理的用户组。",
                SortCode = "00001",
                ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户,
                IsDefaultRole = true
            };

            var role2 = new ApplicationRole() { Name = "新闻信息发布用户", DisplayName = "新闻信息发布用户", Description = "具备新闻信息发布数据权限的用户组。", SortCode = "00002", ApplicationRoleType = ApplicationRoleTypeEnum.适用于前台业务数据如作者之类的人员, IsDefaultRole = true };
            var role3 = new ApplicationRole() { Name = "教务管理人员用户", DisplayName = "教务管理人员用户", Description = "具备教务管理人员数据权限（教学班创建、学员数据维护等）用户组。", SortCode = "00003", ApplicationRoleType = ApplicationRoleTypeEnum.适用于后台业务数据维护人员, IsDefaultRole = true };
            var role4 = new ApplicationRole() { Name = "教师用户", DisplayName = "教师用户", Description = "具备教师用户处理和维护课程，组织教学活动等数据管理权限的用户组。", SortCode = "00004", ApplicationRoleType = ApplicationRoleTypeEnum.适用于教师, IsDefaultRole = true };
            var role5 = new ApplicationRole() { Name = "教学班级学员用户", DisplayName = "教学班级学员用户", Description = "具备普通学生的用户数据维护的用户组。", SortCode = "00005", ApplicationRoleType = ApplicationRoleTypeEnum.适用于教学班级学员, IsDefaultRole = true };
            var role6 = new ApplicationRole() { Name = "管理员用户组", DisplayName = "管理员用户组", Description = "具备全部权限的用户组", SortCode = "00005", ApplicationRoleType = ApplicationRoleTypeEnum.适用于系统管理人员, IsDefaultRole = true };

            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role3);
            await roleManager.CreateAsync(role4);
            await roleManager.CreateAsync(role5);
            await roleManager.CreateAsync(role6);
            #endregion

            #region 创建普通用户
            for (int i = 0; i < 200; i++)
            {
                var counterString = i.ToString();
                if (i < 10)
                    counterString = "00" + i.ToString();
                if (i >= 10 && i < 100)
                    counterString = "0" + i.ToString();

                var normalUser = new ApplicationUser()
                {
                    UserName = "normal" + counterString ,
                    FirstName = "何",
                    LastName = "理" + counterString,
                    ChineseFullName = "何理"+counterString,
                    MobileNumber = "13988888888",
                    Email = "normal" + counterString+"@hotmail.com",
                    LockoutEnabled =false
                };

                await userManager.CreateAsync(normalUser, "123@Abc");
                await userManager.AddToRoleAsync(normalUser, "普通注册用户");
            }
            #endregion

            #region 创建系统管理员用户
            var systemAdministrator = new ApplicationUser()
            {
                UserName = "admin",
                FirstName = "李",
                LastName = "响",
                ChineseFullName = "李响",
                MobileNumber = "13617808232",
                Email = "admin@hotmail.com",
                IsDefaultUser = true
            };

            await userManager.CreateAsync(systemAdministrator, "1234@Abcd");
            await userManager.AddToRoleAsync(systemAdministrator, "管理员用户组");
            #endregion

            #region 创建信息发布用户
            var informationIssuer = new ApplicationUser()
            {
                UserName = "issuer",
                FirstName = "王",
                LastName = "信",
                ChineseFullName = "王信",
                MobileNumber = "13617808232",
                Email = "issuer@hotmail.com"
            };

            await userManager.CreateAsync(informationIssuer, "123@Abc");
            await userManager.AddToRoleAsync(informationIssuer, "新闻信息发布用户");
            #endregion

            #region 创建教务管理员用户
            var educationalAdministrators = new ApplicationUser()
            {
                UserName = "fox",
                FirstName = "黄",
                LastName = "东林",
                ChineseFullName = "黄东林",
                MobileNumber = "13617808232",
                Email = "huangdl@outlook.com"
            };

            await userManager.CreateAsync(educationalAdministrators, "123@Abc");
            await userManager.AddToRoleAsync(educationalAdministrators, "教务管理人员用户");
            #endregion

            #region 创建教师用户
            var teacher = new ApplicationUser()
            {
                UserName = "teacher",
                FirstName = "孔",
                LastName = "夫子",
                ChineseFullName = "孔夫子",
                MobileNumber = "13617808232",
                Email = "teacher@hotmail.com"
            };

            await userManager.CreateAsync(teacher, "123@Abc");
            await userManager.AddToRoleAsync(teacher, "教师用户");
            #endregion

            #region 创建教学班级学生用户
            var student = new ApplicationUser()
            {
                UserName = "student",
                FirstName = "端木",
                LastName = "子贡",
                ChineseFullName = "端木子贡",
                MobileNumber = "13617808232",
                Email = "student@hotmail.com"
            };          

            await userManager.CreateAsync(student, "123@Abc");
            await userManager.AddToRoleAsync(student, "教学班级学员用户");
            #endregion
        }

        /// <summary>
        /// 组织与工作标题
        /// </summary>
        private static void _ForOrganAndJobTitle()
        {
            if (!_dbContext.Organs.Any())
            {
                var o1 = new Organ() { Name = "内部组织", Description = "企业内部部门单位", SortCode = "001" };
                var o2 = new Organ() { Name = "外部组织", Description = "企业外部单位", SortCode = "002" };

                _dbContext.Organs.Add(o1);
                _dbContext.Organs.Add(o2);
                _dbContext.SaveChanges();

                if (_dbContext.JobTitles.Any())
                    return;

                var j1 = new JobTitle() { Name = "总经理", Description = "负责企业全面工作", SortCode = "001", JobTitleType = JobTitleTypeEnum.主担 };
                var j2 = new JobTitle() { Name = "副总经理", Description = "分管企业相关工作", SortCode = "002", JobTitleType = JobTitleTypeEnum.辅担 };
                var j3 = new JobTitle() { Name = "部门经理", Description = "负责企业全面工作", SortCode = "003", JobTitleType = JobTitleTypeEnum.主担 };
                var j4 = new JobTitle() { Name = "部门副经理", Description = "分管企业相关工作", SortCode = "004", JobTitleType = JobTitleTypeEnum.辅担 };
                var j5 = new JobTitle() { Name = "技术经理", Description = "负责企业全面工作", SortCode = "005", JobTitleType = JobTitleTypeEnum.普通 };
                var j6 = new JobTitle() { Name = "普通员工", Description = "分管企业相关工作", SortCode = "006", JobTitleType = JobTitleTypeEnum.普通 };

                _dbContext.JobTitles.AddRange(j1, j2, j3, j4, j5, j6);
                _dbContext.SaveChanges();
            }

        }

        private static void _ForDepartmentAndEmployeeAndStudent()
        {
            if (!_dbContext.Departments.Any())
            {
                var o1 = _dbContext.Organs.Where(x => x.Name == "内部组织").FirstOrDefault();
                var o2 = _dbContext.Organs.Where(x => x.Name == "外部组织").FirstOrDefault();


                var dept01 = new Department() { Name = "数学与信息工程学院", Description = "", SortCode = "01", DepartmentType = DepartmentTypeEnum.二级部门, Organization = o1 };
                var dept02 = new Department() { Name = "物理与材料工程学院", Description = "", SortCode = "02", DepartmentType = DepartmentTypeEnum.二级部门, Organization = o1 };
                var dept03 = new Department() { Name = "教务处", Description = "", SortCode = "03", DepartmentType = DepartmentTypeEnum.总部部门, Organization = o1 };
                var dept04 = new Department() { Name = "攀登数字工作室", Description = "", SortCode = "04", DepartmentType = DepartmentTypeEnum.总部部门, Organization = o2 };
                var dept0401 = new Department() { Name = "客户响应服务组", Description = "", SortCode = "0401", DepartmentType = DepartmentTypeEnum.二级部门 };
                var dept0402 = new Department() { Name = "客户需求分析组", Description = "", SortCode = "0402", DepartmentType = DepartmentTypeEnum.二级部门 };
                var dept0403 = new Department() { Name = "应用设计开发组", Description = "", SortCode = "0403", DepartmentType = DepartmentTypeEnum.二级部门 };
                var dept05 = new Department() { Name = "神华软件技术公司", Description = "", SortCode = "05", DepartmentType = DepartmentTypeEnum.二级部门, Organization = o2 };
                var dept06 = new Department() { Name = "2019年级学生", Description = "", SortCode = "06", DepartmentType = DepartmentTypeEnum.教学班级, Organization = o1 };
                var dept0601 = new Department() { Name = "2019级01班", Description = "", SortCode = "0601", DepartmentType = DepartmentTypeEnum.教学班级 };
                var dept0602 = new Department() { Name = "2019级02班", Description = "", SortCode = "0602", DepartmentType = DepartmentTypeEnum.教学班级 };

                dept01.ParentDepartment = dept01;
                dept02.ParentDepartment = dept02;
                dept03.ParentDepartment = dept03;
                dept04.ParentDepartment = dept04;

                dept0401.ParentDepartment = dept04;
                dept0402.ParentDepartment = dept04;
                dept0403.ParentDepartment = dept04;
                dept05.ParentDepartment = dept05;
                dept06.ParentDepartment = dept06;
                dept0601.ParentDepartment = dept06;
                dept0602.ParentDepartment = dept06;

                var depts = new List<Department>() { dept01, dept02, dept03, dept04, dept0401, dept0402, dept0403, dept05, dept06, dept0601, dept0602 };
                foreach (var item in depts)
                    _dbContext.Departments.Add(item);

                _dbContext.SaveChanges();
            }


            if (!_dbContext.Employees.Any())
            {
                var dept = _dbContext.Departments.FirstOrDefault();
                var persons = new List<Employee>()
                {
                    new Employee() { Name="刘虎军", EmployeeCode="20190001", CredentialsCode="452230198210010011", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介", Department=dept },
                    new Employee() { Name="魏小花", EmployeeCode="20190002", CredentialsCode="452230198210010011", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="李文慧", EmployeeCode="20190003", CredentialsCode="452230198210010011", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="张江的", EmployeeCode="20190004", CredentialsCode="452230198210010011", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="萧可君", EmployeeCode="20190005", CredentialsCode="452230198210010011", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="魏铜生", EmployeeCode="20190006", CredentialsCode="452230198210010011", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="刘德华", EmployeeCode="20190007", CredentialsCode="452230198210010011", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="魏星亮", EmployeeCode="20190008", CredentialsCode="452230198210010011", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="潘家富", EmployeeCode="20190009", CredentialsCode="452230198210010011", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="黎温德", EmployeeCode="20190010", CredentialsCode="452230198210010011", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介",Department=dept },
                    new Employee() { Name="邓淇升", EmployeeCode="20190011", CredentialsCode="452230198210010011", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" ,Department=dept},
                    new Employee() { Name="谭檀檀", EmployeeCode="20190012", CredentialsCode="452230198210010011", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" ,Department=dept},
                    new Employee() { Name="陈慧琳", EmployeeCode="20190013", CredentialsCode="452230198210010011", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介" ,Department=dept},
                    new Employee() { Name="祁华钰", EmployeeCode="20190014", CredentialsCode="452230198210010011", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介" ,Department=dept},
                    new Employee() { Name="胡德财", EmployeeCode="20190015", CredentialsCode="452230198210010011", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介" ,Department=dept},
                    new Employee() { Name="吴富贵", EmployeeCode="20190016", CredentialsCode="452230198210010011", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介" ,Department=dept}
                };

                foreach (var person in persons)
                {
                    _dbContext.Employees.Add(person);
                }
                _dbContext.SaveChanges();
            }

            if (!_dbContext.GradeAndClasses.Any())
            {
                var gc001 = new GradeAndClass() { Name = "LZ2019-2020", Description = "", SortCode = "01" };
                gc001.ParentDepartment = gc001;
                var gc00101 = new GradeAndClass() { Name = "高一3班", Description = "", SortCode = "0101", ParentDepartment = gc001 };
                var gc00102 = new GradeAndClass() { Name = "高一2班", Description = "", SortCode = "0102", ParentDepartment = gc001 };
                _dbContext.GradeAndClasses.AddRange(new GradeAndClass[] { gc001, gc00101, gc00102 });
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Students.Any())
            {
                var grade = _dbContext.GradeAndClasses.FirstOrDefault(); ;
                var students = new List<Student>()
                {
                    new Student() { Name="黄虎军", EmployeeCode="201908001", CredentialsCode="452230198210010011", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介" },
                    new Student() { Name="河小花", EmployeeCode="201908002", CredentialsCode="452230198210010012", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介"},
                    new Student() { Name="陆文慧", EmployeeCode="201908003", CredentialsCode="452230198210010011", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介"},
                    new Student() { Name="刘江的", EmployeeCode="201908004", CredentialsCode="452230198210010011", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介"},
                    new Student() { Name="韦可君", EmployeeCode="201908005", CredentialsCode="452230198210010011", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介"},
                    new Student() { Name="韦铜生", EmployeeCode="201908006", CredentialsCode="452230198210010011", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介"},
                    new Student() { Name="韦德华", EmployeeCode="201908007", CredentialsCode="452230198210010011", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介"},
                    new Student() { Name="蒋星亮", EmployeeCode="201908008", CredentialsCode="452230198210010011", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介"},
                    new Student() { Name="蒋家富", EmployeeCode="201908009", CredentialsCode="452230198210010011", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介"},
                    new Student() { Name="张温德", EmployeeCode="201908010", CredentialsCode="452230198210010011", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介"},
                    new Student() { Name="张淇升", EmployeeCode="201908011", CredentialsCode="452230198210010011", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介"},
                    new Student() { Name="秦冠希", EmployeeCode="201908011", CredentialsCode="452230198210010011", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介"},
                    new Student() { Name="刘慧琳", EmployeeCode="201908012", CredentialsCode="452230198210010011", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介"},
                    new Student() { Name="周华钰", EmployeeCode="201908013", CredentialsCode="452230198210010011", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介"},
                    new Student() { Name="钱德财", EmployeeCode="201908014", CredentialsCode="452230198210010011", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介"},
                    new Student() { Name="孙富贵", EmployeeCode="201908015", CredentialsCode="452230198210010011", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介"},
                    new Student() { Name="韦虎军", EmployeeCode="201908016", CredentialsCode="452230198210010011", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介"},
                    new Student() { Name="韦小花", EmployeeCode="201908017", CredentialsCode="452230198210010011", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介"},
                    new Student() { Name="韦文慧", EmployeeCode="201908018", CredentialsCode="452230198210010011", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介"},
                    new Student() { Name="韦江的", EmployeeCode="201908019", CredentialsCode="452230198210010011", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介"},
                    new Student() { Name="温可君", EmployeeCode="201908020", CredentialsCode="452230198210010011", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介"},
                    new Student() { Name="温铜生", EmployeeCode="201908021", CredentialsCode="452230198210010011", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介"},
                    new Student() { Name="温德华", EmployeeCode="201908022", CredentialsCode="452230198210010011", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介"},
                    new Student() { Name="温星亮", EmployeeCode="201908023", CredentialsCode="452230198210010011", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介"},
                    new Student() { Name="温家富", EmployeeCode="201908024", CredentialsCode="452230198210010011", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介"},
                    new Student() { Name="覃温德", EmployeeCode="201908025", CredentialsCode="452230198210010011", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介"},
                    new Student() { Name="覃淇升", EmployeeCode="201908026", CredentialsCode="452230198210010011", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" },
                    new Student() { Name="覃冠希", EmployeeCode="201908027", CredentialsCode="452230198210010011", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" },
                    new Student() { Name="覃慧琳", EmployeeCode="201908028", CredentialsCode="452230198210010011", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介"},
                    new Student() { Name="覃华钰", EmployeeCode="201908029", CredentialsCode="452230198210010011", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介"},
                    new Student() { Name="覃德财", EmployeeCode="201908030", CredentialsCode="452230198210010011", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介"},
                    new Student() { Name="覃富贵", EmployeeCode="201908031", CredentialsCode="452230198210010011", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介"}
                };

                foreach (var student in students)
                {
                    student.GradeAndClass = grade;
                    _dbContext.Students.Add(student);
                }
                _dbContext.SaveChanges();
            }


        }
    }
}
