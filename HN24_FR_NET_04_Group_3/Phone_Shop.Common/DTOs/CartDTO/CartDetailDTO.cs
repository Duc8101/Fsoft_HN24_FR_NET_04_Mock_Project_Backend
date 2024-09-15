namespace Phone_Shop.Common.DTOs.CartDTO
{
    public class CartDetailDTO : CartCreateDTO
    {
        public int CartId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
