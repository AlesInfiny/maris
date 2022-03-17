using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    public partial class AddAssetsAndCatalogItemAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AssetType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemAssets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CatalogItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemAssets_CatalogItems",
                        column: x => x.CatalogItemId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "AssetType" },
                values: new object[,]
                {
                    { 1L, "b52dc7f712d94ca5812dd995bf926c04", "png" },
                    { 2L, "80bc8e167ccb4543b2f9d51913073492", "png" },
                    { 3L, "05d38fad5693422c8a27dd5b14070ec8", "png" },
                    { 4L, "45c22ba3da064391baac91341067ffe9", "png" },
                    { 5L, "4aed07c4ed5d45a5b97f11acedfbb601", "png" },
                    { 6L, "082b37439ecc44919626ba00fc60ee85", "png" },
                    { 7L, "f5f89954281747fa878129c29e1e0f83", "png" },
                    { 8L, "a8291ef2e8e14869a7048e272915f33c", "png" },
                    { 9L, "66237018c769478a90037bd877f5fba1", "png" },
                    { 10L, "d136d4c81b86478990984dcafbf08244", "png" },
                    { 11L, "47183f32f6584d7fb661f9216e11318b", "png" },
                    { 12L, "cf151206efd344e1b86854f4aa49fdef", "png" },
                    { 13L, "ab2e78eb7fe3408aadbf1e17a9945a8c", "png" },
                    { 14L, "0e557e96bc054f10bc91c27405a83e85", "png" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItemAssets",
                columns: new[] { "Id", "AssetCode", "CatalogItemId" },
                values: new object[,]
                {
                    { 1L, "45c22ba3da064391baac91341067ffe9", 1L },
                    { 2L, "4aed07c4ed5d45a5b97f11acedfbb601", 2L },
                    { 3L, "082b37439ecc44919626ba00fc60ee85", 3L },
                    { 4L, "f5f89954281747fa878129c29e1e0f83", 4L },
                    { 5L, "a8291ef2e8e14869a7048e272915f33c", 5L },
                    { 6L, "66237018c769478a90037bd877f5fba1", 6L },
                    { 7L, "d136d4c81b86478990984dcafbf08244", 7L },
                    { 8L, "47183f32f6584d7fb661f9216e11318b", 8L },
                    { 9L, "cf151206efd344e1b86854f4aa49fdef", 9L },
                    { 10L, "ab2e78eb7fe3408aadbf1e17a9945a8c", 10L },
                    { 11L, "0e557e96bc054f10bc91c27405a83e85", 11L }
                });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "クルーネック Tシャツ - ブラック");

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "暖かいのに着膨れしない起毛デニムです。", "裏起毛 スキニーデニム", 4800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "あたたかく肌ざわりも良いウール100%のロングコートです。", "ウールコート" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", "無地 ボタンダウンシャツ", 2800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "コンパクトサイズのバッグですが収納力は抜群です", "レザーハンドバッグ" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Description",
                value: "エイジング加工したレザーを使用しています。");

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", "トートバッグ ポーチ付き" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "さらりと気軽に纏える、キュートなミニサイズショルダー。", "ショルダーバッグ", 2800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "エレガントな雰囲気を放つキルティングデザインです。", "レザー チェーンショルダーバッグ" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "柔らかいソールは快適な履き心地で、ランニングに最適です。", "ランニングシューズ - ブルー" });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCode",
                table: "Assets",
                column: "AssetCode");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemAssets_CatalogItemId",
                table: "CatalogItemAssets",
                column: "CatalogItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "CatalogItemAssets");

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "クルーネック Tシャツ - グレー");

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "裏起毛で温かいパーカーです。", "無地 パーカー", 5800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "ウール生地のテーラードジャケットです。セットアップだけでなく単体でも使いやすい商品となっています。", "テーラードジャケット" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "ファー襟付きのデニムジャケットです。", "デニムジャケット", 19800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "シンプルなデザインのトートバッグです。", "トートバッグ" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Description",
                value: "軽くしなやかなレザーを使用しています。");

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "外側は高級感のある牛革、内側に丈夫で傷つきにくい豚革を採用した細身で持ち運びやすいビジネスバッグです。", "ビジネスバッグ" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "丁寧に編み込まれた馬革ハンドバッグです。落ち着いた色調で、オールシーズン使いやすくなっています。", "編み込みトートバッグ", 58800m });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "卓越した素材と緻密な縫製作業によって、デザインが完璧に表現されています。", "ハンドバッグ" });

            migrationBuilder.UpdateData(
                table: "CatalogItems",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "定番のハイカットスニーカーです。", "ハイカットスニーカー - ブラック" });
        }
    }
}
