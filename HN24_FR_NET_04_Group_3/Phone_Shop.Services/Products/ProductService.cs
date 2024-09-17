using AutoMapper;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Net;

namespace Phone_Shop.Services.Products
{
  public class ProductService : BaseService, IProductService
  {
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }


    public async Task<ResponseBase> Create(ProductCreateUpdateDTO DTO)
    {
      try
      {
        if (StringHelper.isStringNullOrEmpty(DTO.ProductName))
        {
          return new ResponseBase("You have to input product name", (int)HttpStatusCode.Conflict);
        }

        if (StringHelper.isStringNullOrEmpty(DTO.Image))
        {
          return new ResponseBase("You have to input product image", (int)HttpStatusCode.Conflict);
        }

        if (DTO.Price < 0)
        {
          return new ResponseBase("Price must be greater than or equal to 0", (int)HttpStatusCode.Conflict);
        }

        if (DTO.Quantity <= 0)
        {
          return new ResponseBase("Quantity must be greater than 0", (int)HttpStatusCode.Conflict);
        }

        Category? category = await _unitOfWork.CategoryRepository.GetSingleAsync(null, c => c.CategoryId == DTO.CategoryId && c.IsDeleted == false);
        if (category == null)
        {
          return new ResponseBase($"Not found category with id = {DTO.CategoryId}", (int)HttpStatusCode.NotFound);
        }

        if(await _unitOfWork.ProductRepository.AnyAsync(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
        {
          return new ResponseBase($"Product name '{DTO.ProductName.Trim()}' already exists", (int)HttpStatusCode.Conflict);
        }

        Product product = _mapper.Map<Product>(DTO);
        product.CreatedAt = DateTime.Now;
        product.UpdateAt = DateTime.Now;
        product.IsDeleted = false;
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.CommitAsync();
        return new ResponseBase(true);
      }
      catch (Exception ex)
      {
        await _unitOfWork.RollBackAsync();
        return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
      }
    }

    public async Task<ResponseBase> Update(ProductCreateUpdateDTO DTO)
    {
      throw new NotImplementedException();
    }

    public async Task<ResponseBase> Delete(int id)
    {
      try
      {
       /* Product product = _mapper.Map<Product>(DTO);
        _unitOfWork.ProductRepository.Add(product);
        _unitOfWork.Commit();*/
        return new ResponseBase(true);
      }
      catch (Exception ex)
      {
        _unitOfWork.RollBack();
        return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
      }
    }

    public async Task<ResponseBase> GetAll()
    {
      throw new NotImplementedException();
    }


  }
}
