using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DresscaCMS.Announcement.Migrations
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
                    { new Guid("11111111-1111-1111-1111-111111111111"), "一般", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "一般", new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "一般", new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111115"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111116"), "更新", new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111117"), "更新", new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111118"), "重要", new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111119"), "重要", new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111120"), "一般", new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111121"), "一般", new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111122"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111123"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111124"), "更新", new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111125"), "更新", new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 1, 15, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111126"), "一般", new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111127"), "重要", new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111128"), "一般", new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111129"), "イベント", new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111130"), "一般", new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-1111-1111-1111-111111111131"), "一般", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-2222-1111-1111-111111111111"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("11111111-3333-1111-1111-111111111111"), "テスト", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2100, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2030, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("19999999-1111-1111-1111-111111111111"), "一般", new DateTimeOffset(new DateTime(2011, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2010, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2019, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, new DateTimeOffset(new DateTime(2018, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementContents",
                columns: new[] { "Id", "AnnouncementId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("22222222-2111-2222-2222-222222222222"), new Guid("11111111-2222-1111-1111-111111111111"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("22222222-2112-2222-2222-222222222222"), new Guid("11111111-2222-1111-1111-111111111111"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("22222222-2222-2222-2222-222222222201"), new Guid("11111111-1111-1111-1111-111111111111"), "ja", "https://maris.alesinfiny.org/", "内容 1", "お知らせ 1" },
                    { new Guid("22222222-2222-2222-2222-222222222202"), new Guid("11111111-1111-1111-1111-111111111112"), "ja", "https://maris.alesinfiny.org/", "内容 2", "お知らせ 2" },
                    { new Guid("22222222-2222-2222-2222-222222222203"), new Guid("11111111-1111-1111-1111-111111111113"), "ja", "https://maris.alesinfiny.org/", "内容 3", "お知らせ 3" },
                    { new Guid("22222222-2222-2222-2222-222222222204"), new Guid("11111111-1111-1111-1111-111111111114"), "ja", "https://maris.alesinfiny.org/", "内容 4", "お知らせ 4" },
                    { new Guid("22222222-2222-2222-2222-222222222205"), new Guid("11111111-1111-1111-1111-111111111115"), "ja", "https://maris.alesinfiny.org/", "内容 5", "お知らせ 5" },
                    { new Guid("22222222-2222-2222-2222-222222222206"), new Guid("11111111-1111-1111-1111-111111111116"), "ja", "https://maris.alesinfiny.org/", "内容 6", "お知らせ 6" },
                    { new Guid("22222222-2222-2222-2222-222222222207"), new Guid("11111111-1111-1111-1111-111111111117"), "ja", "https://maris.alesinfiny.org/", "内容 7", "お知らせ 7" },
                    { new Guid("22222222-2222-2222-2222-222222222208"), new Guid("11111111-1111-1111-1111-111111111118"), "ja", "https://maris.alesinfiny.org/", "内容 8", "お知らせ 8" },
                    { new Guid("22222222-2222-2222-2222-222222222209"), new Guid("11111111-1111-1111-1111-111111111119"), "ja", "https://maris.alesinfiny.org/", "内容 9", "お知らせ 9" },
                    { new Guid("22222222-2222-2222-2222-222222222210"), new Guid("11111111-1111-1111-1111-111111111120"), "ja", "https://maris.alesinfiny.org/", "内容 10", "お知らせ 10" },
                    { new Guid("22222222-2222-2222-2222-222222222211"), new Guid("11111111-1111-1111-1111-111111111121"), "ja", "https://maris.alesinfiny.org/", "内容 11", "お知らせ 11" },
                    { new Guid("22222222-2222-2222-2222-222222222212"), new Guid("11111111-1111-1111-1111-111111111122"), "ja", "https://maris.alesinfiny.org/", "内容 12", "お知らせ 12" },
                    { new Guid("22222222-2222-2222-2222-222222222213"), new Guid("11111111-1111-1111-1111-111111111123"), "ja", "https://maris.alesinfiny.org/", "内容 13", "お知らせ 13" },
                    { new Guid("22222222-2222-2222-2222-222222222214"), new Guid("11111111-1111-1111-1111-111111111124"), "ja", "https://maris.alesinfiny.org/", "内容 14", "お知らせ 14" },
                    { new Guid("22222222-2222-2222-2222-222222222215"), new Guid("11111111-1111-1111-1111-111111111125"), "ja", "https://maris.alesinfiny.org/", "内容 15", "お知らせ 15" },
                    { new Guid("22222222-2222-2222-2222-222222222216"), new Guid("11111111-1111-1111-1111-111111111126"), "ja", "https://maris.alesinfiny.org/", "内容 16", "お知らせ 16" },
                    { new Guid("22222222-2222-2222-2222-222222222217"), new Guid("11111111-1111-1111-1111-111111111127"), "ja", "https://maris.alesinfiny.org/", "内容 17", "お知らせ 17" },
                    { new Guid("22222222-2222-2222-2222-222222222218"), new Guid("11111111-1111-1111-1111-111111111128"), "es", "https://maris.alesinfiny.org/", "Detalles 18", "Anuncio 18" },
                    { new Guid("22222222-2222-2222-2222-222222222219"), new Guid("11111111-1111-1111-1111-111111111129"), "zh", "https://maris.alesinfiny.org/", "详情 19", "公告 19" },
                    { new Guid("22222222-2222-2222-2222-222222222220"), new Guid("11111111-1111-1111-1111-111111111130"), "en", "https://maris.alesinfiny.org/", "Details 20", "Notice 20" },
                    { new Guid("22222222-2222-2222-2222-222222222221"), new Guid("11111111-1111-1111-1111-111111111131"), "ja", "https://maris.alesinfiny.org/", "内容 21", "お知らせ 21" },
                    { new Guid("22222222-3111-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("22222222-3112-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("22222222-3113-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "zh", "https://maris.alesinfiny.org/", "内容", "中国語 中文" },
                    { new Guid("22222222-3114-2222-2222-222222222222"), new Guid("11111111-3333-1111-1111-111111111111"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("29999999-2222-2222-2222-222222222222"), new Guid("19999999-1111-1111-1111-111111111111"), "ja", "https://maris.alesinfiny.org/", "内容 削除済み", "お知らせ 削除済み" }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementHistory",
                columns: new[] { "Id", "AnnouncementId", "Category", "ChangedBy", "CreatedAt", "DisplayPriority", "ExpireDateTime", "OperationType", "PostDateTime" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), new Guid("11111111-1111-1111-1111-111111111111"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333302"), new Guid("11111111-1111-1111-1111-111111111112"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333303"), new Guid("11111111-1111-1111-1111-111111111113"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333304"), new Guid("11111111-1111-1111-1111-111111111114"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333305"), new Guid("11111111-1111-1111-1111-111111111115"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333306"), new Guid("11111111-1111-1111-1111-111111111116"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333307"), new Guid("11111111-1111-1111-1111-111111111117"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333308"), new Guid("11111111-1111-1111-1111-111111111118"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333309"), new Guid("11111111-1111-1111-1111-111111111119"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333310"), new Guid("11111111-1111-1111-1111-111111111120"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333311"), new Guid("11111111-1111-1111-1111-111111111121"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333312"), new Guid("11111111-1111-1111-1111-111111111122"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333313"), new Guid("11111111-1111-1111-1111-111111111123"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333314"), new Guid("11111111-1111-1111-1111-111111111124"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333315"), new Guid("11111111-1111-1111-1111-111111111125"), "更新", "system", new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333316"), new Guid("11111111-1111-1111-1111-111111111126"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333317"), new Guid("11111111-1111-1111-1111-111111111127"), "重要", "system", new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333318"), new Guid("11111111-1111-1111-1111-111111111128"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333319"), new Guid("11111111-1111-1111-1111-111111111129"), "イベント", "system", new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333320"), new Guid("11111111-1111-1111-1111-111111111130"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-3333-3333-3333-333333333321"), new Guid("11111111-1111-1111-1111-111111111131"), "一般", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-4444-3333-3333-333333333301"), new Guid("11111111-2222-1111-1111-111111111111"), "テスト", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("33333333-5555-3333-3333-333333333301"), new Guid("11111111-3333-1111-1111-111111111111"), "テスト", "system", new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new DateTimeOffset(new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("39999999-3333-3333-3333-333333333333"), new Guid("19999999-1111-1111-1111-111111111111"), "一般", "system", new DateTimeOffset(new DateTime(2011, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2019, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new DateTimeOffset(new DateTime(2018, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

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
                    { new Guid("44444444-4444-4444-4444-444444444423"), new Guid("33333333-4444-3333-3333-333333333301"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("44444444-4444-4444-4444-444444444424"), new Guid("33333333-5555-3333-3333-333333333301"), "ja", "https://maris.alesinfiny.org/", "内容", "日本語" },
                    { new Guid("44444444-4444-4444-4444-444444444425"), new Guid("33333333-5555-3333-3333-333333333301"), "en", "https://maris.alesinfiny.org/", "内容", "英語 English" },
                    { new Guid("44444444-4444-4444-4444-444444444426"), new Guid("33333333-5555-3333-3333-333333333301"), "zh", "https://maris.alesinfiny.org/", "内容", "中国語 中文" },
                    { new Guid("44444444-4444-4444-4444-444444444427"), new Guid("33333333-5555-3333-3333-333333333301"), "es", "https://maris.alesinfiny.org/", "内容", "スペイン語 español" },
                    { new Guid("49999999-4444-4444-4444-444444444444"), new Guid("39999999-3333-3333-3333-333333333333"), "ja", "https://maris.alesinfiny.org/", "内容 削除済み", "お知らせ 削除済み" }
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
