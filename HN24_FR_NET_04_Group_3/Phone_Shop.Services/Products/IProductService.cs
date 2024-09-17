using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Products
{
  public interface IProductService
  {
    ResponseBase Create(ProductCreateUpdateDTO DTO);
    ResponseBase Update(int productId, ProductCreateUpdateDTO DTO);
    ResponseBase Delete(int productId);
    ResponseBase GetAll(string? name, int? categoryId, int pageSize, int currentPage);
  }
}
