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
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category
            {
                Id = Guid.Parse("FD2A3A42-425C-4B00-BCC3-8BDFFE72BA87"),
                Name = "Asp.net core",
                CreatedBy = "1Admin Test",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            },
             new Category
             {
                 Id = Guid.Parse("35039143-2676-458A-BD92-DF4BB38B80D2"),
                 Name = "java ",
                 CreatedBy = "2Admin Test",
                 CreatedDate = DateTime.Now,
                 IsDeleted = false,
             }
            );
        }
    }
}
