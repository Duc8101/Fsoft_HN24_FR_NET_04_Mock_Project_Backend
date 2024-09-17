using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Products
{
  public interface IProductService
  {
    Task<ResponseBase> Create(ProductCreateUpdateDTO DTO);
    Task<ResponseBase> Update(ProductCreateUpdateDTO DTO);
    Task<ResponseBase> Delete(int id);
    Task<ResponseBase> GetAll();
  }
}
