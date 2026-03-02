using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [MaxLength(25)]
        public virtual string Id { get; set; }

        public virtual DateTime CreatedDateTime { get; set; }

        public virtual DateTime UpdatedDateTime { get; set; }

        public virtual int RowNumber { get; set; }

        public virtual bool Deleted { get; set; }

        [MaxLength(255)]
        public virtual string? CreatedBy { get; set; }

        [MaxLength(255)]
        public virtual string? LastUpdatedBy { get; set; }
    }
}
