import { defineConfig, devices } from '@playwright/test'

/**
 * プロキシを設定している環境では、 localhost へアクセスがプロキシ経由となりローカル開発サーバーへの接続に失敗することがあります。
 * そのため Playwright のテスト実行時にプロキシを無効化するために、NO_PROXY 環境変数に localhost と 127.0.0.1 を追加します。
 */
process.env.NO_PROXY = process.env.NO_PROXY
  ? `${process.env.NO_PROXY},localhost,127.0.0.1`
  : 'localhost,127.0.0.1'

/**
 * 各設定値の詳細： https://playwright.dev/docs/test-configuration.
 */
export default defineConfig({
  testDir: './e2e',
  /* 個々のテスト単位でワーカーに割り当てる。 */
  fullyParallel: true,
  /* 誤って test.only をソースコードに残してしまった場合、CI 上でビルドを失敗させる。 */
  forbidOnly: !!process.env.CI,
  /* CI 環境のみリトライする。 */
  retries: process.env.CI ? 2 : 0,
  /* CI 環境では、テストを並列実行せず、1 ワーカーで実行する。*/
  workers: process.env.CI ? 1 : undefined,
  /* リポーターの種類。 https://playwright.dev/docs/test-reporters */
  reporter: 'html',
  /* 全ての project に適用される共通設定。 https://playwright.dev/docs/api/class-testoptions. */
  use: {
    /* `await page.goto('')` などのアクションで使用する ベース URL。 */
    baseURL: 'http://localhost:6173',
    locale: 'ja-JP',
    /* 失敗したテストをリトライする際にトレースを収集する。 https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry',
  },

  /* テスト実行時の構成。 */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  /* テスト実行前にローカル開発サーバーを起動する。 */
  webServer: [
    {
      command: 'npm run dev',
      url: 'http://localhost:6173',
      reuseExistingServer: !process.env.CI,
      timeout: 60 * 1000,
    },
    {
      command: 'dotnet run',
      cwd: '../../dressca-backend/src/Dressca.Web.Admin',
      url: 'https://localhost:6001',
      ignoreHTTPSErrors: true,
      reuseExistingServer: !process.env.CI,
      timeout: 120 * 1000,
    },
  ],
})
