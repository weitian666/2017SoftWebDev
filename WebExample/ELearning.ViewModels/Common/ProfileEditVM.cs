using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearning.ViewModels.Common
{
    /// <summary>
    /// 用于演示头像上传的例子
    /// </summary>
    public class ProfileEditVM
    {
        [Required]
        public string Name { get; set; }
        public string AvatarPath { get; set; }
        public IFormFile AvatarFile { get; set; }
    }
}
