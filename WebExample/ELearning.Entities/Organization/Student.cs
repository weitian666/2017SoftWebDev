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
    /// 学生
    /// </summary>
    public class Student:IEntity
    {
        [Key]
        public Guid Id { get; set; }            
        [StringLength(100)]
        public string Name { get; set; }        
        [StringLength(500)]
        public string Description { get; set; } 
        [StringLength(100)]
        public string SortCode { get; set; }
        public DateTime CreateDateTime { get; set; }              // 入学日期
        public DateTime ExpiredDateTime { get; set; }             // 毕业日期
        [StringLength(50)]
        public string EmployeeCode { get; set; }                  // 学号
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
        [StringLength(250)]
        public string Address { get; set; }                       // 联系地址
        public DateTime UpdateTime { get; set; }                  // 信息更新时间
        public string AvatarPath { get; set; }                    // 人员头像路径

        public virtual GradeAndClass GradeAndClass { get; set; }  // 归属班级
        public virtual ApplicationUser User { get; set; }         // 关联用户

        public Student()
        {
            this.Id = Guid.NewGuid();
            UpdateTime = CreateDateTime = Birthday = ExpiredDateTime = DateTime.Now;
            Name = Description = EmployeeCode = TelephoneNumber = Mobile = Email = CredentialsCode = Address = AvatarPath = "";
            SortCode = UtilitiesForEntity.SortCodeByDefaultDateTime<Employee>();
        }

    }
}
