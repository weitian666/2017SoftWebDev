using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.Common
{
    public class TestVM
    {
        public Guid ID { get; set; }
        public string OrderNumber { get; set; }   // 列表时做列表序号的标识字符串

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名数据是必须的。")]
        [StringLength(10, ErrorMessage = "字符串长度不能超过 10 个字符。")]
        public string Name { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "电子邮件数据是必须的。")]
        [EmailAddress(ErrorMessage = "请输入合法的电子邮件地址。")]
        public string Email { get; set; }

        [Display(Name = "移动电话")]
        [RegularExpression(@"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "电话号码数据不合规！"),
            Required(ErrorMessage = "移动电话号码数据是必须的。"),
            MaxLength(11, ErrorMessage = "电话号码超过11位数！"),
            MinLength(11, ErrorMessage = "电话号码长度不足11位数！")]
        public string Mobile { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(500, ErrorMessage = "字符串长度不能超过 500 个字符。")]
        public string Description { get; set; }

        [Display(Name = "人员编码")]
        [Required(ErrorMessage = "人员编码数据是必须的。")]
        [StringLength(50, ErrorMessage = "字符串长度不能超过 50 个字符。")]
        public string SortCode { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码是必须的。")]
        [RegularExpression(@"((^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,})$)", ErrorMessage = "密码至少8个字符，至少1个字母，1个数字和1个特殊字符！")]
        public string Password { get; set; }

        [Display(Name = "重复密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码必须一致")]
        public string PasswordComfirm { get; set; }

        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "出生日期是必须选择的。")]
        [DataType(DataType.DateTime,ErrorMessage ="日期时间数据格式错误。")]
        public string Birthday { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "上级领导")]
        [Required(ErrorMessage = "上级领导是必须选择的。")]
        public string LeaderID { get; set; }
        public string LeaderName { get; set; }
        public List<PlainFacadeItem> LeaderItemCollection { get; set; }

        public TestVM() { }

    }
}
