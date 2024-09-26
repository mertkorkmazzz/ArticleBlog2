using BlogApp.Entity.DTOs.Images;
using BlogApp.Entity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadedDto> Upload(string name , IFormFile formFile, ImageType ımageType , string folderName = null);
        void Delete(string imageName);
    }
}
