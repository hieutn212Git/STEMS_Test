using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("UserRole")]
    public class UserRole : BaseEntity
    {
        [Column(TypeName = "varchar(25)")]
        public string UserId { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

        public UserRole() { }

        public UserRole(string id) : this()
        {
            Id = id;
        }
    }
}
