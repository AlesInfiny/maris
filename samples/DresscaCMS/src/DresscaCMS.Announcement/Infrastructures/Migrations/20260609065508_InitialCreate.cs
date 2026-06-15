using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DresscaCMS.Announcement.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PostDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpireDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DisplayPriority = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.CheckConstraint("CK_Announcement_DisplayPriority", "[DisplayPriority] IN (1, 2, 3, 4)");
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    LinkedUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementContents_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PostDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpireDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DisplayPriority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementHistory", x => x.Id);
                    table.CheckConstraint("CK_AnnouncementHistory_DisplayPriority", "[DisplayPriority] IN (1, 2, 3, 4)");
                    table.CheckConstraint("CK_AnnouncementHistory_OperationType", "[OperationType] IN (0, 1, 2,3)");
                    table.ForeignKey(
                        name: "FK_AnnouncementHistory_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementContentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    LinkedUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementContentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementContentHistory_AnnouncementHistory_AnnouncementHistoryId",
                        column: x => x.AnnouncementHistoryId,
                        principalTable: "AnnouncementHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Category", "ChangedAt", "CreatedAt", "DisplayPriority", "ExpireDateTime", "IsDeleted", "PostDateTime" },
                values: new object[,]
                {
                    { new Guid("0193ae97-b800-7001-8001-000000000001"), "一般", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000002"), "一般", new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000003"), "一般", new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000004"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000005"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000006"), "更新", new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000007"), "更新", new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000008"), "重要", new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000009"), "重要", new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000a"), "一般", new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000b"), "一般", new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000c"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000d"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000e"), "更新", new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-00000000000f"), "更新", new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 15, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000010"), "一般", new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000011"), "重要", new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000012"), "一般", new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000013"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000014"), "一般", new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000015"), "一般", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000016"), "一般", new DateTimeOffset(new DateTime(2011, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2010, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2019, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, new DateTimeOffset(new DateTime(2018, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000017"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7001-8001-000000000018"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementContents",
                columns: new[] { "Id", "AnnouncementId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("0193ae97-b800-7002-8001-000000000001"), new Guid("0193ae97-b800-7001-8001-000000000001"), "ja", "https://maris.alesinfiny.org/", "内容 1", "お知らせ 1" },
                    { new Guid("0193ae97-b800-7002-8001-000000000002"), new Guid("0193ae97-b800-7001-8001-000000000002"), "ja", "https://maris.alesinfiny.org/", "内容 2", "お知らせ 2" },
                    { new Guid("0193ae97-b800-7002-8001-000000000003"), new Guid("0193ae97-b800-7001-8001-000000000003"), "ja", "https://maris.alesinfiny.org/", "内容 3", "お知らせ 3" },
                    { new Guid("0193ae97-b800-7002-8001-000000000004"), new Guid("0193ae97-b800-7001-8001-000000000004"), "ja", "https://maris.alesinfiny.org/", "内容 4", "お知らせ 4" },
                    { new Guid("0193ae97-b800-7002-8001-000000000005"), new Guid("0193ae97-b800-7001-8001-000000000005"), "ja", "https://maris.alesinfiny.org/", "内容 5", "お知らせ 5" },
                    { new Guid("0193ae97-b800-7002-8001-000000000006"), new Guid("0193ae97-b800-7001-8001-000000000006"), "ja", "https://maris.alesinfiny.org/", "内容 6", "お知らせ 6" },
                    { new Guid("0193ae97-b800-7002-8001-000000000007"), new Guid("0193ae97-b800-7001-8001-000000000007"), "ja", "https://maris.alesinfiny.org/", "内容 7", "お知らせ 7" },
                    { new Guid("0193ae97-b800-7002-8001-000000000008"), new Guid("0193ae97-b800-7001-8001-000000000008"), "ja", "https://maris.alesinfiny.org/", "内容 8", "お知らせ 8" },
                    { new Guid("0193ae97-b800-7002-8001-000000000009"), new Guid("0193ae97-b800-7001-8001-000000000009"), "ja", "https://maris.alesinfiny.org/", "内容 9", "お知らせ 9" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000a"), new Guid("0193ae97-b800-7001-8001-00000000000a"), "ja", "https://maris.alesinfiny.org/", "内容 10", "お知らせ 10" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000b"), new Guid("0193ae97-b800-7001-8001-00000000000b"), "ja", "https://maris.alesinfiny.org/", "内容 11", "お知らせ 11" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000c"), new Guid("0193ae97-b800-7001-8001-00000000000c"), "ja", "https://maris.alesinfiny.org/", "内容 12", "お知らせ 12" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000d"), new Guid("0193ae97-b800-7001-8001-00000000000d"), "ja", "https://maris.alesinfiny.org/", "内容 13", "お知らせ 13" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000e"), new Guid("0193ae97-b800-7001-8001-00000000000e"), "ja", "https://maris.alesinfiny.org/", "内容 14", "お知らせ 14" },
                    { new Guid("0193ae97-b800-7002-8001-00000000000f"), new Guid("0193ae97-b800-7001-8001-00000000000f"), "ja", "https://maris.alesinfiny.org/", "内容 15", "お知らせ 15" },
                    { new Guid("0193ae97-b800-7002-8001-000000000010"), new Guid("0193ae97-b800-7001-8001-000000000010"), "ja", "https://maris.alesinfiny.org/", "内容 16", "お知らせ 16" },
                    { new Guid("0193ae97-b800-7002-8001-000000000011"), new Guid("0193ae97-b800-7001-8001-000000000011"), "ja", "https://maris.alesinfiny.org/", "内容 17", "お知らせ 17" },
                    { new Guid("0193ae97-b800-7002-8001-000000000012"), new Guid("0193ae97-b800-7001-8001-000000000012"), "es", "https://maris.alesinfiny.org/", "Detalles 18", "Anuncio 18" },
                    { new Guid("0193ae97-b800-7002-8001-000000000013"), new Guid("0193ae97-b800-7001-8001-000000000013"), "zh", "https://maris.alesinfiny.org/", "详情 19", "公告 19" },
                    { new Guid("0193ae97-b800-7002-8001-000000000014"), new Guid("0193ae97-b800-7001-8001-000000000014"), "en", "https://maris.alesinfiny.org/", "Details 20", "Notice 20" },
                    { new Guid("0193ae97-b800-7002-8001-000000000015"), new Guid("0193ae97-b800-7001-8001-000000000015"), "ja", "https://maris.alesinfiny.org/", "内容 21", "お知らせ 21" },
                    { new Guid("0193ae97-b800-7002-8001-000000000016"), new Guid("0193ae97-b800-7001-8001-000000000016"), "ja", "https://maris.alesinfiny.org/", "内容 削除済み", "お知らせ 削除済み" },
                    { new Guid("0193ae97-b800-7002-8001-000000000017"), new Guid("0193ae97-b800-7001-8001-000000000017"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("0193ae97-b800-7002-8001-000000000018"), new Guid("0193ae97-b800-7001-8001-000000000017"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("0193ae97-b800-7002-8001-000000000019"), new Guid("0193ae97-b800-7001-8001-000000000018"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("0193ae97-b800-7002-8001-00000000001a"), new Guid("0193ae97-b800-7001-8001-000000000018"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("0193ae97-b800-7002-8001-00000000001b"), new Guid("0193ae97-b800-7001-8001-000000000018"), "zh", "https://maris.alesinfiny.org/", "内容", "中国語 中文" },
                    { new Guid("0193ae97-b800-7002-8001-00000000001c"), new Guid("0193ae97-b800-7001-8001-000000000018"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementHistory",
                columns: new[] { "Id", "AnnouncementId", "Category", "ChangedBy", "CreatedAt", "DisplayPriority", "ExpireDateTime", "OperationType", "PostDateTime" },
                values: new object[,]
                {
                    { new Guid("0193ae97-b800-7003-8001-000000000001"), new Guid("0193ae97-b800-7001-8001-000000000001"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000002"), new Guid("0193ae97-b800-7001-8001-000000000002"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000003"), new Guid("0193ae97-b800-7001-8001-000000000003"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000004"), new Guid("0193ae97-b800-7001-8001-000000000004"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000005"), new Guid("0193ae97-b800-7001-8001-000000000005"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000006"), new Guid("0193ae97-b800-7001-8001-000000000006"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000007"), new Guid("0193ae97-b800-7001-8001-000000000007"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000008"), new Guid("0193ae97-b800-7001-8001-000000000008"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000009"), new Guid("0193ae97-b800-7001-8001-000000000009"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000a"), new Guid("0193ae97-b800-7001-8001-00000000000a"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000b"), new Guid("0193ae97-b800-7001-8001-00000000000b"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000c"), new Guid("0193ae97-b800-7001-8001-00000000000c"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000d"), new Guid("0193ae97-b800-7001-8001-00000000000d"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000e"), new Guid("0193ae97-b800-7001-8001-00000000000e"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-00000000000f"), new Guid("0193ae97-b800-7001-8001-00000000000f"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000010"), new Guid("0193ae97-b800-7001-8001-000000000010"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000011"), new Guid("0193ae97-b800-7001-8001-000000000011"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000012"), new Guid("0193ae97-b800-7001-8001-000000000012"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000013"), new Guid("0193ae97-b800-7001-8001-000000000013"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000014"), new Guid("0193ae97-b800-7001-8001-000000000014"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000015"), new Guid("0193ae97-b800-7001-8001-000000000015"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000016"), new Guid("0193ae97-b800-7001-8001-000000000017"), "テスト", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000017"), new Guid("0193ae97-b800-7001-8001-000000000018"), "テスト", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0193ae97-b800-7003-8001-000000000018"), new Guid("0193ae97-b800-7001-8001-000000000016"), "一般", "system", new DateTimeOffset(new DateTime(2011, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2019, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2018, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementContentHistory",
                columns: new[] { "Id", "AnnouncementHistoryId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("0193ae97-b800-7004-8001-000000000001"), new Guid("0193ae97-b800-7003-8001-000000000001"), "ja", "https://maris.alesinfiny.org/", "内容 1", "お知らせ 1" },
                    { new Guid("0193ae97-b800-7004-8001-000000000002"), new Guid("0193ae97-b800-7003-8001-000000000002"), "ja", "https://maris.alesinfiny.org/", "内容 2", "お知らせ 2" },
                    { new Guid("0193ae97-b800-7004-8001-000000000003"), new Guid("0193ae97-b800-7003-8001-000000000003"), "ja", "https://maris.alesinfiny.org/", "内容 3", "お知らせ 3" },
                    { new Guid("0193ae97-b800-7004-8001-000000000004"), new Guid("0193ae97-b800-7003-8001-000000000004"), "ja", "https://maris.alesinfiny.org/", "内容 4", "お知らせ 4" },
                    { new Guid("0193ae97-b800-7004-8001-000000000005"), new Guid("0193ae97-b800-7003-8001-000000000005"), "ja", "https://maris.alesinfiny.org/", "内容 5", "お知らせ 5" },
                    { new Guid("0193ae97-b800-7004-8001-000000000006"), new Guid("0193ae97-b800-7003-8001-000000000006"), "ja", "https://maris.alesinfiny.org/", "内容 6", "お知らせ 6" },
                    { new Guid("0193ae97-b800-7004-8001-000000000007"), new Guid("0193ae97-b800-7003-8001-000000000007"), "ja", "https://maris.alesinfiny.org/", "内容 7", "お知らせ 7" },
                    { new Guid("0193ae97-b800-7004-8001-000000000008"), new Guid("0193ae97-b800-7003-8001-000000000008"), "ja", "https://maris.alesinfiny.org/", "内容 8", "お知らせ 8" },
                    { new Guid("0193ae97-b800-7004-8001-000000000009"), new Guid("0193ae97-b800-7003-8001-000000000009"), "ja", "https://maris.alesinfiny.org/", "内容 9", "お知らせ 9" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000a"), new Guid("0193ae97-b800-7003-8001-00000000000a"), "ja", "https://maris.alesinfiny.org/", "内容 10", "お知らせ 10" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000b"), new Guid("0193ae97-b800-7003-8001-00000000000b"), "ja", "https://maris.alesinfiny.org/", "内容 11", "お知らせ 11" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000c"), new Guid("0193ae97-b800-7003-8001-00000000000c"), "ja", "https://maris.alesinfiny.org/", "内容 12", "お知らせ 12" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000d"), new Guid("0193ae97-b800-7003-8001-00000000000d"), "ja", "https://maris.alesinfiny.org/", "内容 13", "お知らせ 13" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000e"), new Guid("0193ae97-b800-7003-8001-00000000000e"), "ja", "https://maris.alesinfiny.org/", "内容 14", "お知らせ 14" },
                    { new Guid("0193ae97-b800-7004-8001-00000000000f"), new Guid("0193ae97-b800-7003-8001-00000000000f"), "ja", "https://maris.alesinfiny.org/", "内容 15", "お知らせ 15" },
                    { new Guid("0193ae97-b800-7004-8001-000000000010"), new Guid("0193ae97-b800-7003-8001-000000000010"), "ja", "https://maris.alesinfiny.org/", "内容 16", "お知らせ 16" },
                    { new Guid("0193ae97-b800-7004-8001-000000000011"), new Guid("0193ae97-b800-7003-8001-000000000011"), "ja", "https://maris.alesinfiny.org/", "内容 17", "お知らせ 17" },
                    { new Guid("0193ae97-b800-7004-8001-000000000012"), new Guid("0193ae97-b800-7003-8001-000000000012"), "es", "https://maris.alesinfiny.org/", "Detalles 18", "Anuncio 18" },
                    { new Guid("0193ae97-b800-7004-8001-000000000013"), new Guid("0193ae97-b800-7003-8001-000000000013"), "zh", "https://maris.alesinfiny.org/", "详情 19", "公告 19" },
                    { new Guid("0193ae97-b800-7004-8001-000000000014"), new Guid("0193ae97-b800-7003-8001-000000000014"), "en", "https://maris.alesinfiny.org/", "Details 20", "Notice 20" },
                    { new Guid("0193ae97-b800-7004-8001-000000000015"), new Guid("0193ae97-b800-7003-8001-000000000015"), "ja", "https://maris.alesinfiny.org/", "内容 21", "お知らせ 21" },
                    { new Guid("0193ae97-b800-7004-8001-000000000016"), new Guid("0193ae97-b800-7003-8001-000000000016"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("0193ae97-b800-7004-8001-000000000017"), new Guid("0193ae97-b800-7003-8001-000000000016"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("0193ae97-b800-7004-8001-000000000018"), new Guid("0193ae97-b800-7003-8001-000000000017"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("0193ae97-b800-7004-8001-000000000019"), new Guid("0193ae97-b800-7003-8001-000000000017"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("0193ae97-b800-7004-8001-00000000001a"), new Guid("0193ae97-b800-7003-8001-000000000017"), "zh", "https://maris.alesinfiny.org/", "内容", "中国語 中文" },
                    { new Guid("0193ae97-b800-7004-8001-00000000001b"), new Guid("0193ae97-b800-7003-8001-000000000017"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("0193ae97-b800-7004-8001-00000000001c"), new Guid("0193ae97-b800-7003-8001-000000000018"), "ja", "https://maris.alesinfiny.org/", "内容 削除済み", "お知らせ 削除済み" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementContentHistory_AnnouncementHistoryId",
                table: "AnnouncementContentHistory",
                column: "AnnouncementHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementContents_AnnouncementId",
                table: "AnnouncementContents",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementHistory_AnnouncementId",
                table: "AnnouncementHistory",
                column: "AnnouncementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementContentHistory");

            migrationBuilder.DropTable(
                name: "AnnouncementContents");

            migrationBuilder.DropTable(
                name: "AnnouncementHistory");

            migrationBuilder.DropTable(
                name: "Announcements");
        }
    }
}
