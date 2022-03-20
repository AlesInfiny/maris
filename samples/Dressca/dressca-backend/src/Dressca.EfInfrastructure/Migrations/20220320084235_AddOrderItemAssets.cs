using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    public partial class AddOrderItemAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemAssets_OrderItemId",
                table: "OrderItemAssets",
                column: "OrderItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemAssets");
        }
    }
}
