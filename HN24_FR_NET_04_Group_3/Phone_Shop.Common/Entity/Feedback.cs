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
            InversionReply = new HashSet<Feedback>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("feedback_id")]
        public int FeedbackId { get; set; }

        [Column("comment")]
        [Required]
        public string Comment { get; set; } = null!;

        [Column("order_detail_id")]
        public int OrderDetailId { get; set; }

        [Column("creator_id")]
        public int CreatorId { get; set; }

        [Column("reply_id")]
        public int? ReplyId { get; set; }

        [Column("rate")]
        [Range((int) FeedBackRate.Min, (int)FeedBackRate.Max)]
        public int Rate { get; set; }

        public virtual OrderDetail OrderDetail { get; set; } = null!;
        public virtual User Creator { get; set; } = null!;
        public virtual Feedback? Reply { get; set; }

        public virtual ICollection<Feedback> InversionReply { get; set; }

    }
}
