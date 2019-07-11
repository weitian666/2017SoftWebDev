using ELearning.DataAccess.Tools;
using ELearning.UserAndRole;
using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.UserAndRole
{
    public class ApplicationRoleVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Display(Name="用户组名称")]
        [StringLength(100, ErrorMessage = "用户组名称超过了100字符。")]
        [Required(ErrorMessage = "用户组名称不能为空值。")]
        public string Name { get; set; }

        [Display(Name = "用户组简要描述")]
        [StringLength(500, ErrorMessage = "用户组名称超过了500字符。")]
        public string Description { get; set; }

        [Display(Name = "用户组编码")]
        [StringLength(30, ErrorMessage = "用户组编码超过了30字符。")]
        [Required(ErrorMessage = "用户组编码不能为空值。")]
        public string SortCode { get; set; }

        [Display(Name = "用户组类型")]
        [Required(ErrorMessage = "用户组类型是必须选择的。")]
        public ApplicationRoleTypeEnum ApplicationRoleType { get; set; }
        public string ApplicationRoleTypeName { get; set; }
        public List<PlainFacadeItem> ApplicationRoleTypeItemCollection { get; set; }

        public int UserAmount { get; set; }

        public ApplicationRoleVM()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }

        //public ApplicationRoleVM(ApplicationRole bo)
        //{
        //    this.Id = bo.Id;
        //    this.Name = bo.Name;
        //    this.Description = bo.Description;
        //    this.SortCode = bo.SortCode;

        //    this.ApplicationRoleType = bo.ApplicationRoleType;
        //    this.ApplicationRoleTypeName = bo.ApplicationRoleType.ToString();
        //    this.ApplicationRoleTypeItemCollection = _GetByEnum(bo.ApplicationRoleType);
        //}

        //public void MapToBo(ApplicationRole bo)
        //{
        //    bo.Id = this.Id;
        //    bo.Name = this.Name;
        //    bo.Description = this.Description;
        //    bo.SortCode = this.SortCode;
        //    bo.ApplicationRoleType = this.ApplicationRoleType;
        //}

        //public void SetPlainFacadeItem()
        //{
        //    this.ApplicationRoleTypeItemCollection = _GetByEnum(this.ApplicationRoleType);
        //}

        ///// <summary>
        ///// 直接将泛型类型中指定的枚举类型转换为 PlainFacdeItem 集合
        ///// </summary>
        ///// <returns></returns>
        //private List<PlainFacadeItem> _GetByEnum(Object enumObject)
        //{
        //    var enumItems = Enum.GetValues(enumObject.GetType());
        //    var items = new List<PlainFacadeItem>();
        //    foreach (var eItem in enumItems)
        //    {
        //        var item = new PlainFacadeItem()
        //        {
        //            ID = ((int)eItem).ToString(),
        //            Name = eItem.ToString(),
        //        };
        //        items.Add(item);
        //    }
        //    return items;
        //}
    }
}
