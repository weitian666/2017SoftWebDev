using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.News;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleTopicController : CommonEntityController<ArticleTopic>
    {
        public ArticleTopicController(IEntityRepository<ArticleTopic> service
            ) : base(service)
        {
            this.EntityModuleName = "站点文章信息管理";
            this.EntityTitle = "站点文章专栏";
        }
    }
}
