using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    public partial class FKChangeToCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogBrands",
                table: "CatalogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogCategories",
                table: "CatalogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogBrands",
                table: "CatalogItems",
                column: "CatalogBrandId",
                principalTable: "CatalogBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogCategories",
                table: "CatalogItems",
                column: "CatalogCategoryId",
                principalTable: "CatalogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogBrands",
                table: "CatalogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogCategories",
                table: "CatalogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogBrands",
                table: "CatalogItems",
                column: "CatalogBrandId",
                principalTable: "CatalogBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogCategories",
                table: "CatalogItems",
                column: "CatalogCategoryId",
                principalTable: "CatalogCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
