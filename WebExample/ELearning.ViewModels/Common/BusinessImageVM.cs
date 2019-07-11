using ELearning.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.Common
{
    public class BusinessImageVM
    {
        public Guid Id { get; set; }
        public string FileId { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFilePath { get; set; }
        //public IFormFile ImageFile { get; set; }

        public BusinessImageVM(BusinessImage bo)
        {
            this.Id = bo.Id;
            this.ObjectId = bo.RelevanceObjectID.ToString();
            this.ImageFileName = bo.Name;
            this.ImageFilePath = bo.UploadPath;
        }
    }
}
