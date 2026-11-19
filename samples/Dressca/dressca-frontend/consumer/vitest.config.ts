import { fileURLToPath } from 'node:url'
import { mergeConfig, defineConfig, configDefaults } from 'vitest/config'
import { playwright } from '@vitest/browser-playwright'
import viteConfig from './vite.config.ts'

/**
 * 画面を描画するテストスイートです。
 * 実ブラウザーで描画結果を検証するため、Vitest ブラウザーモードで実行します。
 */
const browserTestFiles = [
  'src/pages/**/__tests__/**/*.spec.ts',
  'src/**/components/__tests__/**/*.spec.ts',
]

export default defineConfig((configEnv) =>
  mergeConfig(
    viteConfig(configEnv),
    defineConfig({
      test: {
        exclude: [...configDefaults.exclude, 'e2e/*'],
        root: fileURLToPath(new URL('./', import.meta.url)),
        setupFiles: ['./vitest.setup.ts'],
        coverage: {
          provider: 'v8',
          reporter: ['json-summary', 'html'],
          include: ['src/**/*.{ts,vue}'],
          exclude: ['src/system-common/generated/**'],
        },
        projects: [
          {
            extends: true,
            test: {
              name: 'unit',
              environment: 'jsdom',
              exclude: [...configDefaults.exclude, 'e2e/*', ...browserTestFiles],
            },
          },
          {
            extends: true,
            test: {
              name: 'browser',
              include: browserTestFiles,
              browser: {
                enabled: true,
                provider: playwright(),
                headless: true,
                instances: [{ browser: 'chromium' }],
              },
            },
          },
        ],
      },
    }),
  ),
)
