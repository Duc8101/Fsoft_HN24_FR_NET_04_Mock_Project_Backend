using AutoMapper;
using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.Paging;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Linq.Expressions;
using System.Net;

namespace Phone_Shop.Services.Categories
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public ResponseBase Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                if (DTO.CategoryName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input category name", (int)HttpStatusCode.Conflict);
                }

                if (_unitOfWork.CategoryRepository.Any(p => p.CategoryName == DTO.CategoryName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseBase($"Category name '{DTO.CategoryName.Trim()}' already exists", (int)HttpStatusCode.Conflict);
                }

                Category category = _mapper.Map<Category>(DTO);
                category.CreatedAt = DateTime.Now;
                category.UpdateAt = DateTime.Now;
                category.IsDeleted = false;

                _unitOfWork.BeginTransaction();
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Create successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Delete(int categoryId)
        {
            try
            {
                Category? category = _unitOfWork.CategoryRepository.GetSingle(null, p => p.CategoryId == categoryId && p.IsDeleted == false);
                if (category == null)
                {
                    return new ResponseBase($"Not found category with id = {categoryId}", (int)HttpStatusCode.NotFound);
                }

                if (_unitOfWork.ProductRepository.Any(p => p.CategoryId == categoryId && p.IsDeleted == false))
                {
                    return new ResponseBase($"There still exists product in this category, delete those first!", (int)HttpStatusCode.NotFound);
                }

                category.IsDeleted = true;

                _unitOfWork.BeginTransaction();
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Delete successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(int categoryId)
        {
            try
            {
                Category? category = _unitOfWork.CategoryRepository.GetSingle(null, p => p.CategoryId == categoryId && p.IsDeleted == false);
                if (category == null)
                {
                    return new ResponseBase($"Not found category with id = {categoryId}", (int)HttpStatusCode.NotFound);
                }

                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                return new ResponseBase(true, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetAll(bool isAdmin)
        {
            try
            {
                Func<IQueryable<Category>, IQueryable<Category>>? sort;
                if (isAdmin)
                {
                    sort = item => item.OrderByDescending(c => c.UpdateAt);
                }
                else
                {
                    sort = null;
                }

                IQueryable<Category> query = _unitOfWork.CategoryRepository.GetAll(null, sort, c => c.IsDeleted == false);
                List<CategoryListDTO> data = query.Select(c => _mapper.Map<CategoryListDTO>(c)).ToList();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetPagination(string? name, int pageSize, int currentPage)
        {
            try
            {
                Func<IQueryable<Category>, IQueryable<Category>> sort = item => item.OrderByDescending(p => p.UpdateAt);
                List<Expression<Func<Category, bool>>> predicates = new List<Expression<Func<Category, bool>>>()
                {
                    c => c.IsDeleted == false,
                };

                if (name != null && name.Trim().Length > 0)
                {
                    predicates.Add(p => p.CategoryName == name.Trim());
                }

                IQueryable<Category> query = _unitOfWork.CategoryRepository.GetAll(null, sort, predicates.ToArray());
                List<CategoryListDTO> list = query.Skip(pageSize * (currentPage - 1)).Take(pageSize).Select(c => _mapper.Map<CategoryListDTO>(c)).ToList();
                Pagination<CategoryListDTO> data = new Pagination<CategoryListDTO>
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

        public ResponseBase Update(int categoryId, CategoryCreateUpdateDTO DTO)
        {
            try
            {

                Category? category = _unitOfWork.CategoryRepository.GetSingle(null, p => p.CategoryId == categoryId && p.IsDeleted == false);
                if (category == null)
                {
                    return new ResponseBase($"Not found category with id = {categoryId}", (int)HttpStatusCode.NotFound);
                }

                if (DTO.CategoryName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input category name", (int)HttpStatusCode.Conflict);
                }

                if (_unitOfWork.CategoryRepository.Any(p => p.CategoryName == DTO.CategoryName.Trim() && p.IsDeleted == false && p.CategoryId != categoryId))
                {
                    return new ResponseBase($"Category name '{DTO.CategoryName.Trim()}' already exists", (int)HttpStatusCode.Conflict);
                }

                category.CategoryName = DTO.CategoryName.Trim();
                category.UpdateAt = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.CategoryRepository.Update(category);
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
