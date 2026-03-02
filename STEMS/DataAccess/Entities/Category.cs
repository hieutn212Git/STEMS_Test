using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Order { get; set; }

        public string ParrentId { get; set; }

        public bool IsGroup { get; set; }

        public string Description { get; set; }

        public Category() { }

        public Category(string id) : this()
        {
            Id = id;
        }
    }
}
