
using BlogApp.Service.FluentValidations;
using BlogApp.Service.Helpers.Images;
using BlogApp.Service.Services.Abstractions;
using BlogApp.Service.Services.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;



namespace BlogApp.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtensions(this IServiceCollection services)
        {

            var assembly = Assembly.GetExecutingAssembly();
            

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddSingleton<IImageHelper , ImageHalper>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddAutoMapper(assembly);

            services.AddControllersWithViews().AddFluentValidation(opt =>

            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidatior>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            }
            );

            return services;
        }
    }
}
