using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.Areas.Admin.Models
{
    /// <summary>
    /// 用于在部门列表数据中直接处理创建和编辑对应角色组的视图模型
    /// </summary>
    public class CreateOrEditRoleWithDepartmentVM
    {
        public Guid DepartmentId { get; set; }
        public string RoleId { get; set; }

        [Display(Name = "用户组名称")]
        [StringLength(100, ErrorMessage = "用户组名称超过了100字符。")]
        [Required(ErrorMessage = "用户组名称不能为空值。")]
        public string RoleName { get; set; }

        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }

        public string SaveStatus { get; set; }
    }
}
