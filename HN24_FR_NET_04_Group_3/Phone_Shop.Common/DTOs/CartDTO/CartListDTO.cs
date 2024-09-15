namespace Phone_Shop.Common.DTOs.CartDTO
{
    public class CartListDTO
    {

        public string Customer { get; set; } = null!;

        public List<CartDetailDTO> CartDetailDTOs { get; set; } = new List<CartDetailDTO>();
    }
}
