using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.Organization
{
    /// <summary>
    /// 学生视图模型
    /// </summary>
    public class StudentVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "姓名不能为空值。")]
        [Display(Name = "姓名")]
        [StringLength(10, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        public string SortCode { get; set; }  // 系统自动处理不需要在页面处理

        [Required(ErrorMessage = "学号不能为空值。")]
        [Display(Name = "学号")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string EmployeeCode { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "出生日期是必须选择的。")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string Birthday { get; set; }

        [Display(Name = "固定电话")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "移动电话不能为空值。")]
        [Display(Name = "移动电话")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制6个字符的长度。")]
        public string MobileNumber { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "电子邮件数据是必须的。")]
        [EmailAddress(ErrorMessage = "请输入合法的电子邮件地址。")]
        public string Email { get; set; }

        [Display(Name = "身份证件编号")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public string CredentialsCode { get; set; }

        [Display(Name = "联系地址")]
        [StringLength(250, ErrorMessage = "你输入的数据超出限制250个字符的长度。")]
        public string Address { get; set; }

        [Display(Name = "入学日期")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string CreateDateTime { get; set; }

        [Display(Name = "毕业日期")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string ExpiredDateTime { get; set; }

        [Display(Name = "归属班级")]
        [Required(ErrorMessage = "班级数据是必须选择的。")]
        public string GradeAndClassId { get; set; }
        public string GradeAndClassName { get; set; }
        public List<SelfReferentialItem> GradeAndClassItemCollection { get; set; }

        public string AvatarPath { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public StudentVM() {}

    }
}
