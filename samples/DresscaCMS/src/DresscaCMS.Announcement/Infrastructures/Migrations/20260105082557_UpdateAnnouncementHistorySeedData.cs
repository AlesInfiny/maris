using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnnouncementHistorySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AnnouncementContentHistory",
                columns: new[] { "Id", "AnnouncementHistoryId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("33333333-3333-3333-3333-333333333301"), "ja", "https://maris.alesinfiny.org/", "内容 1", "お知らせ 1" },
                    { new Guid("44444444-4444-4444-4444-444444444402"), new Guid("33333333-3333-3333-3333-333333333302"), "ja", "https://maris.alesinfiny.org/", "内容 2", "お知らせ 2" },
                    { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("33333333-3333-3333-3333-333333333303"), "ja", "https://maris.alesinfiny.org/", "内容 3", "お知らせ 3" },
                    { new Guid("44444444-4444-4444-4444-444444444404"), new Guid("33333333-3333-3333-3333-333333333304"), "ja", "https://maris.alesinfiny.org/", "内容 4", "お知らせ 4" },
                    { new Guid("44444444-4444-4444-4444-444444444405"), new Guid("33333333-3333-3333-3333-333333333305"), "ja", "https://maris.alesinfiny.org/", "内容 5", "お知らせ 5" },
                    { new Guid("44444444-4444-4444-4444-444444444406"), new Guid("33333333-3333-3333-3333-333333333306"), "ja", "https://maris.alesinfiny.org/", "内容 6", "お知らせ 6" },
                    { new Guid("44444444-4444-4444-4444-444444444407"), new Guid("33333333-3333-3333-3333-333333333307"), "ja", "https://maris.alesinfiny.org/", "内容 7", "お知らせ 7" },
                    { new Guid("44444444-4444-4444-4444-444444444408"), new Guid("33333333-3333-3333-3333-333333333308"), "ja", "https://maris.alesinfiny.org/", "内容 8", "お知らせ 8" },
                    { new Guid("44444444-4444-4444-4444-444444444409"), new Guid("33333333-3333-3333-3333-333333333309"), "ja", "https://maris.alesinfiny.org/", "内容 9", "お知らせ 9" },
                    { new Guid("44444444-4444-4444-4444-444444444410"), new Guid("33333333-3333-3333-3333-333333333310"), "ja", "https://maris.alesinfiny.org/", "内容 10", "お知らせ 10" },
                    { new Guid("44444444-4444-4444-4444-444444444411"), new Guid("33333333-3333-3333-3333-333333333311"), "ja", "https://maris.alesinfiny.org/", "内容 11", "お知らせ 11" },
                    { new Guid("44444444-4444-4444-4444-444444444412"), new Guid("33333333-3333-3333-3333-333333333312"), "ja", "https://maris.alesinfiny.org/", "内容 12", "お知らせ 12" },
                    { new Guid("44444444-4444-4444-4444-444444444413"), new Guid("33333333-3333-3333-3333-333333333313"), "ja", "https://maris.alesinfiny.org/", "内容 13", "お知らせ 13" },
                    { new Guid("44444444-4444-4444-4444-444444444414"), new Guid("33333333-3333-3333-3333-333333333314"), "ja", "https://maris.alesinfiny.org/", "内容 14", "お知らせ 14" },
                    { new Guid("44444444-4444-4444-4444-444444444415"), new Guid("33333333-3333-3333-3333-333333333315"), "ja", "https://maris.alesinfiny.org/", "内容 15", "お知らせ 15" },
                    { new Guid("44444444-4444-4444-4444-444444444416"), new Guid("33333333-3333-3333-3333-333333333316"), "ja", "https://maris.alesinfiny.org/", "内容 16", "お知らせ 16" },
                    { new Guid("44444444-4444-4444-4444-444444444417"), new Guid("33333333-3333-3333-3333-333333333317"), "ja", "https://maris.alesinfiny.org/", "内容 17", "お知らせ 17" },
                    { new Guid("44444444-4444-4444-4444-444444444418"), new Guid("33333333-3333-3333-3333-333333333318"), "es", "https://maris.alesinfiny.org/", "Detalles 18", "Anuncio 18" },
                    { new Guid("44444444-4444-4444-4444-444444444419"), new Guid("33333333-3333-3333-3333-333333333319"), "zh", "https://maris.alesinfiny.org/", "详情 19", "公告 19" },
                    { new Guid("44444444-4444-4444-4444-444444444420"), new Guid("33333333-3333-3333-3333-333333333320"), "en", "https://maris.alesinfiny.org/", "Details 20", "Notice 20" },
                    { new Guid("44444444-4444-4444-4444-444444444421"), new Guid("33333333-3333-3333-3333-333333333321"), "ja", "https://maris.alesinfiny.org/", "内容 21", "お知らせ 21" },
                    { new Guid("44444444-4444-4444-4444-444444444422"), new Guid("33333333-4444-3333-3333-333333333301"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("44444444-4444-4444-4444-444444444423"), new Guid("33333333-4444-3333-3333-333333333301"), "es", "https://maris.alesinfiny.org/", "内容", "フランス語 français" },
                    { new Guid("44444444-4444-4444-4444-444444444424"), new Guid("33333333-5555-3333-3333-333333333301"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("44444444-4444-4444-4444-444444444425"), new Guid("33333333-5555-3333-3333-333333333301"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("44444444-4444-4444-4444-444444444426"), new Guid("33333333-5555-3333-3333-333333333301"), "zh", "https://maris.alesinfiny.org/", "内容", "中国語 中文" },
                    { new Guid("44444444-4444-4444-4444-444444444427"), new Guid("33333333-5555-3333-3333-333333333301"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444403"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444404"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444405"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444406"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444407"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444408"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444409"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444410"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444411"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444412"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444413"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444414"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444415"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444416"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444417"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444418"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444419"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444420"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444421"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444422"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444423"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444424"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444425"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444426"));

            migrationBuilder.DeleteData(
                table: "AnnouncementContentHistory",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444427"));
        }
    }
}
