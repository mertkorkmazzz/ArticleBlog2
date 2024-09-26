using BlogApp.Core.Entities;
using BlogApp.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity.Entities
{
    public class Image : EntityBase
    {

        public Image()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public Image(string filename , string filetype  ,string createdby)
        {
            FileName = filename;
            FileType = filetype;
            CreatedBy = createdby;
        }



        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
      

        public ICollection<Article> Articles { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }

    }
}
