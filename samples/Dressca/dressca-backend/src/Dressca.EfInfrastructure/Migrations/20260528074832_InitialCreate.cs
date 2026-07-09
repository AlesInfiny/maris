using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dressca.EfInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ConsumptionTaxRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TotalItemsPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DeliveryCharge = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionTax = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ShipToFullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ShipToAzanaAndOthers = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShipToPostalCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ShipToShikuchoson = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ShipToTodofuken = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatalogItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItems_Baskets",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatalogBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogBrands",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogCategories",
                        column: x => x.CatalogCategoryId,
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedCatalogItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedProductCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OrderedProductName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CatalogItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemAssets_CatalogItems",
                        column: x => x.CatalogItemId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemAssets_OrderItems",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "AssetType" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7001-8001-000000000001"), "b52dc7f712d94ca5812dd995bf926c04", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000002"), "80bc8e167ccb4543b2f9d51913073492", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000003"), "05d38fad5693422c8a27dd5b14070ec8", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000004"), "45c22ba3da064391baac91341067ffe9", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000005"), "4aed07c4ed5d45a5b97f11acedfbb601", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000006"), "082b37439ecc44919626ba00fc60ee85", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000007"), "f5f89954281747fa878129c29e1e0f83", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000008"), "a8291ef2e8e14869a7048e272915f33c", "png" },
                    { new Guid("019b76da-a800-7001-8001-000000000009"), "66237018c769478a90037bd877f5fba1", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000a"), "d136d4c81b86478990984dcafbf08244", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000b"), "47183f32f6584d7fb661f9216e11318b", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000c"), "cf151206efd344e1b86854f4aa49fdef", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000d"), "ab2e78eb7fe3408aadbf1e17a9945a8c", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000e"), "0e557e96bc054f10bc91c27405a83e85", "png" },
                    { new Guid("019b76da-a800-7001-8001-00000000000f"), "e622b0098808492cb883831c05486b58", "png" }
                });

            migrationBuilder.InsertData(
                table: "CatalogBrands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7002-8001-000000000001"), "高級なブランド" },
                    { new Guid("019b76da-a800-7002-8001-000000000002"), "カジュアルなブランド" },
                    { new Guid("019b76da-a800-7002-8001-000000000003"), "ノーブランド" }
                });

            migrationBuilder.InsertData(
                table: "CatalogCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7003-8001-000000000001"), "服" },
                    { new Guid("019b76da-a800-7003-8001-000000000002"), "バッグ" },
                    { new Guid("019b76da-a800-7003-8001-000000000003"), "シューズ" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItems",
                columns: new[] { "Id", "CatalogBrandId", "CatalogCategoryId", "Description", "IsDeleted", "Name", "Price", "ProductCode" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7004-8001-000000000001"), new Guid("019b76da-a800-7002-8001-000000000003"), new Guid("019b76da-a800-7003-8001-000000000001"), "定番の無地ロングTシャツです。", false, "クルーネック Tシャツ - ブラック", 1980m, "C000000001" },
                    { new Guid("019b76da-a800-7004-8001-000000000002"), new Guid("019b76da-a800-7002-8001-000000000002"), new Guid("019b76da-a800-7003-8001-000000000001"), "暖かいのに着膨れしない起毛デニムです。", false, "裏起毛 スキニーデニム", 4800m, "C000000002" },
                    { new Guid("019b76da-a800-7004-8001-000000000003"), new Guid("019b76da-a800-7002-8001-000000000001"), new Guid("019b76da-a800-7003-8001-000000000001"), "あたたかく肌ざわりも良いウール100%のロングコートです。", false, "ウールコート", 49800m, "C000000003" },
                    { new Guid("019b76da-a800-7004-8001-000000000004"), new Guid("019b76da-a800-7002-8001-000000000002"), new Guid("019b76da-a800-7003-8001-000000000001"), "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", false, "無地 ボタンダウンシャツ", 2800m, "C000000004" },
                    { new Guid("019b76da-a800-7004-8001-000000000005"), new Guid("019b76da-a800-7002-8001-000000000003"), new Guid("019b76da-a800-7003-8001-000000000002"), "コンパクトサイズのバッグですが収納力は抜群です", false, "レザーハンドバッグ", 18800m, "B000000001" },
                    { new Guid("019b76da-a800-7004-8001-000000000006"), new Guid("019b76da-a800-7002-8001-000000000002"), new Guid("019b76da-a800-7003-8001-000000000002"), "エイジング加工したレザーを使用しています。", false, "ショルダーバッグ", 38000m, "B000000002" },
                    { new Guid("019b76da-a800-7004-8001-000000000007"), new Guid("019b76da-a800-7002-8001-000000000003"), new Guid("019b76da-a800-7003-8001-000000000002"), "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", false, "トートバッグ ポーチ付き", 24800m, "B000000003" },
                    { new Guid("019b76da-a800-7004-8001-000000000008"), new Guid("019b76da-a800-7002-8001-000000000001"), new Guid("019b76da-a800-7003-8001-000000000002"), "さらりと気軽に纏える、キュートなミニサイズショルダー。", false, "ショルダーバッグ", 2800m, "B000000004" },
                    { new Guid("019b76da-a800-7004-8001-000000000009"), new Guid("019b76da-a800-7002-8001-000000000001"), new Guid("019b76da-a800-7003-8001-000000000002"), "エレガントな雰囲気を放つキルティングデザインです。", false, "レザー チェーンショルダーバッグ", 258000m, "B000000005" },
                    { new Guid("019b76da-a800-7004-8001-00000000000a"), new Guid("019b76da-a800-7002-8001-000000000002"), new Guid("019b76da-a800-7003-8001-000000000003"), "柔らかいソールは快適な履き心地で、ランニングに最適です。", false, "ランニングシューズ - ブルー", 12800m, "S000000001" },
                    { new Guid("019b76da-a800-7004-8001-00000000000b"), new Guid("019b76da-a800-7002-8001-000000000001"), new Guid("019b76da-a800-7003-8001-000000000003"), "イタリアの職人が丁寧に手作業で作り上げた一品です。", false, "メダリオン ストレートチップ ドレスシューズ", 23800m, "S000000002" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItemAssets",
                columns: new[] { "Id", "AssetCode", "CatalogItemId" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7005-8001-000000000001"), "45c22ba3da064391baac91341067ffe9", new Guid("019b76da-a800-7004-8001-000000000001") },
                    { new Guid("019b76da-a800-7005-8001-000000000002"), "4aed07c4ed5d45a5b97f11acedfbb601", new Guid("019b76da-a800-7004-8001-000000000002") },
                    { new Guid("019b76da-a800-7005-8001-000000000003"), "082b37439ecc44919626ba00fc60ee85", new Guid("019b76da-a800-7004-8001-000000000003") },
                    { new Guid("019b76da-a800-7005-8001-000000000004"), "f5f89954281747fa878129c29e1e0f83", new Guid("019b76da-a800-7004-8001-000000000004") },
                    { new Guid("019b76da-a800-7005-8001-000000000005"), "a8291ef2e8e14869a7048e272915f33c", new Guid("019b76da-a800-7004-8001-000000000005") },
                    { new Guid("019b76da-a800-7005-8001-000000000006"), "66237018c769478a90037bd877f5fba1", new Guid("019b76da-a800-7004-8001-000000000006") },
                    { new Guid("019b76da-a800-7005-8001-000000000007"), "d136d4c81b86478990984dcafbf08244", new Guid("019b76da-a800-7004-8001-000000000007") },
                    { new Guid("019b76da-a800-7005-8001-000000000008"), "47183f32f6584d7fb661f9216e11318b", new Guid("019b76da-a800-7004-8001-000000000008") },
                    { new Guid("019b76da-a800-7005-8001-000000000009"), "cf151206efd344e1b86854f4aa49fdef", new Guid("019b76da-a800-7004-8001-000000000009") },
                    { new Guid("019b76da-a800-7005-8001-00000000000a"), "ab2e78eb7fe3408aadbf1e17a9945a8c", new Guid("019b76da-a800-7004-8001-00000000000a") },
                    { new Guid("019b76da-a800-7005-8001-00000000000b"), "0e557e96bc054f10bc91c27405a83e85", new Guid("019b76da-a800-7004-8001-00000000000b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCode",
                table: "Assets",
                column: "AssetCode");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemAssets_CatalogItemId",
                table: "CatalogItemAssets",
                column: "CatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogBrandId",
                table: "CatalogItems",
                column: "CatalogBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogCategoryId",
                table: "CatalogItems",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_ProductCode",
                table: "CatalogItems",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemAssets_OrderItemId",
                table: "OrderItemAssets",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "CatalogItemAssets");

            migrationBuilder.DropTable(
                name: "OrderItemAssets");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "CatalogBrands");

            migrationBuilder.DropTable(
                name: "CatalogCategories");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
