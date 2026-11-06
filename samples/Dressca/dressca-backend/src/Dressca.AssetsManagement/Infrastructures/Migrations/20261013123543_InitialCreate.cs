using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dressca.AssetsManagement.Infrastructures.Migrations
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

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCode",
                table: "Assets",
                column: "AssetCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
