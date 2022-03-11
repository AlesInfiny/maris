using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ShipToFullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ShipToPostalCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ShipToTodofuken = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ShipToShikuchoson = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ShipToAzanaAndOthers = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasketId = table.Column<long>(type: "bigint", nullable: false),
                    CatalogItemId = table.Column<long>(type: "bigint", nullable: false),
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CatalogCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CatalogBrandId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogBrands",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogCategories",
                        column: x => x.CatalogCategoryId,
                        principalTable: "CatalogCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderedCatalogItemId = table.Column<long>(type: "bigint", nullable: false),
                    OrderedProductName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    OrderedProductCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CatalogBrands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "高級なブランド" },
                    { 2L, "カジュアルなブランド" },
                    { 3L, "ノーブランド" }
                });

            migrationBuilder.InsertData(
                table: "CatalogCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "服" },
                    { 2L, "バッグ" },
                    { 3L, "シューズ" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItems",
                columns: new[] { "Id", "CatalogBrandId", "CatalogCategoryId", "Description", "Name", "Price", "ProductCode" },
                values: new object[,]
                {
                    { 1L, 3L, 1L, "定番の無地ロングTシャツです。", "クルーネック Tシャツ - グレー", 1980m, "C000000001" },
                    { 2L, 2L, 1L, "裏起毛で温かいパーカーです。", "無地 パーカー", 5800m, "C000000002" },
                    { 3L, 1L, 1L, "ウール生地のテーラードジャケットです。セットアップだけでなく単体でも使いやすい商品となっています。", "テーラードジャケット", 49800m, "C000000003" },
                    { 4L, 2L, 1L, "ファー襟付きのデニムジャケットです。", "デニムジャケット", 19800m, "C000000004" },
                    { 5L, 3L, 2L, "シンプルなデザインのトートバッグです。", "トートバッグ", 18800m, "B000000001" },
                    { 6L, 2L, 2L, "軽くしなやかなレザーを使用しています。", "ショルダーバッグ", 38000m, "B000000002" },
                    { 7L, 3L, 2L, "外側は高級感のある牛革、内側に丈夫で傷つきにくい豚革を採用した細身で持ち運びやすいビジネスバッグです。", "ビジネスバッグ", 24800m, "B000000003" },
                    { 8L, 1L, 2L, "丁寧に編み込まれた馬革ハンドバッグです。落ち着いた色調で、オールシーズン使いやすくなっています。", "編み込みトートバッグ", 58800m, "B000000004" },
                    { 9L, 1L, 2L, "卓越した素材と緻密な縫製作業によって、デザインが完璧に表現されています。", "ハンドバッグ", 258000m, "B000000005" },
                    { 10L, 2L, 3L, "定番のハイカットスニーカーです。", "ハイカットスニーカー - ブラック", 12800m, "S000000001" },
                    { 11L, 1L, 3L, "イタリアの職人が丁寧に手作業で作り上げた一品です。", "メダリオン ストレートチップ ドレスシューズ", 23800m, "S000000002" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

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
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "CatalogBrands");

            migrationBuilder.DropTable(
                name: "CatalogCategories");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
