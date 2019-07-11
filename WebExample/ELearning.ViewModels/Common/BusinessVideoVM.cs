using ELearning.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.Common
{
    public class BusinessVideoVM
    {
        public Guid Id { get; set; }
        public string FileId { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string VideoFileName { get; set; }
        public string VideoFilePath { get; set; }

        public BusinessVideoVM(BusinessVideo bo)
        {
            this.Id = bo.Id;
            this.ObjectId = bo.RelevanceObjectID.ToString();
            this.VideoFileName = bo.Name;
            this.VideoFilePath = bo.UploadPath;
        }
    }
}
