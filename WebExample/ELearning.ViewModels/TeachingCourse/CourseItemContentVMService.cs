using ELearning.DataAccess;
using ELearning.DataAccess.Tools;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.ViewModels.TeachingCourse
{
    public class CourseItemContentVMService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IEntityRepository<CourseItemContent> _boRepository;
        private readonly IEntityRepository<CourseItem> _courseItemRepository;

        public CourseItemContentVMService(
            UserManager<ApplicationUser> userManager,
            IEntityRepository<CourseItemContent> boRepository,
            IEntityRepository<CourseItem> courseItemRepository
            )
        {
            _userManager = userManager;
            _boRepository = boRepository;
            _courseItemRepository = courseItemRepository;
        }

        public CourseItemContentVM GetVM()
        {
            return new CourseItemContentVM();
        }

        public CourseItemContentVM GetVM(Guid boId)
        {
            var boVM = new CourseItemContentVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(boId, z => z.Editor);
            if (bo == null)
            {
                bo = new CourseItemContent();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            SetTypeItems(boVM);

            return boVM;

        }

        public async Task<bool> SaveBo(CourseItemContentVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id);
            if (bo == null)
                bo = new CourseItemContent();

            _VMMapToBo(bo, boVM);

            if (!String.IsNullOrEmpty(boVM.CreatorUserID))
                bo.Editor =await _userManager.FindByIdAsync(boVM.CreatorUserID);

            var x = await _boRepository.AddOrEditAndSaveAsyn(bo);
            return x;

        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }


        public void SetTypeItems(CourseItemContentVM boVM)
        {
        }

        private void _BoMapToVM(CourseItemContent bo, CourseItemContentVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;
            boVM.SecondTitle = bo.SecondTitle;
            boVM.HeadContent = bo.HeadContent;
            boVM.FootContent = bo.FootContent;
            boVM.UpdateDate = bo.UpdateDate.ToString("yyyy-MM-dd");

            if (bo.Editor != null)
            {
                boVM.CreatorUserID = bo.Editor.Id.ToString();
                boVM.CreatorUserName = bo.Editor.UserName;
            }

        }

        private void _VMMapToBo(CourseItemContent bo, CourseItemContentVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.SortCode = boVM.SortCode;
            bo.Description = boVM.Description;
            bo.SecondTitle = boVM.SecondTitle;
            bo.HeadContent = boVM.HeadContent;
            bo.FootContent = boVM.FootContent;
        }

    }
}
