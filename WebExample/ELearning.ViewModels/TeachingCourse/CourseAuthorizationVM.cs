using ELearning.DataAccess.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    /// <summary>
    /// 课程访问授权视图模型
    /// </summary>
    public class CourseAuthorizationVM:IEntityViewModel
    {
        public Guid Id { get; set; }                               // 对应角色组或者用户的 Id
        public string OrderNumber { get; set; }                    // 列表序号
        public bool IsNew { get; set; }                            // 是否是新数据
        public ListPageParameter ListPageParameter { get; set; }   // 列表所需要的分页器

        public string Name { get; set; }                           // 用户名或者角色名称
        public string Description { get; set; }                    // 在组织机构或者班级结构中的层次链说明
        public string SortCode { get; set; }                       // 学生或者员工的 SortCode

        public string DisplayName { get; set; }                    // 学生或者员工，或者角色组的显示名
        public bool IsUser { get; set; }                           // 用户或者角色组
    }
}
