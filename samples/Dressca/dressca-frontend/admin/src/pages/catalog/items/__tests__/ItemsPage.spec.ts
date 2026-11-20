import { describe, it, expect } from 'vitest'
import { render } from 'vitest-browser-vue'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { createPinia, setActivePinia } from 'pinia'
import { http, HttpResponse } from 'msw'
import { HttpStatusCode } from 'axios'
import ItemsPage from '@/pages/catalog/items/ItemsPage.vue'
import { useNotificationStore } from '@/business-common/stores/notification'
import { worker } from '@/../mock/browser'

/**
 * アイテム一覧画面をテスト用に描画します。
 * 新しい Pinia インスタンスを作成してアクティブ化し、
 * グローバルプラグインとして `pinia` と `router` を注入します。
 * @returns 描画結果。
 */
function renderItemsPage() {
  const pinia = createPinia()
  setActivePinia(pinia)
  return render(ItemsPage, {
    global: { plugins: [pinia, createStubRouter()] },
  })
}

describe('アイテム一覧が表示できる', () => {
  it('アイテム一覧画面に遷移できる', async () => {
    // Arrange
    // Act
    const screen = await renderItemsPage()
    // Assert
    await expect.element(screen.getByText('カタログアイテム一覧')).toBeVisible()
  })

  it('アイテムが取得した個数分表示される', async () => {
    // Arrange
    const expectedItemCount = 11
    // Act
    const screen = await renderItemsPage()
    // Assert
    await expect
      .poll(() => screen.getByRole('button', { name: '編集' }).elements())
      .toHaveLength(expectedItemCount)
  })
})

describe('結果の通知', () => {
  it('アイテムを取得できない_トーストを表示しアイテム追加ボタンは表示する', async () => {
    // Arrange
    worker.use(
      http.get(
        '/api/catalog-items',
        () => new HttpResponse(null, { status: HttpStatusCode.InternalServerError }),
      ),
    )
    // Act
    const screen = await renderItemsPage()
    // Assert
    await expect
      .poll(() => useNotificationStore().message)
      .toBe('カタログアイテムの取得に失敗しました。')
    await expect.element(screen.getByRole('button', { name: 'アイテム追加' })).toBeVisible()
    await expect.element(screen.getByRole('button', { name: '編集' })).not.toBeInTheDocument()
  })
})
