import { describe, expect, it, vi } from 'vitest'
import { render } from 'vitest-browser-vue'
import type { Locator, LocatorSelectors } from 'vitest/browser'
import { routeNames } from '@/core/router/route-names'
import type { Router } from 'vue-router'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import LoginPage from '@/pages/authentication/login/LoginPage.vue'
import { createTestingPinia, type TestingPinia } from '@pinia/testing'
import { http, HttpResponse } from 'msw'
import { HttpStatusCode } from 'axios'
import { useNotificationStore } from '@/business-common/stores/notification'
import { worker } from '@/../mock/browser'

let router: Router

/**
 * ログイン画面をテスト用に描画します。
 * @param pinia テストに使用する TestingPinia インスタンス。
 * @param redirect ログイン後の戻り先のパス。
 * @returns 描画結果。
 */
async function renderLoginPage(
  pinia: TestingPinia = createTestingPinia({ createSpy: vi.fn }),
  redirect?: string,
) {
  router = createStubRouter()
  await router.push({ name: routeNames.login, query: redirect ? { redirect } : {} })
  return render(LoginPage, { global: { plugins: [pinia, router] } })
}

/**
 * 入力欄に値を入力します。
 * 入力欄は初めから空なので、空文字は一度入力してから消し、入力値の変更として検証させます。
 * @param input 入力欄。
 * @param value 入力する値。
 */
async function enter(input: Locator, value: string) {
  if (value === '') {
    await input.fill('x')
    await input.clear()
  } else {
    await input.fill(value)
  }
}

/**
 * ユーザー名とパスワードを入力します。
 * @param screen ログイン画面の描画結果。
 * @param userName ユーザー名。
 * @param password パスワード。
 */
async function fillForm(screen: LocatorSelectors, userName: string, password: string) {
  await enter(getUserNameInput(screen), userName)
  await enter(getPasswordInput(screen), password)
}

/**
 * ユーザー名の入力欄を取得します。
 * @param screen ログイン画面の描画結果。
 * @returns ユーザー名の入力欄。
 */
function getUserNameInput(screen: LocatorSelectors) {
  return screen.getByRole('textbox', { name: 'ユーザー名' })
}

/**
 * パスワードの入力欄を取得します。
 * パスワードの入力欄には ARIA ロールがないため、ラベルから取得します。
 * @param screen ログイン画面の描画結果。
 * @returns パスワードの入力欄。
 */
function getPasswordInput(screen: LocatorSelectors) {
  return screen.getByLabelText('パスワード')
}

describe('LoginPage', () => {
  it('ログイン画面を表示できる', async () => {
    // Arrange
    // Act
    const screen = await renderLoginPage()
    // Assert
    await expect.element(getUserNameInput(screen)).toBeVisible()
    await expect.element(getPasswordInput(screen)).toBeVisible()
    await expect.element(screen.getByRole('button', { name: 'ログイン' })).toBeVisible()
  })

  it('ユーザー名が空文字のときエラーメッセージが表示される', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, '', 'password123')
    // Assert
    await expect
      .element(getUserNameInput(screen))
      .toHaveAccessibleDescription('ユーザー名は必須です。')
    await expect.element(screen.getByText('ユーザー名は必須です。')).toBeVisible()
  })

  it('ユーザー名が空白のときエラーメッセージが表示される', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, ' ', 'password123')
    // Assert
    await expect
      .element(getUserNameInput(screen))
      .toHaveAccessibleDescription('ユーザー名は必須です。')
    await expect.element(screen.getByText('ユーザー名は必須です。')).toBeVisible()
  })

  it('ユーザー名の形式が正しくないときエラーメッセージが表示される', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, 'invalid-email', 'password123')
    // Assert
    await expect
      .element(getUserNameInput(screen))
      .toHaveAccessibleDescription('メールアドレスの形式で入力してください。')
    await expect.element(screen.getByText('メールアドレスの形式で入力してください。')).toBeVisible()
  })

  it('パスワードが空文字のときエラーメッセージが表示される', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, 'aaa@example.com', '')
    // Assert
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('パスワードは必須です。')
    await expect.element(screen.getByText('パスワードは必須です。')).toBeVisible()
  })

  it('パスワードが空白のときエラーメッセージが表示される', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, 'aaa@example.com', ' ')
    // Assert
    await expect
      .element(getPasswordInput(screen))
      .toHaveAccessibleDescription('パスワードは必須です。')
    await expect.element(screen.getByText('パスワードは必須です。')).toBeVisible()
  })

  it('エラーがある入力欄だけを不正な状態として示す', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, 'invalid-email', 'password123')
    // Assert
    await expect.element(getUserNameInput(screen)).toBeInvalid()
    await expect.element(getPasswordInput(screen)).toBeValid()
  })

  it('入力値が空のとき両方の入力欄を不正な状態として示す', async () => {
    // Arrange
    const screen = await renderLoginPage()
    // Act
    await fillForm(screen, '', '')
    // Assert
    await expect.element(getUserNameInput(screen)).toBeInvalid()
    await expect.element(getPasswordInput(screen)).toBeInvalid()
  })
})

describe('ログインの結果の通知と遷移', () => {
  it('ログインできる_戻り先の画面へ遷移する', async () => {
    // Arrange
    const pinia = createTestingPinia({ createSpy: vi.fn, stubActions: false })
    const screen = await renderLoginPage(pinia, '/catalog/items')
    await fillForm(screen, 'user@example.com', 'password')
    // Act
    await screen.getByRole('button', { name: 'ログイン' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.fullPath).toBe('/catalog/items')
  })

  it('ログインできない_トーストを表示しログイン画面にとどまる', async () => {
    // Arrange
    worker.use(
      http.get(
        '/api/users',
        () => new HttpResponse(null, { status: HttpStatusCode.InternalServerError }),
      ),
    )
    const pinia = createTestingPinia({ createSpy: vi.fn, stubActions: false })
    const screen = await renderLoginPage(pinia, '/catalog/items')
    await fillForm(screen, 'user@example.com', 'password')
    // Act
    await screen.getByRole('button', { name: 'ログイン' }).click()
    // Assert
    await expect.poll(() => useNotificationStore(pinia).message).toBe('ログインに失敗しました。')
    expect(router.currentRoute.value.name).toBe(routeNames.login)
  })
})
