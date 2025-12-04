using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class InsertTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Category", "ChangedAt", "CreatedAt", "DisplayPriority", "ExpireDateTime", "IsDeleted", "PostDateTime" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-1111-1111-111111111111"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-3333-1111-1111-111111111111"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementContents",
                columns: new[] { "Id", "AnnouncementId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("22222222-2111-2222-2222-222222222222"), new Guid("11111111-2222-1111-1111-111111111111"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("22222222-2112-2222-2222-222222222222"), new Guid("11111111-2222-1111-1111-111111111111"), "es", "https://maris.alesinfiny.org/", "内容", "フランス語 français" },
                    { new Guid("22222222-3111-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("22222222-3112-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("22222222-3113-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "zh", "https://maris.alesinfiny.org/", "内容", "フランス語 français" },
                    { new Guid("22222222-3114-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2111-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2112-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-3111-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-3112-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-3113-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-3114-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("11111111-3333-1111-1111-111111111111"));
        }
    }
}
