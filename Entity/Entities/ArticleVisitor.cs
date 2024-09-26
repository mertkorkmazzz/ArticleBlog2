using BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity.Entities
{
    public class ArticleVisitor : IEntityBase
    {
        //makale ile ziyaretçi arasındaki ilişkiyi temsil eder
        public ArticleVisitor()
        {
            
        }
        public ArticleVisitor(Guid articleId, int visitorId)
        {
            ArticleId = articleId;
            VisitorId = visitorId;
        }

        // hangi makale ile ilgili ilişkili olduğunu bulma için 
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
