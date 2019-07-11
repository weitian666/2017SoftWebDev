using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.Entities.Tools
{
    public class ValidateCode: IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string SortCode { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        public ValidateCode()
        {
            this.Id = Guid.NewGuid();
        }

        public void SetSortCode()
        {
            this.SortCode = "";// 这里创建生成验证码的代码
        }
    }
}
