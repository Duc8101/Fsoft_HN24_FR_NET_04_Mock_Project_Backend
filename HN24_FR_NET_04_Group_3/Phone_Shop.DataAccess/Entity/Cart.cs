using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.DataAccess.Entity
{
    [Table("cart")]
    public partial class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User Customer { get; set; } = null!;
    }
}
