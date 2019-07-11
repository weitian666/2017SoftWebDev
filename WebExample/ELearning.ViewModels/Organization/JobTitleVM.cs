using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.Organization;
using ELearning.ViewModels.ControlModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.Organization
{
    public class JobTitleVM : IEntityViewModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } // 列表时候需要的序号
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "名称不能为空值。")]
        [Display(Name = "岗位名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Required(ErrorMessage = "业务编码不能为空值。")]
        [Display(Name = "业务编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string SortCode { get; set; }

        [Display(Name = "职责类型")]
        [Required(ErrorMessage = "职责类型是必须选择的。")]
        public JobTitleTypeEnum JobTitleType { get; set; }
        public string JobTitleTypeName { get; set; }
        public List<PlainFacadeItem> JobTitleTypeItemCollection { get; set; }

        public JobTitleVM() { }

        public JobTitleVM(JobTitle bo)
        {
            this.Id = bo.Id;
            this.Name = bo.Name;
            this.Description = bo.Description;
            this.SortCode = bo.SortCode;

            this.JobTitleType = bo.JobTitleType;
            this.JobTitleTypeName = bo.JobTitleType.ToString();
            this.JobTitleTypeItemCollection = PlainFacadeItemFactory<JobTitle>.GetByEnum(bo.JobTitleType);
        }

        public JobTitleVM(Guid boId, IEntityRepository<JobTitle> boService)
        {
            var bo = boService.GetSingle(boId);
            if (bo == null)
            {
                bo = new JobTitle();
                this.IsNew = true;
            }
            else
                this.IsNew = false;

            this.Id = bo.Id;
            this.Name = bo.Name;
            this.Description = bo.Description;
            this.SortCode = bo.SortCode;

            this.JobTitleType = bo.JobTitleType;
            this.JobTitleTypeName = bo.JobTitleType.ToString();
            this.JobTitleTypeItemCollection = PlainFacadeItemFactory<JobTitle>.GetByEnum(bo.JobTitleType);
        }

        public void MapToBo(JobTitle bo)
        {
            bo.Name         = Name;
            bo.Description  = Description;
            bo.SortCode     = SortCode;
            bo.JobTitleType = JobTitleType;
        }

        public void SetItems()
        {
            this.JobTitleTypeItemCollection = PlainFacadeItemFactory<JobTitle>.GetByEnum(this.JobTitleType);
        }

    }

}
