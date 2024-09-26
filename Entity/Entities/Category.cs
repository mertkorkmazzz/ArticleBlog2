using BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity.Entities
{
    public class Category : EntityBase
    {

        public Category()
        {
            
        }
        public Category(string name ,string createBy)
        {
            Name = name;
            CreatedBy = createBy;
            
        }



        public Guid Id { get; set; }
        public string Name { get; set; }

        // buda bire çok bir iliki vardır article sınıfındaki makaleleri alır
        public ICollection<Article> Articles { get; set; }
    }
}
