using System;
using System.ComponentModel.DataAnnotations;

namespace ELearning.Entities
{
    public abstract class Entity : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }         // 类型名或栏目名称
        public string Description { get; set; }  // 类型描述
        [StringLength(150)]
        public string SortCode { get; set; }

        public Entity()
        {
            this.Id = Guid.NewGuid();

        }
    }
}
