<#
.SYNOPSIS
  Dressca バックエンドの全コンテキストのマイグレーションを、依存関係の順にデータベースへ適用します。

.DESCRIPTION
  各コンテキストのプロジェクトをビルドしてから dotnet ef database update を実行します。
  接続文字列は、指定した appsettings ファイルの ConnectionStrings セクションから
  "<コンテキスト名>DbContext" の名前で取得します。
  実行には dotnet-ef ツールが必要です。

.PARAMETER AppSettingsPath
  接続文字列を定義した appsettings ファイルのパス。
  省略した場合は、結合テスト専用のローカルデータベースを定義したファイルを使用します。

.PARAMETER Configuration
  ビルド構成。

.EXAMPLE
  ./scripts/Update-Database.ps1

.EXAMPLE
  ./scripts/Update-Database.ps1 -AppSettingsPath ./src/Dressca.Web.Consumer/appsettings.Development.json
#>
[CmdletBinding()]
param(
    [string] $AppSettingsPath = (Join-Path $PSScriptRoot '../tests/Dressca.IntegrationTest/appsettings.LocalIntegrationTest.json'),
    [string] $Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'

# 買い物コンテキストの DisplayItems テーブルがカタログ管理コンテキストの CatalogItems テーブルを参照するため、
# アセット管理、カタログ管理、買い物の順に適用します。
$contexts = @('AssetsManagement', 'CatalogManagement', 'Shopping')

$settings = Get-Content -Raw -Path $AppSettingsPath | ConvertFrom-Json

foreach ($context in $contexts) {
    $dbContextName = "${context}DbContext"
    $connectionString = $settings.ConnectionStrings.$dbContextName
    if ([string]::IsNullOrWhiteSpace($connectionString)) {
        throw "$AppSettingsPath に接続文字列 $dbContextName が定義されていません。"
    }

    $project = Join-Path $PSScriptRoot "../src/Dressca.$context"
    Write-Host "$dbContextName のマイグレーションを適用します。"
    dotnet ef database update --project $project --context $dbContextName --configuration $Configuration --connection $connectionString
    if ($LASTEXITCODE -ne 0) {
        throw "$dbContextName のマイグレーションの適用に失敗しました。"
    }
}
