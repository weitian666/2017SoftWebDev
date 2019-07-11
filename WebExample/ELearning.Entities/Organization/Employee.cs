using ELearning.Entities.Common;
using ELearning.Entities.Tools;
using ELearning.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Organization
{
    /// <summary>
    /// 员工
    /// </summary>
    public class Employee:IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }                          // 员工姓名
        [StringLength(500)]
        public string Description { get; set; }                   // 简要说明
        [StringLength(100)]
        public string SortCode { get; set; }                      // 系统内部编码
        public DateTime CreateDateTime { get; set; }              // 入职日期
        public DateTime ExpiredDateTime { get; set; }             // 离职日期
        [StringLength(50)]
        public string EmployeeCode { get; set; }                  // 工号
        public bool Sex { get; set; }                             // 性别
        [StringLength(20)]
        public string TelephoneNumber { get; set; }               // 电话号码
        [StringLength(20)]
        public string Mobile { get; set; }                        // 手机号码
        [StringLength(100)]
        public string Email { get; set; }                         // 电子邮箱
        public DateTime Birthday { get; set; }                    // 出生日期
        [StringLength(26)]
        public string CredentialsCode { get; set; }               // 身份证编号
        [StringLength(260)]
        public string Address { get; set; }                       // 联系地址
        public DateTime UpdateTime { get; set; }                  // 信息更新时间
        public string AvatarPath { get; set; }                    // 人员头像路径

        public virtual Department Department { get; set; }        // 所属部门
        public virtual JobTitle JobTitle { get; set; }            // 人员头衔
        public virtual ApplicationUser User { get; set; }         // 关联用户

        public Employee()
        {
            this.Id = Guid.NewGuid();
            UpdateTime = CreateDateTime = Birthday = ExpiredDateTime = DateTime.Now;
            Name = Description = EmployeeCode = TelephoneNumber = Mobile = Email = CredentialsCode = Address = AvatarPath = "";
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<Employee>();
        }

    }
}
