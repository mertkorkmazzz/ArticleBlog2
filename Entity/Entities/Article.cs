using BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity.Entities
{
    public class Article : EntityBase 
    {
        //entitybase den miras aldı çünkü EB de her entity sınıfın kullanılcak entityler var kod tekrarını azaltmak için miras alındı
        public Article()
        {

        }

        public Article(string title , string content , Guid userId, string createdBy, Guid categoryId , Guid imageId ) 
        {
            Title = title;
            Content = content;
            UserId = userId;
            CategoryId = categoryId;
            ImageId = imageId;
            CreatedBy = createdBy;
        }
        //ctorun dolu olmasının sebebi verilen değerler null olmaması için dir eğer eşlemeseydik 
        // örnek tittle string değeri null olcaktı ve proje tam doğru şekilde başlamıcaktı
        //Kapsamlı yapıcı metot, bir nesnenin oluşturulması sırasında gerekli tüm bilgilerin bir arada ve doğru sırada verilmesini sağlar. =>
        //Bu, hem kodun daha okunabilir olmasını sağlar hem de hataları en aza indirir. Eğer bu yapıcı metot kullanılmazsa,=>
        //geliştiricinin her özelliği tek tek ataması gerekir ki bu da hataya açık bir yaklaşımdır


        public string Title { get; set; } // başlık
        public string Content { get; set; } //içeriği
        public int ViewCount { get; set; } = 0; // görüntülenme sayısı / 0 olma sebebi ilk oluşturulduğunda null değer yerine 0 görüntülenme olsun diye


        // burda her makalenin bir kategori alabilceğini belli eder
        // bire bir ilişki vardır
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        // burda her makalenin bir resinle eşleşebilir yada nullable olabilir
        public Guid? ImageId { get; set; }
        public Image Image{ get; set; }

        // burda her makalenin bir kullanıcısı olabilceğini belli eder
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
