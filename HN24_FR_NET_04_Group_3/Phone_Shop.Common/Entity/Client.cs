using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("client")]
    public partial class Client : CommonEntity
    {
        public Client()
        {
            UserClients = new HashSet<UserClient>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("hardware_info")]
        [Required]
        public string HardwareInfo { get; set; } = null!;

        public virtual ICollection<UserClient> UserClients { get; set; }
    }
}
