using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Role()
        {
        }

        public Role(string id) : this()
        {
            Id = id;
        }
    }
}
