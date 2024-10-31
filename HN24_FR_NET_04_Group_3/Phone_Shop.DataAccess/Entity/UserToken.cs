using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.DataAccess.Entity
{
    [Table("user_token")]
    public partial class UserToken : CommonEntity
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("token")]
        public string Token { get; set; } = null!;

        [Column("expire_date")]
        public DateTime ExpireDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
