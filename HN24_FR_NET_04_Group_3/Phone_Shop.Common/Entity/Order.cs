using Phone_Shop.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("order")]
    public partial class Order : CommonEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("status")]
        [Required]
        [MaxLength((int)OrderMaxLength.Status)]
        public string Status { get; set; } = null!;

        [Column("address")]
        [Required]
        public string Address { get; set; } = null!;

        [Column("note")]
        public string? Note { get; set; }

        public virtual User Customer { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
