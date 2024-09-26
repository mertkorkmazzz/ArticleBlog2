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

            builder.Services.LoadDataLayerExtensions(builder.Configuration);//servisleri eklemek için kullanýlýr
            builder.Services.LoadServiceLayerExtensions();//servisleri eklemek için kullanýlýr

            builder.Services.AddSession();

            // Add services to the container


            builder.Services.AddControllersWithViews()

                .AddNToastNotifyToastr(new ToastrOptions()
                {
                    PositionClass = ToastPositions.TopRight,
                    TimeOut = 3000
                }) // bildirim göstermek için kullanýlýr
                .AddRazorRuntimeCompilation(); // uygulamayý baþlatmadan deðiþikleri görmeye saðlar


            builder.Services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
            }) //ASP.NET Core Identity'yi yapýlandýrýr. AppUser ve AppRole sýnýflarýný kullanarak kullanýcý ve rol yönetimi saðlar. Þifre politikalarýný belirler: burada, þifrenin büyük harf, küçük harf veya özel karakter içermesi zorunlu deðildir.
            .AddRoleManager<RoleManager<AppRole>>() //Rol yönetimini saðlar, bu da uygulamanýzda kullanýcý rollerinin (örneðin, admin, kullanýcý) tanýmlanmasýna ve yönetilmesine olanak tanýr.
          .AddErrorDescriber<CustomIdentiyErorDescriber>()//Kimlik doðrulama hatalarýný özelleþtirmek için kullanýlýr. CustomIdentiyErorDescriber sýnýfý, varsayýlan hata mesajlarýný özelleþtirmek amacýyla oluþturulmuþ özel bir sýnýftýr.
            .AddEntityFrameworkStores<AppDbContext>()//Kullanýcý ve rol bilgilerini Entity Framework kullanarak belirlenen veritabanýnda saklar (AppDbContext).
            .AddDefaultTokenProviders();//Kullanýcý doðrulama, parola sýfýrlama gibi iþlemler için gereken varsayýlan token saðlayýcýlarýný ekler.


            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = new PathString("/Admin/Auth/Login"); //Kullanýcý oturum açma ve oturum kapatma yollarýný belirler. Örneðin, bir kullanýcý yetkilendirme gerektiren bir sayfaya eriþmeye çalýþtýðýnda /Admin/Auth/Login sayfasýna yönlendirilir.
                config.LogoutPath = new PathString("/Admin/Auth/Logout");

                config.Cookie = new CookieBuilder
                {
                    Name = "BlogApp",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                };

                config.SlidingExpiration  =true; //Çerezin geçerlilik süresi her eriþimde yenilenir.
                config.ExpireTimeSpan  = TimeSpan.FromDays(7);//Çerezin süresi 7 gün olarak belirlenir.
                config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");//Kullanýcýnýn yetkisiz bir sayfaya eriþmeye çalýþtýðýnda yönlendirileceði yolu belirtir

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseNToastNotify();// NToastNotify'yi kullanmasýný saðlar, bu da bildirimlerin istemci tarafýnda gösterilmesine olanak tanýr.
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();//Oturum yönetimini etkinleþtirir. Bu, oturum verilerinin sunucu tarafýnda saklanmasýna olanak tanýr.

            app.UseRouting();


            app.UseAuthentication();//Kimlik doðrulamayý etkinleþtirir. Bu, kullanýcýlarýn kimliklerini doðrulamalarýný gerektiren iþlemler için önemlidir.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();
            });//Uygulama içinde farklý URL yollarýna (routes) belirli denetleyiciler ve eylemler atanmasýný saðlar. Burada, "Admin" adlý bir alan (area) için özel bir yol yapýlandýrmasý yapýlýr.


            app.Run();
        }
    }
}
