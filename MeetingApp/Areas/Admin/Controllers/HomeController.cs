using BlogApp.Entity.Entities;
using BlogApp.Service.Services.Abstractions;
using BlogApp.Service.Services.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static BlogApp.ResultMessage.Messages;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IDashboardService _dashboardService;

        public HomeController(IArticleService articleService , IDashboardService dashboardService)
        {
            this._articleService = articleService;
            this._dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var aryticles = await _articleService.GetAllArticleWithCategoryNonDeletedAsync();
       
            return View(aryticles);
        }

        [HttpGet]
        public async Task<IActionResult> YearlyArticleCounts()
        {
            var count = await _dashboardService.GetYearlyArticleCounts();
            return Json(JsonConvert.SerializeObject(count));
        }


        [HttpGet]
        public async Task<IActionResult> TotalArticleCount()
        {
            var count = await _dashboardService.GetTotalArticleCount();
            return Json(count);
        }
        [HttpGet]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var count = await _dashboardService.GetTotalCategoryCount();
            return Json(count);
        }
    }
}
