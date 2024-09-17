using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Shop.Services.Categories
{
  public interface ICategoryService
  {
    ResponseBase Create(CategoryCreateUpdateDTO DTO);
    ResponseBase Update(int categoryId, CategoryCreateUpdateDTO DTO);
    ResponseBase Delete(int categoryId);
    ResponseBase GetCategoriesPagination(string? name, int pageSize, int currentPage);
  }
}
