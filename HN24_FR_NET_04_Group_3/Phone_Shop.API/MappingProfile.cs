using AutoMapper;
using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.DTOs.FeedbackDTO;
using Phone_Shop.Common.DTOs.OrderDetailDTO;
using Phone_Shop.Common.DTOs.OrderDTO;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.Common.DTOs.UserDTO;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.Helper;

namespace Phone_Shop.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterDTO, User>()
                .ForMember(des => des.FullName, map => map.MapFrom(src => src.FullName.Trim()))
                .ForMember(des => des.Email, map => map.MapFrom(src => src.Email.Trim()))
                .ForMember(des => des.Address, map => map.MapFrom(src => StringHelper.getStringValue(src.Address)))
                .ForMember(des => des.Password, map => map.MapFrom(src => UserHelper.HashPassword(src.Password)));

            CreateMap<User, UserDetailDTO>();

            CreateMap<Cart, CartDetailDTO>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.Product.ProductName))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Product.Image))
                .ForMember(d => d.Price, m => m.MapFrom(source => source.Product.Price));

            CreateMap<OrderCreateDTO, Order>()
                .ForMember(d => d.Address, m => m.MapFrom(source => source.Address.Trim()));

            CreateMap<CartDetailDTO, OrderDetail>();

            CreateMap<User, UserLoginInfoDTO>()
                .ForMember(des => des.RoleName, map => map.MapFrom(src => src.Role.RoleName));

            CreateMap<ProductCreateUpdateDTO, Product>()
                .ForMember(des => des.ProductName, map => map.MapFrom(src => src.ProductName.Trim()))
                .ForMember(des => des.Image, map => map.MapFrom(src => src.Image.Trim()))
                .ForMember(des => des.Description, map => map.MapFrom(src => StringHelper.getStringValue(src.Description)));
            
            CreateMap<Product, ProductListDTO>()
               .ForMember(des => des.CategoryName, map => map.MapFrom(src => src.Category.CategoryName));

            CreateMap<Order, OrderListDTO>()
                .ForMember(des => des.Username, map => map.MapFrom(src => src.Customer.Username))
                .ForMember(des => des.OrderDate, map => map.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd h:mm:ss tt")));

            CreateMap<OrderDetail, OrderDetailListDTO>();

            CreateMap<FeedbackCreateDTO, Feedback>()
                .ForMember(des => des.Comment, map => map.MapFrom(src => src.Comment.Trim()));

            CreateMap<FeedbackReplyDTO, Feedback>()
                .ForMember(des => des.Comment, map => map.MapFrom(src => src.Comment.Trim()));

            CreateMap<Category, CategoryListDTO>();

            CreateMap<CategoryCreateUpdateDTO, Category>()
                .ForMember(des => des.CategoryName, map => map.MapFrom(src => src.CategoryName.Trim()));
        }
    }
}
