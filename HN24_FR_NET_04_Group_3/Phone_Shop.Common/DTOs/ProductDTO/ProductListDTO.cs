namespace Phone_Shop.Common.DTOs.ProductDTO
{
    public class ProductListDTO : ProductCreateUpdateDTO
    {

        public int ProductId { get; set; }

        public string CategoryName { get; set; } = null!;

    }
}
