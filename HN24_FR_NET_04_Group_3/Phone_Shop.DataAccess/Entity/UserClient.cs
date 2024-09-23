using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.DataAccess.Entity
{
    [Table("user_client")]
    public partial class UserClient : CommonEntity
    {

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("client_id")]
        public int ClientId { get; set; }

        [Required]
        [Column("token")]
        public string Token { get; set; } = null!;

        [Column("expire_date")]
        public DateTime ExpireDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
