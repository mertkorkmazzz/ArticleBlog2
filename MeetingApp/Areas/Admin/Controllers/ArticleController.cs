
using AutoMapper;
using BlogApp.Consts;
using BlogApp.Entity.DTOs.Articles;
using BlogApp.Entity.Entities;
using BlogApp.ResultMessage;
using BlogApp.Service.Extensions;
using BlogApp.Service.Services.Abstractions;
using FluentValidation;
using MeetingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _validator;
        private readonly IToastNotification _toastNotification;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleController(IArticleService articleService , ICategoryService categoryService ,IMapper mapper , IValidator<Article> validator , IToastNotification toastNotification , IHttpContextAccessor httpContextAccessor)
        {
            this._articleService = articleService;
            this._categoryService = categoryService;
            this._mapper = mapper;
            this._validator = validator;
            this._toastNotification = toastNotification;
            this._httpContextAccessor = httpContextAccessor;
        }



        //LİSTELEME
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}, {RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {
            var articles =await _articleService.GetAllArticleWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> DeletedArticles()
        {
            var articles = await _articleService.GetAllDeletArticles();
            return View(articles);
        }

        //EKLEME
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add()
        {
      

            var categories = await _categoryService.GetAllCategoriesenNonDeleted(); 
            return View(new ArticleAddDto { categories = categories});
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {
            var map = _mapper.Map<Article>(articleAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
              await _articleService.CreateArticleAsync(articleAddDto);
                _toastNotification.AddSuccessToastMessage(Messages.Article.Add(articleAddDto.Title), new ToastrOptions { Title = "Başar"});
              return  RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState); 
                var categories = await _categoryService.GetAllCategoriesenNonDeleted();
                return View(new ArticleAddDto { categories = categories });
            }  
        }


        //GÜNCELLEME
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
     

            var categories = await _categoryService.GetAllCategoriesenNonDeleted();
            var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.categories = categories;

            return View(articleUpdateDto);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {
          

            var map = _mapper.Map<Article>(articleUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await _articleService.UpdateArticleAsync(articleUpdateDto);
                _toastNotification.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions() { Title = "işlem başarılı" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await _categoryService.GetAllCategoriesenNonDeleted();

            articleUpdateDto.categories = categories;

            return View(articleUpdateDto);

        }



        //DELET
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Delet(Guid articleId)
        {
            var title =   await _articleService.SafeDeletArticleAsync(articleId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions() { Title = "işlem başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelet(Guid articleId)

        {
            var title = await _articleService.UndoDeletArticleAsync(articleId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions() { Title = "işlem başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }

    }
}
