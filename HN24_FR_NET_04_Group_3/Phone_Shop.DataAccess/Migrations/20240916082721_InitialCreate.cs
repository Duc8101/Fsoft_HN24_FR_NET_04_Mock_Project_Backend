using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Phone_Shop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hardware_info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_product_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "char(10)", true, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_cart_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_user_customer_id",
                        column: x => x.customer_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_order_user_customer_id",
                        column: x => x.customer_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_client",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expire_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_client", x => new { x.client_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_user_client_client_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_client_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_detail",
                columns: table => new
                {
                    order_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_detail", x => x.order_detail_id);
                    table.ForeignKey(
                        name: "FK_order_detail_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_detail_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_detail_id = table.Column<int>(type: "int", nullable: false),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    reply_id = table.Column<int>(type: "int", nullable: true),
                    rate = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.feedback_id);
                    table.ForeignKey(
                        name: "FK_feedback_feedback_reply_id",
                        column: x => x.reply_id,
                        principalTable: "feedback",
                        principalColumn: "feedback_id");
                    table.ForeignKey(
                        name: "FK_feedback_order_detail_order_detail_id",
                        column: x => x.order_detail_id,
                        principalTable: "order_detail",
                        principalColumn: "order_detail_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feedback_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "category_id", "category_name", "created_at", "is_deleted", "update_at" },
                values: new object[,]
                {
                    { 1, "Samsung", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3326), false, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3336) },
                    { 2, "Oppo", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3339), false, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3339) },
                    { 3, "Iphone", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3341), false, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3341) },
                    { 4, "Vivo", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3342), false, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3342) },
                    { 5, "Nokia", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3343), false, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3343) }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "role_id", "created_at", "is_deleted", "role_name", "update_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3385), false, "Admin", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3385) },
                    { 2, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3387), false, "Staff", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3387) },
                    { 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3389), false, "Customer", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3389) }
                });

            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "product_id", "category_id", "created_at", "description", "image", "is_deleted", "price", "product_name", "quantity", "update_at" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3477), null, "https://cdn.tgdd.vn/Products/Images/42/240631/oppo-a16-silver-8-600x600.jpg", false, 7.67m, "OPPO A16", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3477) },
                    { 2, 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3480), null, "https://cdn.tgdd.vn/Products/Images/42/153856/iphone-xi-xanhla-600x600.jpg", false, 12.23m, "iPhone 11", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3480) },
                    { 3, 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3483), null, "https://cdn.tgdd.vn/Products/Images/42/230529/iphone-13-pro-max-gold-1-600x600.jpg", false, 20.40m, "iPhone 13 Pro Max", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3483) },
                    { 4, 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3485), null, "https://cdn.tgdd.vn/Products/Images/42/228744/iphone-12-pro-max-xanh-duong-new-600x600-600x600.jpg", false, 18.70m, "iPhone 12 Pro Max 512GB", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3485) },
                    { 5, 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3489), null, "https://cdn.tgdd.vn/Products/Images/42/230521/iphone-13-pro-sierra-blue-600x600.jpg", false, 17.53m, "iPhone 13 Pro", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3490) },
                    { 6, 4, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3492), null, "https://cdn.tgdd.vn/Products/Images/42/249720/Vivo-y15s-2021-xanh-den-660x600.jpg", false, 6.86m, "Vivo Y15 series", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3492) },
                    { 7, 1, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3594), null, "https://cdn.tgdd.vn/Products/Images/42/235838/Galaxy-S22-Ultra-Burgundy-600x600.jpg", false, 8.99m, "Galaxy S22 Ultra 5G", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3494) },
                    { 8, 4, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3596), null, "https://cdn.tgdd.vn/Products/Images/42/238047/vivo-v21-5g-xanh-den-600x600.jpg", false, 5.56m, "Vivo V21 5G", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3496) },
                    { 9, 4, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3597), null, "https://cdn.tgdd.vn/Products/Images/42/278949/vivo-y55-den-thumb-600x600.jpg", false, 10.23m, "Vivo Y55", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3498) },
                    { 10, 1, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3503), null, "https://cdn.tgdd.vn/Products/Images/42/251856/samsung-galaxy-a03-xanh-thumb-600x600.jpg", false, 8.99m, "Samsung Galaxy A03", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3504) },
                    { 11, 1, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3505), null, "https://cdn.tgdd.vn/Products/Images/42/246199/samsung-galaxy-a33-5g-xanh-thumb-600x600.jpg", false, 5.50m, "Samsung Galaxy A33 5G 6GB", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3506) },
                    { 12, 4, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3507), null, "https://cdn.tgdd.vn/Products/Images/42/240286/vivo-y53s-xanh-600x600.jpg", false, 4.65m, "Vivo Y53s", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3508) },
                    { 13, 5, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3510), null, "https://cdn.tgdd.vn/Products/Images/42/270207/nokia-g21-xanh-thumb-600x600.jpg", false, 2.77m, "Nokia G21", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3511) },
                    { 14, 5, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3512), null, "https://cdn.tgdd.vn/Products/Images/42/272148/Nokia-g11-x%C3%A1m-thumb-600x600.jpg", false, 1.99m, "Nokia G11", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3513) },
                    { 15, 2, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3514), null, "https://cdn.tgdd.vn/Products/Images/42/271717/oppo-reno7-z-5g-thumb-1-1-600x600.jpg", false, 10.25m, "OPPO Reno7 series", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3514) },
                    { 16, 5, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3516), null, "https://cdn.tgdd.vn/Products/Images/42/235995/Nokia%20g10%20xanh%20duong-600x600.jpg", false, 3.65m, "Nokia G10", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3516) },
                    { 17, 5, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3518), null, "https://cdn.tgdd.vn/Products/Images/42/228366/nokia-215-xanh-ngoc-new-600x600-600x600.jpg", false, 2.65m, "Nokia 215 4G", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3518) },
                    { 18, 2, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3520), null, "https://cdn.tgdd.vn/Products/Images/42/250622/oppo-find-x5-pro-den-thumb-600x600.jpg", false, 10.35m, "OPPO Find X5 Pro 5G", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3521) },
                    { 19, 2, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3522), null, "https://cdn.tgdd.vn/Products/Images/42/270360/OPPO-A76-%C4%91en-600x600.jpg", false, 7.99m, "OPPO A76", 10, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3522) }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "user_id", "address", "created_at", "email", "full_name", "is_deleted", "password", "phone", "role_id", "update_at", "username" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3457), "fellcock2@gmail.com", "Chu Quang Quan", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", "8851738015", 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3357), "QuangQuan" },
                    { 2, "Fpt Hòa Lạc", new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3460), "ducnm8101@gmail.com", "Nguyen Minh Duc", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", "5541282702", 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3360), "MinhDuc" },
                    { 3, null, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3462), "bkervin4@gmail.com", "Kirk Nelson", false, "70db85967ceb5ab1d79060fe0e2fc536f02ca747086564989953385fe58cab7f", "4533389559", 1, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3362), "Admin123" },
                    { 4, null, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3485), "oparagreen0@gmail.com", "Nguyen Thi Thu", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", "0984739845", 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3385), "ThuThu" },
                    { 5, "FSoft Academy", new DateTime(2024, 9, 16, 15, 27, 21, 649, DateTimeKind.Local).AddTicks(7810), "DuocNQ1@fpt.com", "Nguyen Quoc Duoc", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", null, 2, new DateTime(2024, 9, 16, 15, 27, 21, 649, DateTimeKind.Local).AddTicks(7819), "DuocNQ1" },
                    { 6, null, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3390), "", "Nguyen Anh Tuan", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", "6298446654", 3, new DateTime(2024, 7, 3, 1, 46, 24, 556, DateTimeKind.Unspecified).AddTicks(3390), "AnhTuan" },
                    { 7, "FSoft Academy", new DateTime(2024, 9, 16, 15, 27, 21, 649, DateTimeKind.Local).AddTicks(7833), "LamLT1@fsoft.com", "Luu Tung Lam", false, "c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5", null, 2, new DateTime(2024, 9, 16, 15, 27, 21, 649, DateTimeKind.Local).AddTicks(7834), "LamLT1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_customer_id",
                table: "cart",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_product_id",
                table: "cart",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_creator_id",
                table: "feedback",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_order_detail_id",
                table: "feedback",
                column: "order_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_reply_id",
                table: "feedback",
                column: "reply_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_customer_id",
                table: "order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_order_id",
                table: "order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_product_id",
                table: "order_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_id",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_client_user_id",
                table: "user_client",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "user_client");

            migrationBuilder.DropTable(
                name: "order_detail");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
