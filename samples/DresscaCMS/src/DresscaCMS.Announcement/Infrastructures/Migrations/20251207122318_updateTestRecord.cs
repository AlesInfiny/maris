using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class updateTestRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-1111-1111-111111111111"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-3333-1111-1111-111111111111"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-1111-1111-111111111111"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-3333-1111-1111-111111111111"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
