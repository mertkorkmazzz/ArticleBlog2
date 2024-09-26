using BlogApp.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.FluentValidations
{
    public class ArticleValidatior : AbstractValidator<Article>
    {
        //Bu kod, FluentValidation kütüphanesi kullanılarak Article nesnesi için belirli kurallar tanımlamanıza olanak tanır.
        //Bu sayede, uygulamanızda kullanıcılardan alınan verilerin geçerli olup olmadığını kolayca kontrol edebilirsiniz.
        //Bu tür bir doğrulama, veri kalitesini artırır ve hatalı verilerin işleme alınmasını önler.
        public ArticleValidatior()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(150)
                .WithName("Başlık");

            RuleFor(x => x.Content)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(150)
                .WithName("İçerik");

        }
    }
}
