using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Entities.Common
{
    /// <summary>
    /// 通过关联的两种对象的ID来约束抽象的类关系，在具体的应用中只要需要关联多对多关系的
    /// 统一通过这个类来进行约束。
    /// </summary>
    public class BusinessMultiRelationWthID:IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string MasterObjectName { get; set; }  // 全路径类的名称
        public string DetailObjectName { get; set; }  // 全路径类的名称

        public Guid MasterObjectID { get; set; }
        public Guid DetailObjectID { get; set; }

        public BusinessMultiRelationWthID() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}
