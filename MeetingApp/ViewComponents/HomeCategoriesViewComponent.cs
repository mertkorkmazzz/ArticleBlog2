﻿using BlogApp.Service.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents
{
    public class HomeCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public HomeCategoriesViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoryService.GetAllCategoriesNonDeletedTake24();
            return View(categories);
        }
    }
}