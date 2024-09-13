using Phone_Shop.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("feedback")]
    public class Feedback : CommonEntity
    {
        public Feedback()
        {
            FeedbackReplies = new HashSet<FeedbackReply>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("feedback_id")]
        public int FeedbackId { get; set; }

        [Column("content")]
        [Required]
        public string Content { get; set; } = null!;

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("rate")]
        [Range((int) FeedBackRate.Min, (int)FeedBackRate.Max)]
        public int Rate { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        public virtual ICollection<FeedbackReply> FeedbackReplies { get; set; }
    }
}
