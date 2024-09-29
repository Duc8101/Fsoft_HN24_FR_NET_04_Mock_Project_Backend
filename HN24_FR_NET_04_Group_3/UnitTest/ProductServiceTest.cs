using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Phone_Shop.API;
using Phone_Shop.Common.DTOs.ProductDTO;
using Phone_Shop.DataAccess.DBContext;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTest
{
  public class ProductServiceTest
  {
    private PhoneShopContext _context;
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private ProductService _service;

    [SetUp]
    public void SetUp()
    {


      var mappingConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new MappingProfile());
      });

      _mapper = mappingConfig.CreateMapper();

      var options = new DbContextOptionsBuilder<PhoneShopContext>()
          .UseInMemoryDatabase(databaseName: "Test2Db")
          .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)) // Bỏ qua cảnh báo
          .Options;

      _context = new PhoneShopContext(options);
      _unitOfWork = new UnitOfWork(_context);
      _service = new ProductService(_unitOfWork, _mapper);
      _context.Database.EnsureDeleted();
      if (_context.Database.EnsureCreated())
      {
        var category = new Category
        {
          CategoryName = "Nokia"
          //BorrowingDetails = new List<BorrowingDetails>
          //      {
          //          new BorrowingDetails { Id = 1, BorrowingId = 1, Quantity = 3, NumberReturnedBook = 3 }, // Đã trả hết
          //          new BorrowingDetails { Id = 2, BorrowingId = 1, Quantity = 2, NumberReturnedBook = 0 }, // Chưa trả
          //          new BorrowingDetails { Id = 3, BorrowingId = 1, Quantity = 1, NumberReturnedBook = 1 }  // Trả đủ 1 quyển
          //      }
        };
        _context.Categories.Add(category);
        _context.SaveChanges();
        var product = new Product
        {
          CategoryId = 1,
          ProductName = "Test",
          Image = "Test",
          Price = 1111,
          Quantity = 111,
          IsDeleted = false
         };
        _context.Products.Add(product);
        _context.SaveChanges();

      }


    }
    [TearDown]
    public void TearDown()
    {
      _context.Dispose();
      _unitOfWork.Dispose();

    }




    [Test]
    public void CheckCreateProduct_ShouldAddReturnedProductAndSaveChanges()
    {

      var ProductDTO = new ProductCreateUpdateDTO
      {
        CategoryId = 1,
        ProductName = "Test1",
        Image = "Test1",
        Price = 1111,
        Quantity = 111,
      };


      var result = _service.Create(ProductDTO);
      Assert.AreEqual(true, result.Data);
    }
    [Test]
    public void UpdateReturnedProduct_ShouldUpdateReturnedBooksAndSaveChanges()
    {

      var ProductId = 2;
      var ProductDTO = new ProductCreateUpdateDTO
      {
        CategoryId = 1,
        ProductName = "Test2",
        Image = "Test2",
        Price = 1000,
        Quantity = 111,
      };

      // Act
      var result = _service.Update(ProductId, ProductDTO);

      // Assert
      var updatedProduct = _context.Products.FirstOrDefault(b => b.ProductId == ProductId);

      Assert.IsNotNull(updatedProduct, "Product should exist.");
      Assert.AreEqual(updatedProduct.ProductName, "Test2", "ProductName returned should be updated.");

    }
  }
}
