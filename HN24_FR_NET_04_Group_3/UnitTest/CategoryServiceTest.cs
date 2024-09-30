using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Phone_Shop.API;
using Phone_Shop.Common.DTOs.CategoryDTO;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.DBContext;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Categories;

namespace UnitTest
{
  [TestFixture]
  public class CategoryServiceTest
  {
    private PhoneShopContext _context;
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private CategoryService _service;

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
      _service = new CategoryService(_unitOfWork, _mapper);
      _context.Database.EnsureDeleted();
      if (_context.Database.EnsureCreated())
      {
        var category = new Category
        {
          CategoryName = "Nokia"
        };
        _context.Categories.Add(category);
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
    public void CheckCreateCategory_ShouldAddReturnedCategoryAndSaveChanges()
    {

      var categoryDTO = new CategoryCreateUpdateDTO
      {
        CategoryName = "Test",

      };


      var result = _service.Create(categoryDTO);
      Assert.AreEqual(true, result.Data); 
    }
    [Test]
    public void UpdateReturnedCategory_ShouldUpdateReturnedBooksAndSaveChanges()
    {

      var categoryId = 2;
      var categoryDTO = new CategoryCreateUpdateDTO
      {
        CategoryName = "Test1",
      };

      // Act
      var result = _service.Update(categoryId, categoryDTO);

      // Assert
      var updatedCategory = _context.Categories.FirstOrDefault(b => b.CategoryId == categoryId);

      Assert.IsNotNull(updatedCategory, "Category should exist.");
      Assert.AreEqual(updatedCategory.CategoryName, "Test1", "CategoryName returned should be updated.");

    }



  }
}

