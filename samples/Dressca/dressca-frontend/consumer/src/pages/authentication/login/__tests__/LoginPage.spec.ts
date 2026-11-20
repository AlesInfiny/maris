import { beforeEach, describe, expect, it, vi } from 'vitest'
import { render } from 'vitest-browser-vue'
import type { Locator } from 'vitest/browser'
import { i18n } from '@/system-common/locales/i18n'
import LoginPage from '@/pages/authentication/login/LoginPage.vue'
import type { Router } from 'vue-router'
import { routeNames } from '@/core/router/route-names'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { createTestingPinia } from '@pinia/testing'

let router: Router

beforeEach(() => {
  router = createStubRouter()
})

/**
 * ログイン画面を指定したロケールでテスト用に描画するヘルパー関数です。
 * @param locale 表示に使用するロケール。
 * @returns 描画結果。
 */
function renderLoginPage(locale: 'ja' | 'en') {
  const pinia = createTestingPinia({
    createSpy: vi.fn,
  })
  i18n.global.locale.value = locale
  return render(LoginPage, {
    global: { plugins: [pinia, router, i18n] },
  })
}

/**
 * 入力欄に値を入力します。
 * @param input 入力欄。
 * @param value 入力する値。
 */
async function fillInput(input: Locator, value: string) {
  if (value === '') {
    // 入力欄の初期値も空なので、一度入力してから消さないと値が変わらず検証されません。
    await input.fill('x')
  }
  await input.fill(value)
}

/**
 * メールアドレスとパスワードを入力します。
 * @param screen ログイン画面の描画結果。
 * @param email メールアドレス
 * @param password パスワード
 */
async function enterCredentials(
  screen: Awaited<ReturnType<typeof renderLoginPage>>,
  email: string,
  password: string,
) {
  await fillInput(getEmailInput(screen), email)
  await fillInput(getPasswordInput(screen), password)
}

/**
 * メールアドレスの入力欄を取得します。
 * @param screen ログイン画面の描画結果。
 * @returns メールアドレスの入力欄。
 */
function getEmailInput(screen: Awaited<ReturnType<typeof renderLoginPage>>) {
  return screen.getByRole('textbox', { name: 'メールアドレス' })
}

/**
 * パスワードの入力欄を取得します。
 * パスワードの入力欄には ARIA ロールがないため、ラベルから取得します。
 * @param screen ログイン画面の描画結果。
 * @returns パスワードの入力欄。
 */
function getPasswordInput(screen: Awaited<ReturnType<typeof renderLoginPage>>) {
  return screen.getByLabelText('パスワード')
}

describe('Authentication validation', () => {
  it('入力値が空の場合、日本語で必須メッセージを返す', async () => {
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, '', '')

    await expect.element(getEmailInput(screen)).toHaveAccessibleDescription('値を入力してください')
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('値を入力してください')
  })

  it('入力値が空白の場合、日本語で必須メッセージを返す', async () => {
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, ' ', ' ')

    await expect.element(getEmailInput(screen)).toHaveAccessibleDescription('値を入力してください')
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('値を入力してください')
  })

  it('日本語でメール形式メッセージを返す', async () => {
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, 'invalid-email', 'aaa')

    await expect
      .element(getEmailInput(screen))
      .toHaveAccessibleDescription('メールアドレスの形式で入力してください')
  })

  it('入力値が空の場合、英語で必須メッセージを返す', async () => {
    const screen = await renderLoginPage('en')
    await enterCredentials(screen, '', '')

    await expect
      .element(getEmailInput(screen))
      .toHaveAccessibleDescription('this field is required')
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('this field is required')
  })

  it('入力値が空白の場合、英語で必須メッセージを返す', async () => {
    const screen = await renderLoginPage('en')
    await enterCredentials(screen, ' ', ' ')

    await expect
      .element(getEmailInput(screen))
      .toHaveAccessibleDescription('this field is required')
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('this field is required')
  })

  it('英語でメール形式メッセージを返す', async () => {
    const screen = await renderLoginPage('en')
    await enterCredentials(screen, 'invalid-email', 'aaa')

    await expect.element(getEmailInput(screen)).toHaveAccessibleDescription('invalid email format')
  })

  it('エラーがある入力欄だけを不正な状態として示す', async () => {
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, 'invalid-email', 'aaa')

    await expect.element(getEmailInput(screen)).toBeInvalid()
    await expect.element(getPasswordInput(screen)).toBeValid()
  })
})

describe('ログイン後の遷移', () => {
  it('戻り先がある_戻り先の画面へ遷移する', async () => {
    // Arrange
    await router.push({ name: routeNames.login, query: { redirect: '/basket' } })
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, 'user@example.com', 'password')
    // Act
    await screen.getByRole('button', { name: 'ログイン' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.fullPath).toBe('/basket')
  })

  it('戻り先がアプリケーション外を指す_陳列品画面へ遷移する', async () => {
    // Arrange
    await router.push({ name: routeNames.login, query: { redirect: '//example.com' } })
    const screen = await renderLoginPage('ja')
    await enterCredentials(screen, 'user@example.com', 'password')
    // Act
    await screen.getByRole('button', { name: 'ログイン' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.fullPath).toBe('/')
  })
})
