using BlogApp.Entity.DTOs.Categories;
using BlogApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesenNonDeleted();
        Task CreateCategoryAsync(CategoryAddDto categoryAddDto);
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<List<CategoryDto>> GetAllCategoriesNonDeletedTake24();
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<List<CategoryDto>> GetAllCategoriesDeleted();
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);
    }
}
