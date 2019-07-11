using ELearning.DataAccess;
using ELearning.Entities.News;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ELearning.ViewModels.ControlModels;
using ELearning.DataAccess.Tools;

namespace ELearning.ViewModels.News
{
    public class ArticleTypeVMService
    {
        private readonly IEntityRepository<ArticleType> _boRepository;

        public ArticleTypeVMService(
            IEntityRepository<ArticleType> repository
            )
        {
            _boRepository = repository;
        }

        /// <summary>
        /// 返回一个新的 VM
        /// </summary>
        /// <returns></returns>
        public ArticleTypeVM GetVM()
        {
            return new ArticleTypeVM();
        }

        /// <summary>
        /// 根据业务实体对象 Id 返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public ArticleTypeVM GetVM(Guid id)
        {
            var boVM = new ArticleTypeVM();
            // 初始化数据对象
            var bo = _boRepository.GetSingle(id, x => x.ParentType);
            if (bo == null)
            {
                bo = new ArticleType();
                boVM.IsNew = true;
            }
            else
                boVM.IsNew = false;

            // 映射基本的属性值
            _BoMapToVM(bo, boVM);

            // 设置供前端下拉选项所需要的数据集合
            SetRelevanceItems(boVM);

            return boVM;
        }

        /// <summary>
        /// 根据业务实体对象返回对应的视图模型
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public ArticleTypeVM GetVM(ArticleType bo)
        {
            var boVM = new ArticleTypeVM();
            _BoMapToVM(bo, boVM);
            return boVM;
        }

        /// <summary>
        /// 返回全部业务对象对应的视图模型，返回的视图模型根据要求做了层次化处理
        /// </summary>
        /// <param name="boService"></param>
        /// <returns></returns>
        public async Task<List<ArticleTypeVM>> GetboVMCollectionAsyn()
        {
            var boCollection = await _boRepository.GetAllIncludingAsyn(x=>x.ParentType);
            var boVMCollection = new List<ArticleTypeVM>();
            var counter = 0;
            foreach (var bo in boCollection.OrderBy(x => x.SortCode))
            {
                var boVM = GetVM(bo);
                boVM.OrderNumber = (++counter).ToString();
                boVMCollection.Add(boVM);
            }

            #region 为部门数据呈现处理名称缩进
            var parentTypeItems = SelfReferentialItemFactory<ArticleType>.GetCollection(boCollection.ToList(), true);
            foreach (var item in parentTypeItems)
            {
                var dID = Guid.Parse(item.ID);
                var dept = boVMCollection.FirstOrDefault(x => x.Id == dID);
                dept.Name = item.DisplayName;
            }
            #endregion

            return boVMCollection;
        }

        /// <summary>
        /// 将视图对象的数据，映射为业务对象数据，进行持久化处理
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveBo(ArticleTypeVM boVM)
        {
            var bo = _boRepository.GetSingle(boVM.Id, r => r.ParentType);
            if (bo == null)
                bo = new ArticleType();

            await _VMMapToBo(bo, boVM);

            var saveResult = await _boRepository.AddOrEditAndSaveAsyn(bo);
            return saveResult;
        }

        public async Task<DeleteStatusModel> DeletBoStatus(Guid id)
        {
            var status = await _boRepository.DeleteAndSaveAsyn(id);
            return status;
        }

        /// <summary>
        /// 设置用于前端页面需要的关联数据选项
        /// </summary>
        public void SetRelevanceItems(ArticleTypeVM boVM)
        {
            boVM.ParentItemCollection = SelfReferentialItemFactory<ArticleType>.GetCollection(_boRepository, true);
        }

        /// <summary>
        /// 业务对象的属性转换为视图对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private void _BoMapToVM(ArticleType bo, ArticleTypeVM boVM)
        {
            boVM.Id = bo.Id;
            boVM.Name = bo.Name;
            boVM.Description = bo.Description;
            boVM.SortCode = bo.SortCode;

            if (bo.ParentType != null)
            {
                boVM.ParentItemID = bo.ParentType.Id.ToString();
                boVM.ParentItemName = bo.ParentType.Name;
            }
        }

        /// <summary>
        /// 视图对象的属性转换为业务对象的属性
        /// </summary>
        /// <param name="bo"></param>
        private async Task _VMMapToBo(ArticleType bo, ArticleTypeVM boVM)
        {
            bo.Id = boVM.Id;
            bo.Name = boVM.Name;
            bo.Description = boVM.Description;
            bo.SortCode = boVM.SortCode;

            if (!String.IsNullOrEmpty(boVM.ParentItemID))
                bo.ParentType = await _boRepository.GetSingleAsyn(Guid.Parse(boVM.ParentItemID));
            else
            {
                bo.ParentType = bo;
            }

        }

    }
}
