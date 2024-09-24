using Phone_Shop.Common.DTOs.CartDTO;

namespace Phone_Shop.Common.DTOs.OrderDTO
{
    public class OrderCreateDTO
    {
        public string Address { get; set; } = null!;

        public List<CartDetailDTO> CartDetailDTOs { get; set; } = new List<CartDetailDTO>();
    }
}
