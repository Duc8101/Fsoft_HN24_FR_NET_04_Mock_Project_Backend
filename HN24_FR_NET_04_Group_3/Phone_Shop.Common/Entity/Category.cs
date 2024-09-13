using Phone_Shop.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("category")]
    public partial class Category : CommonEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("category_name")]
        [Required]
        [MaxLength((int)CategoryMaxLength.Category_Name)]
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
