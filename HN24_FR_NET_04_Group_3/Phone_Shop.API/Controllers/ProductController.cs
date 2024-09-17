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

    [HttpPost("[action]")]
    [Authorize]
    [Role(Roles.Admin)]
    public async Task<ResponseBase> Create([Required] ProductCreateUpdateDTO DTO)
    {
      return await _service.Create(DTO);
    }
  }
}
