namespace Phone_Shop.Common.DTOs.OrderDTO
{
    public class OrderListDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Note { get; set; }
        public string OrderDate { get; set; } = null!;
    }
}
