using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ELearning.ViewModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ELearning.Web.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using ELearning.DataAccess;
using ELearning.Entities.Common;

namespace ELearning.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 上传文件的控制器
    /// </summary>
    [Area("Admin")]
    public class UploadFilesController : Controller
    {
        private readonly IHostingEnvironment _HostingEnvironment;  // 系统驻留环境
        private readonly ILogger<UploadFilesController> _logger;

        // Get the default form options so that we can use them to set the default limits for request body data
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        private readonly IEntityRepository<BusinessImage> _businessImageService;
        private readonly IEntityRepository<BusinessFile> _businessFileService;
        private readonly IEntityRepository<BusinessVideo> _businessVideoService;

        public UploadFilesController(
            IHostingEnvironment hostingEnvironment,
            IEntityRepository<BusinessImage> image,
            IEntityRepository<BusinessFile> file,
            IEntityRepository<BusinessVideo> video
            )
        {
            _HostingEnvironment = hostingEnvironment;
            _businessFileService = file;
            _businessImageService = image;
            _businessVideoService = video;
        }

        /// <summary>
        /// 支持多文件上传的处理，前端使用 Bootstrap Fileinput 插件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FilesSave(string id) 
        {
            var files= Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = _GetFileSuffix(formFile.FileName);   // 文件扩展名，不含“.”
                    long fileSize = formFile.Length;                      // 获得文件大小，以字节为单位
                    string newFileName = id+"_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/"+ newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var fileBo = new BusinessFile();
                    fileBo.RelevanceObjectID = Guid.Parse(id);
                    fileBo.Name = formFile.FileName;
                    fileBo.UploadPath = "/uploadFiles/" + newFileName;
                    await _businessFileService.AddOrEditAndSaveAsyn(fileBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        [HttpPost]
        public async Task<IActionResult> ImageSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件扩展名，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/images/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var imageBo = new BusinessImage();
                    imageBo.RelevanceObjectID = Guid.Parse(id);
                    imageBo.Name = formFile.FileName;
                    imageBo.UploadPath = "/uploadFiles/images/" + newFileName;
                    imageBo.DisplayName = formFile.FileName;
                    await _businessImageService.AddOrEditAndSaveAsyn(imageBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }


        [HttpPost]
        public async Task<IActionResult> AvatarSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件后缀，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/avatars/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var imageBo = _businessImageService.GetSingleBy(x => x.RelevanceObjectID == Guid.Parse(id));
                    if(imageBo==null)
                        imageBo = new BusinessImage();

                    imageBo.RelevanceObjectID = Guid.Parse(id);
                    imageBo.Name = formFile.FileName;
                    imageBo.Description = "";
                    imageBo.UploadFileSuffix = fileExt;
                    imageBo.UploadPath = "/uploadFiles/avatars/" + newFileName;
                    imageBo.DisplayName = "头像";
                    await _businessImageService.AddOrEditAndSaveAsyn(imageBo);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        [HttpPost]
        public async Task<IActionResult> VideoSave(string id)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _HostingEnvironment.WebRootPath;
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = _GetFileSuffix(formFile.FileName);     // 文件扩展名，不含“.”
                    long fileSize = formFile.Length;                        // 获得文件大小，以字节为单位
                    string newFileName = id + "_" + formFile.FileName;      // 生成新的文件名
                    var filePath = webRootPath + "/uploadFiles/videos/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // 处理一个关联对象只能拥有一个视频
                    var videoBo =await _businessVideoService.GetSingleAsyn(Guid.Parse(id));
                    if (videoBo == null)
                    {
                        videoBo = new BusinessVideo();
                        videoBo.RelevanceObjectID = Guid.Parse(id);
                        videoBo.Name = formFile.FileName;
                        videoBo.UploadFileSuffix = fileExt;
                        videoBo.UploadPath = "/uploadFiles/videos/" + newFileName;
                    }
                    else
                    {
                        videoBo.RelevanceObjectID = Guid.Parse(id);
                        videoBo.Name = formFile.FileName;
                        videoBo.UploadFileSuffix = fileExt;
                        videoBo.UploadPath = "/uploadFiles/videos/" + newFileName;
                    }
                    await _businessVideoService.AddOrEditAndSaveAsyn(videoBo);
                }

            }

            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// 使用流的形式的来处理上传的文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FilesSaveWithStream()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(Request.ContentType),_defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        targetFilePath = Path.GetTempFileName();
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);

                            _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).ToString();
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key, value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            //var user = new User();
            //var formValueProvider = new FormValueProvider(
            //    BindingSource.Form,
            //    new FormCollection(formAccumulator.GetResults()),
            //    CultureInfo.CurrentCulture);

            //var bindingSuccessful = await TryUpdateModelAsync(user, prefix: "",
            //    valueProvider: formValueProvider);
            //if (!bindingSuccessful)
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }
            //}

            //var uploadedData = new UploadedData()
            //{
            //    Name = user.Name,
            //    Age = user.Age,
            //    Zipcode = user.Zipcode,
            //    FilePath = targetFilePath
            //};
            return Ok(new { count = 1100, size = 100 });

        }

        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private string _GetFileSuffix(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                  return "";
            string[] temp = fileName.Split('.');
            return temp[temp.Length - 1].ToLower();
        }
        
        private static Encoding GetEncoding(MultipartSection section)
        {
            Microsoft.Net.Http.Headers.MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }

    }
}