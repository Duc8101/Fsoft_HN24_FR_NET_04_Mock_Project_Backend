using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("order_detail")]
    public partial class OrderDetail : CommonEntity
    {
        public OrderDetail() 
        {
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("order_detail_id")]
        public int OrderDetailId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_name")]
        [Required]
        public string ProductName { get; set; } = null!;

        [Column("image")]
        [Required]
        public string Image { get; set; } = null!;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        public virtual ICollection<Feedback> Feedbacks { get; set;}
    }
}
