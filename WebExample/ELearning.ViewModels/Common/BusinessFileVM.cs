using ELearning.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.Common
{
    public class BusinessFileVM
    {
        public Guid Id { get; set; }
        public string FileId { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public BusinessFileVM(BusinessFile bo)
        {
            this.Id = bo.Id;
            this.ObjectId = bo.RelevanceObjectID.ToString();
            this.FileName = bo.Name;
            this.FilePath = bo.UploadPath;
        }
    }
}
