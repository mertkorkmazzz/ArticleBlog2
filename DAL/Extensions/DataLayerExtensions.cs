using BlogApp.DAL.Context;
using BlogApp.DAL.Repositories.Abstractions;
using BlogApp.DAL.Repositories.Concretes;
using BlogApp.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DAL.Extensions
{
    public static class  DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtensions(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(Opt => Opt.UseSqlServer(config.GetConnectionString("DefaultConnection"))); // veritabanına bağlanmak için

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUnitOfWork, UnitOfWorks>();

            return services;
        }
    }
}
