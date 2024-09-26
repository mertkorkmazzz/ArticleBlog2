using BlogApp.DAL.Context;
using BlogApp.DAL.Extensions;
using BlogApp.Entity.Entities;
using BlogApp.Service.Describers;
using BlogApp.Service.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NToastNotify;

namespace MeetingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.LoadDataLayerExtensions(builder.Configuration);//servisleri eklemek i�in kullan�l�r
            builder.Services.LoadServiceLayerExtensions();//servisleri eklemek i�in kullan�l�r

            builder.Services.AddSession();

            // Add services to the container


            builder.Services.AddControllersWithViews()

                .AddNToastNotifyToastr(new ToastrOptions()
                {
                    PositionClass = ToastPositions.TopRight,
                    TimeOut = 3000
                }) // bildirim g�stermek i�in kullan�l�r
                .AddRazorRuntimeCompilation(); // uygulamay� ba�latmadan de�i�ikleri g�rmeye sa�lar


            builder.Services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
            }) //ASP.NET Core Identity'yi yap�land�r�r. AppUser ve AppRole s�n�flar�n� kullanarak kullan�c� ve rol y�netimi sa�lar. �ifre politikalar�n� belirler: burada, �ifrenin b�y�k harf, k���k harf veya �zel karakter i�ermesi zorunlu de�ildir.
            .AddRoleManager<RoleManager<AppRole>>() //Rol y�netimini sa�lar, bu da uygulaman�zda kullan�c� rollerinin (�rne�in, admin, kullan�c�) tan�mlanmas�na ve y�netilmesine olanak tan�r.
          .AddErrorDescriber<CustomIdentiyErorDescriber>()//Kimlik do�rulama hatalar�n� �zelle�tirmek i�in kullan�l�r. CustomIdentiyErorDescriber s�n�f�, varsay�lan hata mesajlar�n� �zelle�tirmek amac�yla olu�turulmu� �zel bir s�n�ft�r.
            .AddEntityFrameworkStores<AppDbContext>()//Kullan�c� ve rol bilgilerini Entity Framework kullanarak belirlenen veritaban�nda saklar (AppDbContext).
            .AddDefaultTokenProviders();//Kullan�c� do�rulama, parola s�f�rlama gibi i�lemler i�in gereken varsay�lan token sa�lay�c�lar�n� ekler.


            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = new PathString("/Admin/Auth/Login"); //Kullan�c� oturum a�ma ve oturum kapatma yollar�n� belirler. �rne�in, bir kullan�c� yetkilendirme gerektiren bir sayfaya eri�meye �al��t���nda /Admin/Auth/Login sayfas�na y�nlendirilir.
                config.LogoutPath = new PathString("/Admin/Auth/Logout");

                config.Cookie = new CookieBuilder
                {
                    Name = "BlogApp",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                };

                config.SlidingExpiration  =true; //�erezin ge�erlilik s�resi her eri�imde yenilenir.
                config.ExpireTimeSpan  = TimeSpan.FromDays(7);//�erezin s�resi 7 g�n olarak belirlenir.
                config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");//Kullan�c�n�n yetkisiz bir sayfaya eri�meye �al��t���nda y�nlendirilece�i yolu belirtir

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseNToastNotify();// NToastNotify'yi kullanmas�n� sa�lar, bu da bildirimlerin istemci taraf�nda g�sterilmesine olanak tan�r.
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();//Oturum y�netimini etkinle�tirir. Bu, oturum verilerinin sunucu taraf�nda saklanmas�na olanak tan�r.

            app.UseRouting();


            app.UseAuthentication();//Kimlik do�rulamay� etkinle�tirir. Bu, kullan�c�lar�n kimliklerini do�rulamalar�n� gerektiren i�lemler i�in �nemlidir.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();
            });//Uygulama i�inde farkl� URL yollar�na (routes) belirli denetleyiciler ve eylemler atanmas�n� sa�lar. Burada, "Admin" adl� bir alan (area) i�in �zel bir yol yap�land�rmas� yap�l�r.


            app.Run();
        }
    }
}
