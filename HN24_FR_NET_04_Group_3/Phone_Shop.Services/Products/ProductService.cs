using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Paging;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Linq.Expressions;
using System.Net;

namespace Phone_Shop.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }


        public ResponseBase Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input product name", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Image.Trim().Length == 0)
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

                Category? category = _unitOfWork.CategoryRepository.GetSingle(null, c => c.CategoryId == DTO.CategoryId && c.IsDeleted == false);
                if (category == null)
                {
                    return new ResponseBase($"Not found category with id = {DTO.CategoryId}", (int)HttpStatusCode.NotFound);
                }

                if (_unitOfWork.ProductRepository.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseBase($"Product name '{DTO.ProductName.Trim()}' already exists", (int)HttpStatusCode.Conflict);
                }

                Product product = _mapper.Map<Product>(DTO);
                product.CreatedAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDeleted = false;

                _unitOfWork.BeginTransaction();
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Create successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Delete(int productId)
        {
            try
            {
                Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase($"Not found product with id = {productId}", (int)HttpStatusCode.NotFound);
                }

                product.IsDeleted = true;

                _unitOfWork.BeginTransaction();
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Delete successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(int productId)
        {
            try
            {
                Product? product = _unitOfWork.ProductRepository.GetSingle(item => item.Include(p => p.Category), p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase($"Not found product with id = {productId}", (int)HttpStatusCode.NotFound);
                }

                ProductListDTO data = _mapper.Map<ProductListDTO>(product);

                // --------------------- set rate for  product ----------------------
                double? rate = _unitOfWork.FeedbackRepository.GetAll(null, null, f => f.OrderDetail.ProductId == productId).Average(f => f.Rate);
                data.Rate = rate == null ? 0 : Math.Round(rate.Value, 2);
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetAll(string? name, decimal? priceFrom, decimal? priceTo, List<int> categoryIds, int pageSize, int currentPage)
        {
            try
            {
                Func<IQueryable<Product>, IQueryable<Product>> include = item => item.Include(p => p.Category);
                Func<IQueryable<Product>, IQueryable<Product>> sort = item => item.OrderByDescending(p => p.UpdateAt);
                List<Expression<Func<Product, bool>>> predicates = new List<Expression<Func<Product, bool>>>()
                {
                    p => p.IsDeleted == false,
                };

                if (name != null && name.Trim().Length > 0)
                {
                    predicates.Add(p => p.ProductName.ToLower().Contains(name.ToLower().Trim()));
                }

                if (categoryIds.Count > 0)
                {
                    predicates.Add(p => categoryIds.Contains(p.CategoryId));
                }

                if (priceFrom.HasValue && priceTo.HasValue)
                {
                    predicates.Add(p => p.Price >= priceFrom && p.Price <= priceTo);
                }

                IQueryable<Product> query = _unitOfWork.ProductRepository.GetAll(include, sort, predicates.ToArray());
                List<ProductListDTO> list = query.Skip(pageSize * (currentPage - 1)).Take(pageSize).Select(p => _mapper.Map<ProductListDTO>(p))
                    .ToList();

                // --------------------- set rate for each product ----------------------
                list.ForEach(DTO =>
                {
                    double? rate = _unitOfWork.FeedbackRepository.GetAll(null, null, f => f.OrderDetail.ProductId == DTO.ProductId).Average(f => f.Rate);
                    DTO.Rate = rate == null ? 0 : Math.Round(rate.Value, 2);
                });

                Pagination<ProductListDTO> data = new Pagination<ProductListDTO>
                {
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    List = list,
                    TotalElement = query.Count()
                };
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetTop(int pageSize, int currentPage)
        {
            try
            {
                Func<IQueryable<Product>, IQueryable<Product>> include = item => item.Include(p => p.Category);

                Func<IQueryable<Product>, IQueryable<Product>> sort = item => item.OrderByDescending(p => p.OrderDetails
                .Where(od => od.Order.Status == OrderStatus.Done.ToString()).Sum(od => od.Quantity))
                .ThenByDescending(p => p.UpdateAt);

                IQueryable<Product> query = _unitOfWork.ProductRepository.GetAll(include, sort, p => p.IsDeleted == false);
                List<ProductListDTO> list = query.Skip(pageSize * (currentPage - 1)).Take(pageSize).Select(p => _mapper.Map<ProductListDTO>(p))
                    .ToList();

                // --------------------- set rate for each product ----------------------
                list.ForEach(DTO =>
                {
                    double? rate = _unitOfWork.FeedbackRepository.GetAll(null, null, f => f.OrderDetail.ProductId == DTO.ProductId).Average(f => f.Rate);
                    DTO.Rate = rate == null ? 0 : Math.Round(rate.Value, 2);
                });

                Pagination<ProductListDTO> data = new Pagination<ProductListDTO>
                {
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    List = list,
                    TotalElement = query.Count()
                };
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Update(int productId, ProductCreateUpdateDTO DTO)
        {
            try
            {

                Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase($"Not found product with id = {productId}", (int)HttpStatusCode.NotFound);
                }

                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input product name", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Image.Trim().Length == 0)
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

                Category? category = _unitOfWork.CategoryRepository.GetSingle(null, c => c.CategoryId == DTO.CategoryId && c.IsDeleted == false);
                if (category == null)
                {
                    return new ResponseBase($"Not found category with id = {DTO.CategoryId}", (int)HttpStatusCode.NotFound);
                }

                if (_unitOfWork.ProductRepository.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false && p.ProductId != productId))
                {
                    return new ResponseBase($"Product name '{DTO.ProductName.Trim()}' already exists", (int)HttpStatusCode.Conflict);
                }

                product.ProductName = DTO.ProductName.Trim();
                product.CategoryId = DTO.CategoryId;
                product.Price = DTO.Price;
                product.Image = DTO.Image.Trim();
                product.Description = product.Description == null || product.Description.Trim().Length == 0 ? null : product.Description.Trim();
                product.Quantity = DTO.Quantity;
                product.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Update successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
