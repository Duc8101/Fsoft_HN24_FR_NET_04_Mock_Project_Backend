using Phone_Shop.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.DataAccess.Entity
{
    [Table("user")]
    public partial class User : CommonEntity
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("full_name")]
        [Required]
        public string FullName { get; set; } = null!;

        [Column("phone")]
        [StringLength((int)UserLength.Phone)]
        public string? Phone { get; set; }

        [Column("email")]
        [Required]
        public string Email { get; set; } = null!;

        [Column("address")]
        public string? Address { get; set; }

        [Column("username")]
        [Required]
        [MaxLength((int)UserLength.Max_Username)]
        public string Username { get; set; } = null!;

        [Column("password")]
        [Required]
        public string Password { get; set; } = null!;

        [Column("role_id")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual UserToken? UserToken { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
