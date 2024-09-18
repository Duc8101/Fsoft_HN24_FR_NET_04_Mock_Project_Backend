using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Categories
{
    public interface ICategoryService
    {
        ResponseBase Create(CategoryCreateUpdateDTO DTO);
        ResponseBase Update(int categoryId, CategoryCreateUpdateDTO DTO);
        ResponseBase Delete(int categoryId);
        ResponseBase GetPagination(string? name, int pageSize, int currentPage);
        ResponseBase GetAll(bool isAdmin);
        ResponseBase Detail(int categoryId);
    }
}
