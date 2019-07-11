using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.Areas.Admin.Models
{
    public class CreateOrEditUserWithPersonVM
    {
        public Guid PersonId { get; set; }
        public Guid UserId { get; set; }

        [Display(Name = "用户组名称")]
        [StringLength(100, ErrorMessage = "用户组名称超过了100字符。")]
        [Required(ErrorMessage = "用户组名称不能为空值。")]
        public string UserName { get; set; }

        [Display(Name = "姓名")]
        public string PersonName { get; set; }

        public string SaveStatus { get; set; }
    }
}
