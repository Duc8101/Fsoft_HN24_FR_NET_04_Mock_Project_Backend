namespace Phone_Shop.Common.DTOs.FeedbackDTO
{
    public class FeedbackListDTO
    {
        public int FeedbackId { get; set; }
        public string username { get; set; }
        public string Comment { get; set; } = null!;
        public int OrderDetailId { get; set; }
        public string CreatorName { get; set; } = null!;
        public string? RepliedName { get; set; }
        public int? Rate { get; set; }
        public string CreatedAt { get; set; }
        public List<FeedbackListDTO> FeedBackReplies { get; set; } = new List<FeedbackListDTO>();
    }
}
