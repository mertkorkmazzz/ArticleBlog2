﻿using BlogApp.Entity.DTOs.Images;
using BlogApp.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Helpers.Images
{
    public class ImageHalper : IImageHelper  
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly string wwwroot;
        private const string imgFolder = "imeges";
        private const string articleImagesFolder = "article-images";
        private const string UserImageFolder = "User-images";

        public ImageHalper(IWebHostEnvironment webHost)
        {
            this._webHost = webHost;
            wwwroot = webHost.WebRootPath;
        }



        //Amacı: Resim dosya adlarındaki geçersiz veya istenmeyen karakterleri temizlemek için kullanılır. Bu, dosya adlarının web sunucusunda geçerli ve güvenli olmasını sağlar.
        private string ReplaceInvalidChars(string fileName)
        {
            return fileName.Replace("İ", "I")
                .Replace("ı", "i")
                .Replace("Ğ", "G")
                .Replace("ğ", "g")
                .Replace("Ü", "U")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("Ş", "S")
                .Replace("Ö", "O")
                .Replace("ö", "o")
                .Replace("Ç", "C")
                .Replace("ç", "c")
                .Replace("é", "")
                .Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("+", "")
                .Replace("%", "")
                .Replace("/", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace("_", "")
                .Replace("*", "")
                .Replace("æ", "")
                .Replace("ß", "")
                .Replace("@", "")
                .Replace("€", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("½", "")
                .Replace("{", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("}", "")
                .Replace(@"\", "")
                .Replace("|", "")
                .Replace("~", "")
                .Replace("¨", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace("`", "")
                .Replace(".", "")
                .Replace(":", "")
                .Replace(" ", "");
        }



        //Amacı: Kullanıcı veya makale resmi gibi dosyaları sunucuya yüklemek ve dosya adlarını düzenlemek için kullanılır.
        public async Task<ImageUploadedDto> Upload(string name, IFormFile imageFile, ImageType ımageType , string folderName = null)
        {
          folderName ??= ımageType == ImageType.user ? UserImageFolder : articleImagesFolder;

            if (!Directory.Exists($"{wwwroot}/{imgFolder}/{folderName}")) ;
            Directory.CreateDirectory($"{wwwroot}/{imgFolder}/{folderName}");

            string oldFileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string fileExtension = Path.GetExtension(imageFile.FileName);

            name = ReplaceInvalidChars(name);

            DateTime dateTime = DateTime.Now;

            string newFileName = $"{name}_{dateTime.Millisecond}{fileExtension}";

            var path = Path.Combine($"{wwwroot}/{imgFolder}/{folderName}", newFileName);

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            await imageFile.CopyToAsync(stream);
            await stream.FlushAsync();

            string message = ımageType == ImageType.user
                ? $"{newFileName} isimli kullanıcı resmi başarı ile eklenmiştir."
                : $"{newFileName} isimli makale resmi başarı ile eklenmiştir";

            return new ImageUploadedDto()
            {
                FullName = $"{folderName}/{newFileName}"
            };


        }


        //Amacı: Sunucuda belirtilen bir resim dosyasını silmek için kullanılır.
        public void Delete(string imageName)
        {
            var fileToDelete = Path.Combine($"{wwwroot}/{imgFolder}/{imageName}");
            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
        }


    }
}
