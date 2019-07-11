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
using System.Text;

namespace ELearning.ViewModels.Organization
{
    public class DepartmentVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } // 列表时候需要的序号
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "部门名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Required(ErrorMessage = "业务编码不能为空值。")]
        [Display(Name = "业务编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string SortCode { get; set; }

        [Display(Name = "部门类型")]
        [Required(ErrorMessage = "部门类型是必须选择的。")]
        public DepartmentTypeEnum DepartmentType { get; set; }       // 部门种类
        public string DepartmentTypeName { get; set; }
        public List<PlainFacadeItem> DepartmentTypeItemCollection { get; set; }

        [Display(Name = "归属组织类型")]
        public string OrganizationId { get; set; }  
        public string OrganizationName { get; set; }
        public List<PlainFacadeItem> OrganizationItemCollection { get; set; }

        [Display(Name = "归属上级")]
        public string ParentDepartmentId { get; set; }
        public string ParentDepartmentName { get; set; }
        public List<SelfReferentialItem> ParentDepartmentItemCollection { get; set; }

        [Display(Name = "部门角色组")]
        public string ApplicationRoleId { get; set; }
        public string ApplicationRoleName { get; set; }
        public List<PlainFacadeItem> ApplicationRoleItemCollection { get; set; }

        [Display(Name = "自动创建用户组")]
        public bool IsCreateRoleAuto { get; set; } // 是否自动创建以单位命名的用户组

        public int PersonAmount { get; set; } // 人员数量

        public DepartmentVM()
        {
            this.Id = Guid.NewGuid();

        }
    }
}
