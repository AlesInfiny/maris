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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CatalogBrandId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    OrderedCatalogItemId = table.Column<long>(type: "bigint", nullable: false),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CatalogItemId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false)
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
                    { 1L, "b52dc7f712d94ca5812dd995bf926c04", "png" },
                    { 2L, "80bc8e167ccb4543b2f9d51913073492", "png" },
                    { 3L, "05d38fad5693422c8a27dd5b14070ec8", "png" },
                    { 4L, "45c22ba3da064391baac91341067ffe9", "png" },
                    { 5L, "4aed07c4ed5d45a5b97f11acedfbb601", "png" },
                    { 6L, "082b37439ecc44919626ba00fc60ee85", "png" },
                    { 7L, "f5f89954281747fa878129c29e1e0f83", "png" },
                    { 8L, "a8291ef2e8e14869a7048e272915f33c", "png" },
                    { 9L, "66237018c769478a90037bd877f5fba1", "png" },
                    { 10L, "d136d4c81b86478990984dcafbf08244", "png" },
                    { 11L, "47183f32f6584d7fb661f9216e11318b", "png" },
                    { 12L, "cf151206efd344e1b86854f4aa49fdef", "png" },
                    { 13L, "ab2e78eb7fe3408aadbf1e17a9945a8c", "png" },
                    { 14L, "0e557e96bc054f10bc91c27405a83e85", "png" },
                    { 15L, "e622b0098808492cb883831c05486b58", "png" }
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
                    { 1L, 3L, 1L, "定番の無地ロングTシャツです。", "クルーネック Tシャツ - ブラック", 1980m, "C000000001" },
                    { 2L, 2L, 1L, "暖かいのに着膨れしない起毛デニムです。", "裏起毛 スキニーデニム", 4800m, "C000000002" },
                    { 3L, 1L, 1L, "あたたかく肌ざわりも良いウール100%のロングコートです。", "ウールコート", 49800m, "C000000003" },
                    { 4L, 2L, 1L, "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", "無地 ボタンダウンシャツ", 2800m, "C000000004" },
                    { 5L, 3L, 2L, "コンパクトサイズのバッグですが収納力は抜群です", "レザーハンドバッグ", 18800m, "B000000001" },
                    { 6L, 2L, 2L, "エイジング加工したレザーを使用しています。", "ショルダーバッグ", 38000m, "B000000002" },
                    { 7L, 3L, 2L, "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", "トートバッグ ポーチ付き", 24800m, "B000000003" },
                    { 8L, 1L, 2L, "さらりと気軽に纏える、キュートなミニサイズショルダー。", "ショルダーバッグ", 2800m, "B000000004" },
                    { 9L, 1L, 2L, "エレガントな雰囲気を放つキルティングデザインです。", "レザー チェーンショルダーバッグ", 258000m, "B000000005" },
                    { 10L, 2L, 3L, "柔らかいソールは快適な履き心地で、ランニングに最適です。", "ランニングシューズ - ブルー", 12800m, "S000000001" },
                    { 11L, 1L, 3L, "イタリアの職人が丁寧に手作業で作り上げた一品です。", "メダリオン ストレートチップ ドレスシューズ", 23800m, "S000000002" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItemAssets",
                columns: new[] { "Id", "AssetCode", "CatalogItemId" },
                values: new object[,]
                {
                    { 1L, "45c22ba3da064391baac91341067ffe9", 1L },
                    { 2L, "4aed07c4ed5d45a5b97f11acedfbb601", 2L },
                    { 3L, "082b37439ecc44919626ba00fc60ee85", 3L },
                    { 4L, "f5f89954281747fa878129c29e1e0f83", 4L },
                    { 5L, "a8291ef2e8e14869a7048e272915f33c", 5L },
                    { 6L, "66237018c769478a90037bd877f5fba1", 6L },
                    { 7L, "d136d4c81b86478990984dcafbf08244", 7L },
                    { 8L, "47183f32f6584d7fb661f9216e11318b", 8L },
                    { 9L, "cf151206efd344e1b86854f4aa49fdef", 9L },
                    { 10L, "ab2e78eb7fe3408aadbf1e17a9945a8c", 10L },
                    { 11L, "0e557e96bc054f10bc91c27405a83e85", 11L }
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
