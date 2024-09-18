namespace Phone_Shop.Common.DTOs.FeedbackDTO
{
    public class FeedbackCreateDTO
    {
        public string Comment { get; set; } = null!;

        public int OrderDetailId { get; set; }

        public int Rate { get; set; }
    }
}
