using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.API.Attributes;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Products;
using System.ComponentModel.DataAnnotations;

namespace Phone_Shop.API.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {

        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("get-all-products")]
        public ResponseBase GetAll(string? name, decimal? priceFrom, decimal? priceTo, [Required] List<int> categoryIds, [Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return _service.GetAll(name, priceFrom, priceTo, categoryIds, pageSize, currentPage);
        }

        [HttpPost("[action]")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Create([Required] ProductCreateUpdateDTO DTO)
        {
            return _service.Create(DTO);
        }

        [HttpPut("[action]/{productId}")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Update([Required] int productId, [Required] ProductCreateUpdateDTO DTO)
        {
            return _service.Update(productId, DTO);
        }

        [HttpDelete("[action]/{productId}")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Delete([Required] int productId)
        {
            return _service.Delete(productId);
        }

        [HttpGet("get-top-products")]
        public ResponseBase GetTop([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return _service.GetTop(pageSize, currentPage);
        }

        [HttpGet("[action]/{productId}")]
        public ResponseBase Detail([Required] int productId)
        {
            return _service.Detail(productId);
        }
    }
}
