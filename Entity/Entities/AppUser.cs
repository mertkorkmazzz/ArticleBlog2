using BlogApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity.Entities
{
    public class AppUser :IdentityUser<Guid> , IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid ImageId { get; set; } = Guid.Parse("ADF16BF4-7DD0-410A-8D1C-45C73DAE9E1E");
        public Image Image { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
