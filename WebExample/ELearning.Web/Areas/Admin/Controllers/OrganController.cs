using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess;
using ELearning.Entities.Organization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrganController : CommonEntityController<Organ>
    {
        public OrganController(IEntityRepository<Organ> service
            ) : base(service)
        {
            this.EntityModuleName = "组织人员管理";
            this.EntityTitle = "组织定义";
        }
    }
}