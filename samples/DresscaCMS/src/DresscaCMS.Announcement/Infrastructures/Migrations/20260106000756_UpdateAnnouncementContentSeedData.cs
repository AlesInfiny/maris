using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnnouncementContentSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444423"),
                column: "Title",
                value: "スペイン語 español");

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2112-2222-2222-222222222222"),
                column: "Title",
                value: "スペイン語 español");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444423"),
                column: "Title",
                value: "フランス語 français");

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2112-2222-2222-222222222222"),
                column: "Title",
                value: "フランス語 français");
        }
    }
}
