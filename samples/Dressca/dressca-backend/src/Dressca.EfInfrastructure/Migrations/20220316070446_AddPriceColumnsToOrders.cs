using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    public partial class AddPriceColumnsToOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConsumptionTax",
                table: "Orders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsumptionTaxRate",
                table: "Orders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCharge",
                table: "Orders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalItemsPrice",
                table: "Orders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumptionTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ConsumptionTaxRate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCharge",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalItemsPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");
        }
    }
}
