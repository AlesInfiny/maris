import { describe, it, expect, vi } from 'vitest'
import { render } from 'vitest-browser-vue'
import type { LocatorSelectors } from 'vitest/browser'
import { routeNames } from '@/core/router/route-names'
import { createTestingPinia, type TestingPinia } from '@pinia/testing'
import { http, HttpResponse } from 'msw'
import { HttpStatusCode } from 'axios'
import type { Router } from 'vue-router'
import ItemsEditPage from '@/pages/catalog/items/edit/ItemsEditPage.vue'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { useNotificationStore } from '@/business-common/stores/notification'
import { worker } from '@/../mock/browser'
import { catalogItems } from '@/../mock/data/catalog-items'

const itemUrl = '/api/catalog-items/:catalogItemId'

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

let router: Router

/**
 * 指定された Pinia ストアを利用して、アイテム編集画面をテスト用に描画します。
 * @param pinia - テストに使用する TestingPinia インスタンス
 * @returns 描画結果
 */
async function renderItemsEditPage(pinia: TestingPinia) {
  router = createStubRouter()
  await router.push({ name: routeNames.catalogItemsEdit, params: { itemId: catalogItems[0]!.id } })
  return render(ItemsEditPage, { global: { plugins: [pinia, router] } })
}

/**
 * 指定した文言を含むモーダルを取得します。
 * 画面には複数のモーダルがあるため、文言で区別します。
 * @param screen - 描画結果
 * @param text - モーダルに含まれる文言
 * @returns モーダルのロケーター
 */
function dialog(screen: LocatorSelectors, text: string) {
  return screen.getByRole('dialog').filter({ hasText: text })
}

const deleteConfirmBody = 'カタログアイテムを削除します。'
const deleteNoticeBody = 'カタログアイテムを削除しました。'
const updateConfirmBody = 'カタログアイテムを更新します。'
const updateNoticeBody = 'カタログアイテムを更新しました。'

/**
 * 削除ボタンを押下し、確認モーダルの「はい」を押下します。
 * @param screen - 描画結果
 */
async function confirmDelete(screen: LocatorSelectors) {
  await screen.getByRole('button', { name: '削除' }).click()
  await dialog(screen, deleteConfirmBody).getByRole('button', { name: 'はい' }).click()
}

/**
 * 更新ボタンを押下し、確認モーダルの「はい」を押下します。
 * @param screen - 描画結果
 */
async function confirmUpdate(screen: LocatorSelectors) {
  await screen.getByRole('button', { name: '更新' }).click()
  await dialog(screen, updateConfirmBody).getByRole('button', { name: 'はい' }).click()
}

describe('管理者ロール_アイテムが削除できる', () => {
  it('編集画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Assert
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
  })

  it('削除ボタンを押下_確認モーダルが開く', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Act
    await screen.getByRole('button', { name: '削除' }).click()
    // Assert
    await expect
      .element(dialog(screen, deleteConfirmBody).getByText(deleteConfirmBody))
      .toBeVisible()
  })

  it('OKボタンを押下_確認モーダルが閉じる', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Act
    await confirmDelete(screen)
    // Assert
    await expect.element(screen.getByText(deleteConfirmBody)).not.toBeInTheDocument()
  })

  it('削除成功_通知モーダルが開く', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Act
    await confirmDelete(screen)
    // Assert
    await expect.element(dialog(screen, deleteNoticeBody).getByText(deleteNoticeBody)).toBeVisible()
  })

  it('OKボタンを押下__通知モーダルが閉じる', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    await confirmDelete(screen)
    // Act
    await dialog(screen, deleteNoticeBody).getByRole('button', { name: 'はい' }).click()
    // Assert
    await expect.element(screen.getByText(deleteNoticeBody)).not.toBeInTheDocument()
  })
})

describe('ゲストロール_アイテム削除ボタンが非活性', () => {
  it('編集画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
  })

  it('削除ボタンが非活性', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByRole('button', { name: '削除' })).toBeDisabled()
  })
})

describe('管理者ロール_アイテムが更新できる', () => {
  it('編集画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Assert
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
  })

  it('更新ボタンを押下__確認モーダルが開く', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Act
    await screen.getByRole('button', { name: '更新' }).click()
    // Assert
    await expect
      .element(dialog(screen, updateConfirmBody).getByText(updateConfirmBody))
      .toBeVisible()
  })

  it('更新成功__通知モーダルが開く', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    // Act
    await confirmUpdate(screen)
    // Assert
    await expect.element(dialog(screen, updateNoticeBody).getByText(updateNoticeBody)).toBeVisible()
    await expect.element(screen.getByText(updateConfirmBody)).not.toBeInTheDocument()
  })

  it('OKボタンを押下__通知モーダルが閉じる', async () => {
    // Arrange
    const screen = await renderItemsEditPage(CreateLoginState([adminRole]))
    await confirmUpdate(screen)
    // Act
    await dialog(screen, updateNoticeBody).getByRole('button', { name: 'はい' }).click()
    // Assert
    await expect.element(screen.getByText(updateNoticeBody)).not.toBeInTheDocument()
  })
})

describe('ゲストロール_アイテム更新ボタンが非活性', () => {
  it('編集画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
  })

  it('更新ボタンが非活性', async () => {
    // Arrange
    // Act
    const screen = await renderItemsEditPage(CreateLoginState(['ROLE_GUEST']))
    // Assert
    await expect.element(screen.getByRole('button', { name: '更新' })).toBeDisabled()
  })
})

describe('結果の通知と遷移', () => {
  it('アイテムが存在しない_トーストを表示しアイテム一覧画面へ遷移する', async () => {
    // Arrange
    worker.use(http.get(itemUrl, () => new HttpResponse(null, { status: HttpStatusCode.NotFound })))
    const loginState = CreateLoginState([adminRole])
    // Act
    await renderItemsEditPage(loginState)
    // Assert
    await expect
      .poll(() => useNotificationStore(loginState).message)
      .toBe('対象のアイテムが見つかりませんでした。')
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.catalogItems)
  })

  it('更新が競合した_トーストを表示し編集画面にとどまる', async () => {
    // Arrange
    const loginState = CreateLoginState([adminRole])
    const screen = await renderItemsEditPage(loginState)
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
    worker.use(http.put(itemUrl, () => new HttpResponse(null, { status: HttpStatusCode.Conflict })))
    // Act
    await confirmUpdate(screen)
    // Assert
    await expect
      .poll(() => useNotificationStore(loginState).message)
      .toBe('カタログアイテムの更新が競合しました。もう一度更新してください。')
    expect(router.currentRoute.value.name).toBe(routeNames.catalogItemsEdit)
  })

  it('削除対象が存在しない_トーストを表示しアイテム一覧画面へ遷移する', async () => {
    // Arrange
    const loginState = CreateLoginState([adminRole])
    const screen = await renderItemsEditPage(loginState)
    await expect.element(screen.getByText('カタログアイテム編集')).toBeVisible()
    worker.use(
      http.delete(itemUrl, () => new HttpResponse(null, { status: HttpStatusCode.NotFound })),
    )
    // Act
    await confirmDelete(screen)
    // Assert
    await expect
      .poll(() => useNotificationStore(loginState).message)
      .toBe('削除対象のカタログアイテムが見つかりませんでした。')
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.catalogItems)
  })
})
