using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dressca.Shopping.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "DisplayItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatalogItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisplayItems_CatalogItems",
                        column: x => x.CatalogItemId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    DisplayItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedDisplayItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                table: "DisplayItems",
                columns: new[] { "Id", "CatalogItemId" },
                values: new object[,]
                {
                    { new Guid("019b76da-a800-7006-8001-000000000001"), new Guid("019b76da-a800-7004-8001-000000000001") },
                    { new Guid("019b76da-a800-7006-8001-000000000002"), new Guid("019b76da-a800-7004-8001-000000000002") },
                    { new Guid("019b76da-a800-7006-8001-000000000003"), new Guid("019b76da-a800-7004-8001-000000000003") },
                    { new Guid("019b76da-a800-7006-8001-000000000004"), new Guid("019b76da-a800-7004-8001-000000000004") },
                    { new Guid("019b76da-a800-7006-8001-000000000005"), new Guid("019b76da-a800-7004-8001-000000000005") },
                    { new Guid("019b76da-a800-7006-8001-000000000006"), new Guid("019b76da-a800-7004-8001-000000000006") },
                    { new Guid("019b76da-a800-7006-8001-000000000007"), new Guid("019b76da-a800-7004-8001-000000000007") },
                    { new Guid("019b76da-a800-7006-8001-000000000008"), new Guid("019b76da-a800-7004-8001-000000000008") },
                    { new Guid("019b76da-a800-7006-8001-000000000009"), new Guid("019b76da-a800-7004-8001-000000000009") },
                    { new Guid("019b76da-a800-7006-8001-00000000000a"), new Guid("019b76da-a800-7004-8001-00000000000a") },
                    { new Guid("019b76da-a800-7006-8001-00000000000b"), new Guid("019b76da-a800-7004-8001-00000000000b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_DisplayItems_CatalogItemId",
                table: "DisplayItems",
                column: "CatalogItemId");

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
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "DisplayItems");

            migrationBuilder.DropTable(
                name: "OrderItemAssets");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
