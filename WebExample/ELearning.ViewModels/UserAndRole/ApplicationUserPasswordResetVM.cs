using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.UserAndRole
{
    /// <summary>
    /// 重置用户密码视图模型
    /// </summary>
    public class ApplicationUserPasswordResetVM
    {
        public Guid UserId { get; set; }

        [Display(Name = "新密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码是必须的。")]
        [RegularExpression(@"((^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,})$)", ErrorMessage = "密码至少8个字符，至少1个小写字母，一个大写字母，1个数字和1个特殊字符！")]
        public string Password { get; set; }

        [Display(Name = "重复密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码必须一致")]
        public string PasswordComfirm { get; set; }

        public string Code { get; set; }
        public string ResetStatus { get; set; }


    }
}
