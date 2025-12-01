using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DresscaCMS.Announcement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AnnouncementHistory",
                columns: new[] { "Id", "AnnouncementId", "Category", "ChangedBy", "CreatedAt", "DisplayPriority", "ExpireDateTime", "OperationType", "PostDateTime" },
                values: new object[] { new Guid("39999999-3333-3333-3333-333333333333"), new Guid("19999999-1111-1111-1111-111111111111"), "一般", "system", new DateTimeOffset(new DateTime(2011, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 11, new DateTimeOffset(new DateTime(2019, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2018, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "AnnouncementContentHistory",
                columns: new[] { "Id", "AnnouncementHistoryId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[] { new Guid("49999999-4444-4444-4444-444444444444"), new Guid("39999999-3333-3333-3333-333333333333"), "ja", "https://maris.alesinfiny.org/", "内容 削除済み", "お知らせ 削除済み" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("49999999-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "AnnouncementHistory",
                keyColumn: "Id",
                keyValue: new Guid("39999999-3333-3333-3333-333333333333"));
        }
    }
}
