import { describe, it, expect, vi } from 'vitest'
import { render } from 'vitest-browser-vue'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { createTestingPinia, type TestingPinia } from '@pinia/testing'
import { http, HttpResponse } from 'msw'
import { HttpStatusCode } from 'axios'
import ItemsAddPage from '@/pages/catalog/items/add/ItemsAddPage.vue'
import { useNotificationStore } from '@/business-common/stores/notification'
import { worker } from '@/../mock/browser'

const adminRole = 'ROLE_ADMIN'

/**
 * テスト用の Pinia ストアを生成します。
 * ユーザーのロール情報を初期状態として設定し、結合テスト用の環境を構築します。
 * @param userRoles - 認証状態に設定するユーザーロールの配列
 * @returns 初期化済みの TestingPinia インスタンス
 */
function CreateLoginState(userRoles: string[]) {
  return createTestingPinia({
    initialState: {
      authentication: {
        userRoles,
      },
    },
    createSpy: vi.fn, // 明示的に設定する必要があります。
    stubActions: false, // 結合テストなので、アクションはモック化しないように設定します。
  })
}

/**
 * 指定された Pinia ストアを利用して、アイテム追加画面をテスト用に描画します。
 * グローバルプラグインとして `pinia` と `router` を注入します。
 * @param pinia - テストに使用する TestingPinia インスタンス
 * @returns 描画結果
 */
function renderItemsAddPage(pinia: TestingPinia) {
  return render(ItemsAddPage, {
    global: { plugins: [pinia, createStubRouter()] },
  })
}

describe('管理者ロール_アイテムを追加できる', () => {
  it('追加画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsAddPage(CreateLoginState([adminRole]))
    // Assert
    await expect.element(screen.getByText('カタログアイテム追加')).toBeVisible()
  })

  it('追加ボタンを押下_追加成功_通知モーダルが開く', async () => {
    // Arrange
    const screen = await renderItemsAddPage(CreateLoginState([adminRole]))
    // Act
    await screen.getByRole('button', { name: '追加' }).click()
    // Assert
    await expect
      .element(screen.getByRole('dialog').getByText('カタログアイテムを追加しました。'))
      .toBeVisible()
  })
})

describe('ゲストロール_アイテム追加ボタンが非活性', () => {
  it('追加画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsAddPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByText('カタログアイテム追加')).toBeVisible()
  })

  it('追加ボタンが非活性', async () => {
    // Arrange
    // Act
    const screen = await renderItemsAddPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByRole('button', { name: '追加' })).toBeDisabled()
  })
})

describe('結果の通知', () => {
  it('カテゴリとブランドを取得できない_トーストを表示し入力フォームを表示しない', async () => {
    // Arrange
    worker.use(
      http.get(
        '/api/catalog-categories',
        () => new HttpResponse(null, { status: HttpStatusCode.InternalServerError }),
      ),
    )
    const loginState = CreateLoginState([adminRole])
    // Act
    const screen = await renderItemsAddPage(loginState)
    // Assert
    await expect
      .poll(() => useNotificationStore(loginState).message)
      .toBe('カテゴリとブランド情報の取得に失敗しました。')
    await expect.element(screen.getByLabelText('アイテム名')).not.toBeInTheDocument()
    await expect.element(screen.getByRole('button', { name: '追加' })).not.toBeInTheDocument()
  })

  it('追加に失敗した_トーストを表示する', async () => {
    // Arrange
    const loginState = CreateLoginState([adminRole])
    const screen = await renderItemsAddPage(loginState)
    await expect.element(screen.getByRole('button', { name: '追加' })).toBeEnabled()
    worker.use(
      http.post(
        '/api/catalog-items',
        () => new HttpResponse(null, { status: HttpStatusCode.InternalServerError }),
      ),
    )
    // Act
    await screen.getByRole('button', { name: '追加' }).click()
    // Assert
    await expect
      .poll(() => useNotificationStore(loginState).message)
      .toBe('カタログアイテムの追加に失敗しました。')
    await expect.element(screen.getByRole('dialog')).not.toBeInTheDocument()
  })
})
