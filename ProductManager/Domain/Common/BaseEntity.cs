using System;

namespace ProductManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime? CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
