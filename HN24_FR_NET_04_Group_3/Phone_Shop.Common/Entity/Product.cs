using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("product")]
    public partial class Product : CommonEntity
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
