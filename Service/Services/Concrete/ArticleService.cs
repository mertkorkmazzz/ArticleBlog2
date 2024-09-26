using AutoMapper;
using BlogApp.DAL.UnitOfWork;
using BlogApp.Entity.DTOs.Articles;
using BlogApp.Entity.Entities;
using BlogApp.Entity.Enums;
using BlogApp.Service.Extensions;
using BlogApp.Service.Helpers.Images;
using BlogApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = BlogApp.Entity.Entities.Image;

namespace BlogApp.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {


        private readonly IUnitOfWork _unitOfWork;// Veritabanı işlemleri için bir birim işi (Unit of Work) yönetimi sağlar.
        private readonly IMapper _mapper;//AutoMapper kullanarak veri transfer nesnelerini (DTO) ve entity nesneleri arasında dönüşüm yapar.
        private readonly IHttpContextAccessor _httpContextAccessor;// HTTP bağlamına erişim sağlar, bu genellikle kullanıcı bilgilerini almak için kullanılır
        private readonly IImageHelper _ımageHelper;// Resim yükleme ve işleme işlevlerini sağlar
        private readonly ClaimsPrincipal _User;//Kullanıcı bilgilerini sağlarv

        public ArticleService(IUnitOfWork unitOfWork , IMapper mapper  , IHttpContextAccessor httpContextAccessor ,IImageHelper ımageHelper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._ımageHelper = ımageHelper;
            _User = httpContextAccessor.HttpContext.User;
        }





        //METOTLAR

        //Sayfalandırmış makale listesi döndürür (parametreler current = şuanki sayfa numarası , page = sayfa başına makale sayısı , isAscending: Makalelerin tarih sırasına göre sıralanıp sıralanmayacağını belirtir)
        public async Task<ArticleListDto> GetAllByPagingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize; // pagasize 20 nin üstüne çıkmasını engelliyor
            var articles = categoryId == null //eğer makale categori belirtilmemişse null ise yani silinmemiş makaleler gelcek
                ? await _unitOfWork.GetRepository<Article>().GetAllAsync(a => !a.IsDeleted, a => a.Category, i => i.Image, u => u.User)//Bu metot, ilgili Article entity'sinin veritabanından getirilmesini sağlar. İlişkili veriler olan Category, Image ve User da sorguya dahil edilir
                : await _unitOfWork.GetRepository<Article>().GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted,
                    a => a.Category, i => i.Image, u => u.User);
            var sortedArticles = isAscending//isAscending parametresine göre makaleler CreatedDate (oluşturulma tarihi) değerine göre artan sıralama yapılır ,ırayla sıralanırsa (isAscending == false), makaleler en yeni tarihli olandan başlayarak sıralanır.
                ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList() //currentPage'e göre kaç makalenin atlanacağını hesaplar : Sonra belirtilen pageSize kadar makale alınır.
                : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };//Amaç: Sayfalanmış ve sıralanmış makaleleri içeren bir DTO
        }

        // Yeni bir makale oluşturur. / Oturum açmış kullanıcının kimliği ve e-posta adresi alınır.Resim yüklenir ve veritabanına kaydedilir.Makale oluşturulur ve veritabanına eklenir.Değişiklikler kaydedilir.
        public async Task CreateArticleAsync(ArticleAddDto articleAddDto)
        {

            var userId =_User.GetLoggedInUserId();
            var userEmail = _User.GetLoggedInEmail();

            var imageUpload = await _ımageHelper.Upload(articleAddDto.Title, articleAddDto.photo, ImageType.post);
            Image image = new(imageUpload.FullName, articleAddDto.photo.ContentType, userEmail);
            await _unitOfWork.GetRepository<Image>().AddAsync(image);

            var article = new Article(articleAddDto.Title, articleAddDto.Content, userId, userEmail, articleAddDto.CategoryId, image.Id);


            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            await _unitOfWork.SaveAsync();
        }

        //Silinmemiş tüm makaleleri ve ilişkili kategorileri getirir. /Veritabanındaki silinmemiş makaleler ve kategorileri DTO'lara dönüştürülerek döndürülür.
        public async Task<List<ArticleDto>> GetAllArticleWithCategoryNonDeletedAsync()
        {
          var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x =>!x.IsDeleted , x => x.Category);
            var map = _mapper.Map<List<ArticleDto>>(articles);

            return map;
        }

        // Belirli bir makaleyi ID'sine göre alır ve DTO'ya dönüştürür.İşleyiş: Belirtilen makale veritabanından alınır ve DTO'ya dönüştürülür.
        public async Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleId , x => x.Category , i => i.Image);
            var map = _mapper.Map<ArticleDto>(article);

            return map;
        }

        //Var olan bir makaleyi günceller.
        public async Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var UserEmail = _User.GetLoggedInEmail();
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted && x.Id == articleUpdateDto.Id, x => x.Category , i => i.Image);


            if (articleUpdateDto.Photo != null)
            {
                _ımageHelper.Delete(article.Image.FileName);

                var imageUpload = await _ımageHelper.Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.post);
                Image image = new(imageUpload.FullName, articleUpdateDto.Photo.ContentType, UserEmail);
                await _unitOfWork.GetRepository<Image>().AddAsync(image);

                article.ImageId = image.Id;

            }



            article.Title = articleUpdateDto.Title;
            article.Content = articleUpdateDto.Content;
            article.CategoryId = articleUpdateDto.CategoryId;

            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy = UserEmail;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;


        }

        //Makaleyi fiziksel olarak silmek yerine "silinmiş" olarak işaretler (soft delete).
        public async Task<string> SafeDeletArticleAsync(Guid ArticleId)
        {

            var UserEmail = _User.GetLoggedInEmail();
            var article = await _unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);

            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = UserEmail;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }

        //Silinmiş (soft delete) tüm makaleleri kategori bilgileriyle birlikte veritabanından alır ve DTO'lara dönüştürerek döner
        public async Task<List<ArticleDto>> GetAllDeletArticles()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = _mapper.Map<List<ArticleDto>>(articles);

            return map;
        }

        //Silinmiş bir makaleyi geri alır
        public async Task<string> UndoDeletArticleAsync(Guid ArticleId)
        {
      
            var article = await _unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);

            article.IsDeleted = false;
            article.DeletedDate = null;
            article.DeletedBy = null;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }


        //Anahtar kelime ile makale araması yapar ve sayfalama ile sonuçları döner.
        public async Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(
                a => !a.IsDeleted && (a.Title.Contains(keyword) || a.Content.Contains(keyword) || a.Category.Name.Contains(keyword)),
            a => a.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }
    }
}
