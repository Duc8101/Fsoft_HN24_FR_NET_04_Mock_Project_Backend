using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    [Table("feedback_reply")]
    public class FeedbackReply : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("feedback_reply_id")]
        public int FeedbackReplyId { get; set; }

        [Column("feedback_id")]
        public int FeedbackId { get; set; }

        [Column("reply_id")]
        public int ReplyId;

        [Column("content")]
        [Required]
        public string Content { get; set; } = null!; 

        public virtual Feedback Feedback { get; set; } = null!;

        public virtual User Reply { get; set; } = null!;
    }
}
