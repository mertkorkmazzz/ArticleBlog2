using BlogApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DAL.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(
            
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "c# makale",
                Content = "Bu kapsam doğrultusunda ChatGPT’yi bir SEO uzmanı olarak nitelendirmek şu an ki aşamada doğru olmayabilir fakat bir SEO stajyeri olarak bir nitelendirme yapılabilir. ChatGPT bizler için soru-cevap, içerik üretimi, metin çevirisi, anahtar kelime araştırması, semantik kelime araştırması gibi birçok maddede yardımcı olabilir ve sıranızı yükseltmek istediğiniz anahtar kelimelerinizde sizlere bir yol haritası planlaması yapabilir.",
                ViewCount = 15,
                CategoryId= Guid.Parse("FD2A3A42-425C-4B00-BCC3-8BDFFE72BA87"),
                ImageId = Guid.Parse("ADF16BF4-7DD0-410A-8D1C-45C73DAE9E1E"),
                CreatedBy ="Admin Test",
                CreatedDate= DateTime.Now,
                IsDeleted= false,
                UserId  = Guid.Parse("B7F28859-F188-445B-884E-334883C9B01E")
            },



            new Article
            {
                Id = Guid.NewGuid(),
                Title = "java makale",
                Content = "java Bu kapsam doğrultusunda ChatGPT’yi bir SEO uzmanı olarak nitelendirmek şu an ki aşamada doğru olmayabilir fakat bir SEO stajyeri olarak bir nitelendirme yapılabilir. ChatGPT bizler için soru-cevap, içerik üretimi, metin çevirisi, anahtar kelime araştırması, semantik kelime araştırması gibi birçok maddede yardımcı olabilir ve sıranızı yükseltmek istediğiniz anahtar kelimelerinizde sizlere bir yol haritası planlaması yapabilir.",
                ViewCount = 15,   
                CategoryId= Guid.Parse("35039143-2676-458A-BD92-DF4BB38B80D2"),
                ImageId= Guid.Parse("E9EEE5F2-EC5E-474F-BC02-53560E6A34A8"),
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = Guid.Parse("7DE80FB4-FC89-45AF-988D-4DBABEA6A811")
            }
            );
        }
    }
}
