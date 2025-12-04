using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class MultiLanguageMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222218"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "es", "Detalles 18", "Anuncio 18" });

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222219"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "zh", "详情 19", "公告 19" });

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222220"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "en", "Details 20", "Notice 20" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222218"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "ja", "内容 18", "お知らせ 18" });

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222219"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "ja", "内容 19", "お知らせ 19" });

            migrationBuilder.UpdateData(
                table: "AnnouncementContents",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222220"),
                columns: new[] { "LanguageCode", "Message", "Title" },
                values: new object[] { "ja", "内容 20", "お知らせ 20" });
        }
    }
}
