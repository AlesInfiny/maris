using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DresscaCMS.ApplicationCore.Migrations
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
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpireDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DisplayPriority = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementsContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementsContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementsContent_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Category", "ChangedAt", "CreatedAt", "DisplayPriority", "ExpireDateTime", "IsDeleted", "PostDateTime" },
                values: new object[,]
                {
                    { new Guid("01182620-90c4-498e-b0a5-1a8b740ff421"), "障害情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1631), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1631), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1630), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("06d6b6c6-906c-4de8-a02a-fd0f448d9d2f"), "イベント情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1565), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1564), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 10, 1, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1566), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1562), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("3ed8bba8-fe95-477c-89ac-0abb586affae"), "障害情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1603), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("450a2931-f066-4f64-920a-25d2804c4951"), "会社情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1597), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1596), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 10, 3, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1597), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("5030af08-13b9-478f-96d3-77e68225ebec"), "イベント情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1496), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1495), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 9, 27, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1497), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1494), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("5ff8f1f1-29f2-4c55-ad12-90004bcb3003"), "新機能・サービス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1607), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1607), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1606), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("704945e2-ab1f-4a46-b853-d14f09e38ed4"), "障害情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1488), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1486), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1483), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("85355a45-24d1-4b1d-98cd-d073f2ccfa02"), "新機能・サービス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1560), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1555), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1554), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("8721cd7a-f8cb-4d00-b5c8-53ce5eeac062"), "会社情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1540), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1524), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1524), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("9eabc9d8-39af-472b-afbc-1f7e5ac61899"), "メンテナンス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1547), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1546), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 9, 22, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1546), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("a2c651dc-e000-473e-b064-8a1e399f93a7"), "メンテナンス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1212), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1059), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 9, 21, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(521), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 53, DateTimeKind.Unspecified).AddTicks(3957), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("bf62d39d-338d-423e-9f49-e97605c8d157"), "メンテナンス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1627), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1627), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 9, 21, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1628), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1626), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("c738b93c-06ef-44be-9bb8-87fe0c3f6151"), "新機能・サービス", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1492), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1491), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1490), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("c8180f90-fd55-4d81-9f1e-0946d5c3d75d"), "イベント情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 10, 8, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1611), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1609), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("c948318a-44cf-4e3a-b4a2-33a382570283"), "会社情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1594), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1593), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1592), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("cfdbc43c-fae1-4a43-9772-e3bef8c955dd"), "重要なお知らせ", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1638), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("da1cc949-385e-44b3-a7d1-3f489cf2347d"), "会社情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1544), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1543), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1542), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("e16b13a2-8698-4bb1-97dc-2fc64448e2fd"), "イベント情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1634), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 9, 0, 0, 0)), (short)3, new DateTimeOffset(new DateTime(2025, 9, 29, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1635), new TimeSpan(0, 9, 0, 0, 0)), false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("f46e232c-ce77-408d-924e-aeaff126a639"), "障害情報", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1551), new TimeSpan(0, 9, 0, 0, 0)) },
                    { new Guid("f83041bb-acb0-4e73-a87f-60646616b6a3"), "重要なお知らせ", new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 9, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1624), new TimeSpan(0, 9, 0, 0, 0)), (short)3, null, false, new DateTimeOffset(new DateTime(2025, 9, 17, 23, 26, 15, 56, DateTimeKind.Unspecified).AddTicks(1623), new TimeSpan(0, 9, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "AnnouncementsContent",
                columns: new[] { "Id", "AnnouncementId", "LanguageCode", "LinkedUrl", "Message", "Title" },
                values: new object[,]
                {
                    { new Guid("a1e2b3c4-5d6f-4a7b-8c9d-0e1f2a3b4c5d"), new Guid("a2c651dc-e000-473e-b064-8a1e399f93a7"), "jp", "", "9月25日午前2時から5時まで、システムメンテナンスのためサービスを一時停止いたします。ご利用のお客様にはご不便をおかけいたします。", "メンテナンスのお知らせ" },
                    { new Guid("a3b4c5d6-7e8f-4a9b-0c1d-2e3f4a5b6c7d"), new Guid("3ed8bba8-fe95-477c-89ac-0abb586affae"), "jp", "", "決済システムに遅延が発生しており、処理に時間がかかる状況です。復旧作業中のため、完了次第改めてお知らせいたします。", "障害情報（決済）" },
                    { new Guid("a7b8c9d0-1e2f-4a3b-4c5d-6e7f8a9b0c1d"), new Guid("9eabc9d8-39af-472b-afbc-1f7e5ac61899"), "jp", "", "定期メンテナンスを9月30日22時より実施いたします。作業中はログインや一部機能がご利用いただけませんので、あらかじめご了承ください。", "メンテナンスのお知らせ" },
                    { new Guid("a9b0c1d2-3e4f-4a5b-6c7d-8e9f0a1b2c3d"), new Guid("e16b13a2-8698-4bb1-97dc-2fc64448e2fd"), "jp", "", "12月1日より「冬の感謝フェア」を開催いたします。限定グッズの配布や特典をご用意しておりますので、ぜひご参加ください。", "冬の感謝フェアーのご案内" },
                    { new Guid("b0c1d2e3-4f5a-4b6c-7d8e-9f0a1b2c3d4e"), new Guid("cfdbc43c-fae1-4a43-9772-e3bef8c955dd"), "jp", "", "現在の天候不良の影響により、一部地域で商品のお届けに遅延が発生しております。ご利用のお客様にはご迷惑をおかけしますが、何卒ご了承ください。", "天候不良に伴う配送遅延について" },
                    { new Guid("b2c3d4e5-6f7a-4b8c-9d0e-1f2a3b4c5d6e"), new Guid("704945e2-ab1f-4a46-b853-d14f09e38ed4"), "jp", "", "現在、一部のユーザーにおいてログインできない障害が発生しております。原因を調査中であり、復旧まで今しばらくお待ちください。", "障害情報（ログイン）" },
                    { new Guid("b4c5d6e7-8f9a-4b0c-1d2e-3f4a5b6c7d8e"), new Guid("5ff8f1f1-29f2-4c55-ad12-90004bcb3003"), "jp", "", "スマートフォンアプリに「プッシュ通知」機能を追加しました。最新情報をリアルタイムでお届けしますのでぜひご活用ください。", "プッシュ通知機能追加のお知らせ" },
                    { new Guid("b8c9d0e1-2f3a-4b4c-5d6e-7f8a9b0c1d2e"), new Guid("f46e232c-ce77-408d-924e-aeaff126a639"), "jp", "", "メール送信システムに不具合が発生し、通知メールが届かない状況です。復旧対応を進めておりますのでご不便をおかけいたします。", "障害情報（メール送信）" },
                    { new Guid("c3d4e5f6-7a8b-4c9d-0e1f-2a3b4c5d6e7f"), new Guid("c738b93c-06ef-44be-9bb8-87fe0c3f6151"), "jp", "", "新しい会員向けダッシュボード機能を公開しました。利用状況やお知らせを一目で確認できるようになり、利便性が向上しました。", "ダッシュボード機能追加のお知らせ" },
                    { new Guid("c5d6e7f8-9a0b-4c1d-2e3f-4a5b6c7d8e9f"), new Guid("c8180f90-fd55-4d81-9f1e-0946d5c3d75d"), "jp", "", "当社主催のユーザー交流イベントを11月15日に東京本社で開催いたします。詳細や参加方法は後日お知らせいたします。", "ユーザー交流イベントのご案内" },
                    { new Guid("c9d0e1f2-3a4b-4c5d-6e7f-8a9b0c1d2e3f"), new Guid("85355a45-24d1-4b1d-98cd-d073f2ccfa02"), "jp", "", "本日よりオンライン決済にクレジットカード以外に電子マネー決済が追加されました。ぜひ新しい決済方法をご利用ください。", "決済方法追加のお知らせ" },
                    { new Guid("d0e1f2a3-4b5c-4d6e-7f8a-9b0c1d2e3f4a"), new Guid("06d6b6c6-906c-4de8-a02a-fd0f448d9d2f"), "jp", "", "秋のキャンペーンイベントを10月1日より実施いたします。期間中は特別割引や限定コンテンツを用意しておりますのでご期待ください。", "秋のキャンペーンイベントのご案内" },
                    { new Guid("d4e5f6a7-8b9c-4d0e-1f2a-3b4c5d6e7f8a"), new Guid("5030af08-13b9-478f-96d3-77e68225ebec"), "jp", "", "10月10日に開催されるオンラインセミナー「最新クラウド活用術」の参加受付を開始しました。ぜひお申し込みください。", "オンラインセミナーのご案内" },
                    { new Guid("d6e7f8a9-0b1c-4d2e-3f4a-5b6c7d8e9f0a"), new Guid("f83041bb-acb0-4e73-a87f-60646616b6a3"), "jp", "", "現在、当社を装った不審なメールが確認されております。添付ファイルの開封やURLのクリックは行わず、削除していただきますようお願いいたします。", "当社をかたる不審なメールへのご注意" },
                    { new Guid("e1f2a3b4-5c6d-4e7f-8a9b-0c1d2e3f4a5b"), new Guid("c948318a-44cf-4e3a-b4a2-33a382570283"), "jp", "", "2026年度新卒採用エントリーの受付を開始しました。募集要項や応募方法の詳細は採用情報ページをご確認ください。", "2026年度新卒採用について" },
                    { new Guid("e5f6a7b8-9c0d-4e1f-2a3b-4c5d6e7f8a9b"), new Guid("8721cd7a-f8cb-4d00-b5c8-53ce5eeac062"), "jp", "", "本社オフィスは11月1日より新住所へ移転いたします。詳細な所在地やアクセス方法は会社概要ページをご確認ください。", "本社移転のお知らせ" },
                    { new Guid("e7f8a9b0-1c2d-4e3f-4a5b-6c7d8e9f0a1b"), new Guid("bf62d39d-338d-423e-9f49-e97605c8d157"), "jp", "", "サーバー保守作業のため、10月5日深夜にサービスが断続的にご利用いただけなくなります。ご理解とご協力をお願いいたします。", "メンテナンスのお知らせ" },
                    { new Guid("f2a3b4c5-6d7e-4f8a-9b0c-1d2e3f4a5b6c"), new Guid("450a2931-f066-4f64-920a-25d2804c4951"), "jp", "", "中途採用にてシステムエンジニア職の募集を開始しました。経験者の方はぜひエントリーフォームよりご応募ください。", "中途採用募集について" },
                    { new Guid("f6a7b8c9-0d1e-4f2a-3b4c-5d6e7f8a9b0c"), new Guid("da1cc949-385e-44b3-a7d1-3f489cf2347d"), "jp", "", "年末年始休業期間は12月29日から1月3日までとなります。休業中のお問い合わせは1月4日以降に順次対応いたします。", "年末年始の営業について" },
                    { new Guid("f8a9b0c1-2d3e-4f4a-5b6c-7d8e9f0a1b2c"), new Guid("01182620-90c4-498e-b0a5-1a8b740ff421"), "jp", "", "スマートフォンアプリでのプッシュ通知が受信できない障害が発生しております。対応中ですので、復旧まで今しばらくお待ちください。", "障害情報（スマホアプリ）" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsContent_AnnouncementId",
                table: "AnnouncementsContent",
                column: "AnnouncementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementsContent");

            migrationBuilder.DropTable(
                name: "Announcements");
        }
    }
}
