using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.API.Attributes;
using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Categories;
using System.ComponentModel.DataAnnotations;

namespace Phone_Shop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("get-categories-pagination")]
        public ResponseBase GetPagination(string? name, [Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return _service.GetPagination(name, pageSize, currentPage);
        }

        [HttpPost("[action]")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Create([Required] CategoryCreateUpdateDTO DTO)
        {
            return _service.Create(DTO);
        }

        [HttpPut("[action]/{categoryId}")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Update([Required] int categoryId, [Required] CategoryCreateUpdateDTO DTO)
        {
            return _service.Update(categoryId, DTO);
        }

        [HttpDelete("[action]/{categoryId}")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Delete([Required] int categoryId)
        {
            return _service.Delete(categoryId);
        }

        [HttpGet("get-all-categories")]
        public ResponseBase GetAll()
        {
            return _service.GetAll();
        }
    }
}
