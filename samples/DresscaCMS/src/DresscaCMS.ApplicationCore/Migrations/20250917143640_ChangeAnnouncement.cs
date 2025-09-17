using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DresscaCMS.ApplicationCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Announcements");

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("01182620-90c4-498e-b0a5-1a8b740ff421"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("06d6b6c6-906c-4de8-a02a-fd0f448d9d2f"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("3ed8bba8-fe95-477c-89ac-0abb586affae"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("450a2931-f066-4f64-920a-25d2804c4951"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("5030af08-13b9-478f-96d3-77e68225ebec"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("5ff8f1f1-29f2-4c55-ad12-90004bcb3003"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("704945e2-ab1f-4a46-b853-d14f09e38ed4"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("85355a45-24d1-4b1d-98cd-d073f2ccfa02"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("8721cd7a-f8cb-4d00-b5c8-53ce5eeac062"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("9eabc9d8-39af-472b-afbc-1f7e5ac61899"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("a2c651dc-e000-473e-b064-8a1e399f93a7"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("bf62d39d-338d-423e-9f49-e97605c8d157"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c738b93c-06ef-44be-9bb8-87fe0c3f6151"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c8180f90-fd55-4d81-9f1e-0946d5c3d75d"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c948318a-44cf-4e3a-b4a2-33a382570283"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cfdbc43c-fae1-4a43-9772-e3bef8c955dd"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("da1cc949-385e-44b3-a7d1-3f489cf2347d"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("e16b13a2-8698-4bb1-97dc-2fc64448e2fd"),
                columns: new[] { "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 9, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("f46e232c-ce77-408d-924e-aeaff126a639"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("f83041bb-acb0-4e73-a87f-60646616b6a3"),
                column: "PostDateTime",
                value: new DateTimeOffset(new DateTime(2024, 9, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ChangedAt",
                table: "Announcements",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Announcements",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("01182620-90c4-498e-b0a5-1a8b740ff421"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1631), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1631), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1630), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("06d6b6c6-906c-4de8-a02a-fd0f448d9d2f"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1565), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1564), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 10, 1, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1566), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1562), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("3ed8bba8-fe95-477c-89ac-0abb586affae"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1603), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("450a2931-f066-4f64-920a-25d2804c4951"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1597), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1596), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 10, 3, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1597), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("5030af08-13b9-478f-96d3-77e68225ebec"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1496), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1495), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 27, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1497), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1494), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("5ff8f1f1-29f2-4c55-ad12-90004bcb3003"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1607), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1607), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1606), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("704945e2-ab1f-4a46-b853-d14f09e38ed4"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1488), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1486), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1483), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("85355a45-24d1-4b1d-98cd-d073f2ccfa02"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1560), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1555), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1554), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("8721cd7a-f8cb-4d00-b5c8-53ce5eeac062"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1540), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1524), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1524), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("9eabc9d8-39af-472b-afbc-1f7e5ac61899"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1547), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1546), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 22, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1546), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("a2c651dc-e000-473e-b064-8a1e399f93a7"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1212), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1059), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 21, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(521), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 53, DateTimeKind.Unspecified).AddTicks(3957), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("bf62d39d-338d-423e-9f49-e97605c8d157"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1627), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1627), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 21, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1628), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1626), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c738b93c-06ef-44be-9bb8-87fe0c3f6151"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1492), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1491), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1490), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c8180f90-fd55-4d81-9f1e-0946d5c3d75d"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 10, 8, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1611), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1609), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("c948318a-44cf-4e3a-b4a2-33a382570283"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1594), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1593), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1592), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cfdbc43c-fae1-4a43-9772-e3bef8c955dd"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1638), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("da1cc949-385e-44b3-a7d1-3f489cf2347d"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1544), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1543), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1542), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("e16b13a2-8698-4bb1-97dc-2fc64448e2fd"),
                columns: new[] { "ChangedAt", "CreatedAt", "ExpireDateTime", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1634), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 29, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1635), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("f46e232c-ce77-408d-924e-aeaff126a639"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1551), new TimeSpan(0, 9, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("f83041bb-acb0-4e73-a87f-60646616b6a3"),
                columns: new[] { "ChangedAt", "CreatedAt", "PostDateTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1624), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1623), new TimeSpan(0, 9, 0, 0, 0)) });
        }
    }
}
