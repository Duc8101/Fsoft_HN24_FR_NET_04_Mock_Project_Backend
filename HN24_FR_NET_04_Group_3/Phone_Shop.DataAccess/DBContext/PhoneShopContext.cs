using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.Configuration;
using Phone_Shop.DataAccess.Entity;
using System.Globalization;

namespace Phone_Shop.DataAccess.DBContext
{
    public class PhoneShopContext : DbContext
    {

        public PhoneShopContext()
        {

        }

        public PhoneShopContext(DbContextOptions<PhoneShopContext> options)
           : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(WebConfig.SqlConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // --------------------------- seed data -----------------------------------
            string format = "yyyy-MM-dd HH:mm:ss.fffffff";

            // -------------------- role -------------------------
            List<Role> roles = new List<Role>()
            {
                new Role(){RoleId = 1 , RoleName = "Admin"},
                new Role(){RoleId = 2, RoleName = "Customer"},
            };
            modelBuilder.Entity<Role>().HasData(roles);

            // -------------------- user -------------------------
            List<User> users = new List<User>()
            {
                new User(){UserId = 1 ,FullName = "Chu Quang Quan", Phone = "8851738015", Email = "fellcock2@gmail.com", Address = null, Username = "QuangQuan", Password = "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", RoleId = roles[1].RoleId,CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563457", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563357", format, CultureInfo.InvariantCulture) },
                new User(){UserId = 2 ,FullName = "Nguyen Minh Duc", Phone = "5541282702", Email = "ducnm8101@gmail.com", Address = "Fpt Hòa Lạc", Username = "MinhDuc", Password = "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", RoleId = roles[1].RoleId,CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563460", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563360", format, CultureInfo.InvariantCulture) },
                new User(){UserId = 3 ,FullName = "Kirk Nelson", Phone = "4533389559", Email = "bkervin4@gmail.com", Address = null, Username = "Admin123", Password = "70db85967ceb5ab1d79060fe0e2fc536f02ca747086564989953385fe58cab7f", RoleId = roles[0].RoleId,CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563462", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563362", format, CultureInfo.InvariantCulture) },
                new User(){UserId = 4 ,FullName = "Nguyen Thi Thu", Phone = "0984739845", Email = "oparagreen0@gmail.com", Address = null, Username = "ThuThu", Password = "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", RoleId = roles[1].RoleId,CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563485", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563385", format, CultureInfo.InvariantCulture) },
                new User(){UserId = 5 ,FullName = "Nguyen Anh Tuan", Phone = "6298446654", Email = "", Address = null, Username = "AnhTuan", Password = "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", RoleId = roles[1].RoleId,CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563390", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563390", format, CultureInfo.InvariantCulture) },
            };
            modelBuilder.Entity<User>().HasData(users);

            // -------------------- category -------------------------
            List<Category> categories = new List<Category>()
            {
                new Category(){CategoryId = 1, CategoryName = "Samsung", CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563326", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563336", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Category(){CategoryId = 2, CategoryName = "Oppo", CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563339", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563339", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Category(){CategoryId = 3, CategoryName = "Iphone", CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563341", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563341", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Category(){CategoryId = 4, CategoryName = "Vivo", CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563342", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563342", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Category(){CategoryId = 5, CategoryName = "Nokia", CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563343", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563343", format, CultureInfo.InvariantCulture), IsDeleted = false},
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // -------------------- product -------------------------
            List<Product> products = new List<Product>()
            {
                new Product() {ProductId = 1, ProductName = "OPPO A16", Image = "https://cdn.tgdd.vn/Products/Images/42/240631/oppo-a16-silver-8-600x600.jpg", Price = 7.67m, CategoryId = categories[1].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563477", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563477", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 2, ProductName = "iPhone 11", Image = "https://cdn.tgdd.vn/Products/Images/42/153856/iphone-xi-xanhla-600x600.jpg", Price = 12.23m, CategoryId = categories[2].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563480", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563480", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 3, ProductName = "iPhone 13 Pro Max", Image = "https://cdn.tgdd.vn/Products/Images/42/230529/iphone-13-pro-max-gold-1-600x600.jpg", Price = 20.40m, CategoryId = categories[2].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563483", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563483", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 4, ProductName = "iPhone 12 Pro Max 512GB", Image = "https://cdn.tgdd.vn/Products/Images/42/228744/iphone-12-pro-max-xanh-duong-new-600x600-600x600.jpg", Price = 18.70m, CategoryId = categories[2].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563485", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563485", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 5, ProductName = "iPhone 13 Pro", Image = "https://cdn.tgdd.vn/Products/Images/42/230521/iphone-13-pro-sierra-blue-600x600.jpg", Price = 17.53m, CategoryId = categories[2].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563489", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563490", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 6, ProductName = "Vivo Y15 series", Image = "https://cdn.tgdd.vn/Products/Images/42/249720/Vivo-y15s-2021-xanh-den-660x600.jpg", Price = 6.86m, CategoryId = categories[3].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563492", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563492", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 7, ProductName = "Galaxy S22 Ultra 5G", Image = "https://cdn.tgdd.vn/Products/Images/42/235838/Galaxy-S22-Ultra-Burgundy-600x600.jpg", Price = 8.99m, CategoryId = categories[0].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563594", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563494", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 8, ProductName = "Vivo V21 5G", Image = "https://cdn.tgdd.vn/Products/Images/42/238047/vivo-v21-5g-xanh-den-600x600.jpg", Price = 5.56m, CategoryId = categories[3].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563596", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563496", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 9, ProductName = "Vivo Y55", Image = "https://cdn.tgdd.vn/Products/Images/42/278949/vivo-y55-den-thumb-600x600.jpg", Price = 10.23m, CategoryId = categories[3].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563597", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563498", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 10, ProductName = "Samsung Galaxy A03", Image = "https://cdn.tgdd.vn/Products/Images/42/251856/samsung-galaxy-a03-xanh-thumb-600x600.jpg", Price = 8.99m, CategoryId = categories[0].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563503", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563504", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 11, ProductName = "Samsung Galaxy A33 5G 6GB", Image = "https://cdn.tgdd.vn/Products/Images/42/246199/samsung-galaxy-a33-5g-xanh-thumb-600x600.jpg", Price = 5.50m, CategoryId = categories[0].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563505", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563506", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 12, ProductName = "Vivo Y53s", Image = "https://cdn.tgdd.vn/Products/Images/42/240286/vivo-y53s-xanh-600x600.jpg", Price = 4.65m, CategoryId = categories[3].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563507", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563508", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 13, ProductName = "Nokia G21", Image = "https://cdn.tgdd.vn/Products/Images/42/270207/nokia-g21-xanh-thumb-600x600.jpg", Price = 2.77m, CategoryId = categories[4].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563510", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563511", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 14, ProductName = "Nokia G11", Image = "https://cdn.tgdd.vn/Products/Images/42/272148/Nokia-g11-x%C3%A1m-thumb-600x600.jpg", Price = 1.99m, CategoryId = categories[4].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563512", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563513", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 15, ProductName = "OPPO Reno7 series", Image = "https://cdn.tgdd.vn/Products/Images/42/271717/oppo-reno7-z-5g-thumb-1-1-600x600.jpg", Price = 10.25m, CategoryId = categories[1].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563514", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563514", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 16, ProductName = "Nokia G10", Image = "https://cdn.tgdd.vn/Products/Images/42/235995/Nokia%20g10%20xanh%20duong-600x600.jpg", Price = 3.65m, CategoryId = categories[4].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563516", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563516", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 17, ProductName = "Nokia 215 4G", Image = "https://cdn.tgdd.vn/Products/Images/42/228366/nokia-215-xanh-ngoc-new-600x600-600x600.jpg", Price = 2.65m, CategoryId = categories[4].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563518", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563518", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 18, ProductName = "OPPO Find X5 Pro 5G", Image = "https://cdn.tgdd.vn/Products/Images/42/250622/oppo-find-x5-pro-den-thumb-600x600.jpg", Price = 10.35m, CategoryId = categories[1].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563520", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563521", format, CultureInfo.InvariantCulture), IsDeleted = false},
                new Product() {ProductId = 19, ProductName = "OPPO A76", Image = "https://cdn.tgdd.vn/Products/Images/42/270360/OPPO-A76-%C4%91en-600x600.jpg", Price = 7.99m, CategoryId = categories[1].CategoryId, Quantity = 10, CreatedAt = DateTime.ParseExact("2024-07-03 01:46:24.5563522", format, CultureInfo.InvariantCulture), UpdateAt = DateTime.ParseExact("2024-07-03 01:46:24.5563522", format, CultureInfo.InvariantCulture), IsDeleted = false},
            };
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
