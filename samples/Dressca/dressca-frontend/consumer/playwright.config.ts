import { defineConfig, devices } from '@playwright/test'

export default defineConfig({
  testDir: './playwright',
  timeout: 60_000,
  fullyParallel: false,
  retries: process.env.CI ? 1 : 0,
  reporter: [['list'], ['json', { outputFile: 'playwright-report/a11y-results.json' }]],
  use: {
    baseURL: 'http://127.0.0.1:5173',
    headless: true,
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'off',
  },
  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
      },
    },
  ],
  webServer: {
    command: 'npm run mock -- --host 127.0.0.1 --strictPort',
    url: 'http://127.0.0.1:5173/',
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
  },
})
