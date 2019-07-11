using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.Organization
{
    /// <summary>
    /// 员工视图
    /// </summary>
    public class EmployeeVM : IEntityViewModel
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

        [Required(ErrorMessage = "工号不能为空值。")]
        [Display(Name = "工号")]
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
        public string Address { get; set; }                       // 联系地址

        [Display(Name = "入职日期")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string CreateDateTime { get; set; }

        [Display(Name = "离职日期")]
        [DataType(DataType.DateTime, ErrorMessage = "日期时间数据格式错误。")]
        public string ExpiredDateTime { get; set; }

        [Required(ErrorMessage = "归属部门是必须的。")]
        [Display(Name = "归属部门")]
        public string ParentDepartmentId { get; set; }
        public string ParentDepartmentName { get; set; }
        public List<SelfReferentialItem> ParentDepartmentItemCollection { get; set; }

        [Display(Name = "职位")]
        public string JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public List<PlainFacadeItem> JobTitleItemCollection { get; set; }

        public string AvatarPath { get; set; }                       // 头像
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public EmployeeVM()
        {
            this.Id = Guid.NewGuid();
        }

    }
}
