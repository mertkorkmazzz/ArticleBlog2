using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public abstract class EntityBase : IEntityBase // CLASSLARI HİÇ BİR ZAMAN ÇIPLAK BIRAK MA İNTERFACE LER İLE KULLAN
    {


        public virtual Guid Id { get; set; }


        public virtual string CreatedBy { get; set; } = "Undefined";
        public virtual string? ModifiedBy { get; set; }
        public virtual string? DeletedBy { get; set; }
             

        public virtual DateTime CreatedDate { get; set; }=DateTime.Now;
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual DateTime? DeletedDate { get; set; }


        public virtual bool IsDeleted { get; set; }=false;
    }
}
