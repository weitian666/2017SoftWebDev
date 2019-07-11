using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.UserAndRole
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        [StringLength(250)]
        public string DisplayName { get; set; }  // 显示名（用于显示中文名）
        [StringLength(550)]
        public string Description { get; set; }  // 简要说明
        [StringLength(50)]
        public string SortCode { get; set; }     // 简要的编码
        public bool IsDefaultRole { get; set; }  // 是否是缺省用户组，如果是，则不能在管理台中进行编辑和删除

        public ApplicationRoleTypeEnum ApplicationRoleType { get; set; }  // 角色类型

        public ApplicationRole() : base()
        {
            this.Id = Guid.NewGuid();
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
            this.Id = Guid.NewGuid();
            this.Name = roleName;
        }
    }
}

