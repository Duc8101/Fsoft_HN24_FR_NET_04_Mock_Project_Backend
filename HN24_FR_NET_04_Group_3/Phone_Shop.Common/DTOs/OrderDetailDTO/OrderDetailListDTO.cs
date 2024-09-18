namespace Phone_Shop.Common.DTOs.OrderDetailDTO
{
    public class OrderDetailListDTO
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Image { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
