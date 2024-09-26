using BlogApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DAL.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> b)
        {
            // Primary key
            b.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            b.ToTable("AspNetUserRoles");

            b.HasData(new AppUserRole
            {
                UserId = Guid.Parse("B7F28859-F188-445B-884E-334883C9B01E"),
                RoleId = Guid.Parse("FEF13233-2EBD-479C-8D32-442AA674E754")
            },
            new AppUserRole
            {
                UserId = Guid.Parse("7DE80FB4-FC89-45AF-988D-4DBABEA6A811"),
                RoleId = Guid.Parse("6E6CB999-4940-4AC2-B628-EFFA18A8BAF7")
            }
            );
        }
    }
}
