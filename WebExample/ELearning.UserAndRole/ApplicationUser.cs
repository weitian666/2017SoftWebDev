using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ELearning.UserAndRole
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [StringLength(100)]
        public string FirstName { get; set; }       // 姓氏
        [StringLength(100)]
        public string LastName { get; set; }        // 名字
        [StringLength(100)]
        public string ChineseFullName { get; set; } // 中文全名
        [StringLength(50)]
        public string MobileNumber { get; set; }    // 移动电话，父类中的 PhoneNumber 用于固定电话     
        public string AvatarPath { get; set; }      // 人员头像路径
        public bool IsDefaultUser { get; set; }     // 是否是缺省的系统用户，如果是，将不能对用户资料进行编辑

        public ApplicationUser() : base()
        {
            this.Id = Guid.NewGuid();
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid();
            this.UserName = userName;
        }
    }
}
