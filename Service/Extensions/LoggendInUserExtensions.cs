using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Extensions
{
    public static class LoggedInUserExtensions
    {

        //Bu kod, ASP.NET Core uygulamalarında oturum açmış (giriş yapmış) kullanıcının kimlik bilgilerine erişimi kolaylaştırmak
        //için yazılmış iki uzantı metodunu (extension method) içerir. Bu metodlar, ClaimsPrincipal sınıfına ek işlevler sağlar.
        //ClaimsPrincipal, ASP.NET Core'da oturum açmış kullanıcı hakkında bilgi içeren bir nesnedir ve bu nesne, kullanıcının kimliğini ve
        //rollerini temsil eden talepler (claims) içerir.


        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetLoggedInEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
