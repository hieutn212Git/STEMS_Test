using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string CategoryId { get; set; }

        public Category? Category { get; set; }

        public string Tags { get; set; }

        public Product() { }

        public Product(string id) : this()
        {
            Id = id;
        }
    }
}
