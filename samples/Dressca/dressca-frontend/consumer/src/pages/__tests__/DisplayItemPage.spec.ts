import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render } from 'vitest-browser-vue'
import { createTestingPinia, type TestingPinia } from '@pinia/testing'
import type { Router } from 'vue-router'
import DisplayItemPage from '@/pages/DisplayItemPage.vue'
import { routeNames } from '@/core/router/route-names'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { i18n } from '@/system-common/locales/i18n'
import { ServerError } from '@/system-common/error-handler/custom-error'
import { useNotificationStore } from '@/business-common/stores/notification'
import { basket } from '@/../mock/data/basket-items'
import { pagedListDisplayItem } from '@/../mock/data/display-items'
import { displayItemBrands } from '@/../mock/data/display-item-brands'
import { displayItemCategories } from '@/../mock/data/display-item-categories'

const {
  getDisplayItemCategoriesMock,
  getDisplayItemBrandsMock,
  getByQueryMock,
  postBasketItemMock,
  getBasketItemsMock,
} = vi.hoisted(() => ({
  getDisplayItemCategoriesMock: vi.fn(),
  getDisplayItemBrandsMock: vi.fn(),
  getByQueryMock: vi.fn(),
  postBasketItemMock: vi.fn(),
  getBasketItemsMock: vi.fn(),
}))

vi.mock('@/system-common/api-client', async (importOriginal) => ({
  ...(await importOriginal<typeof import('@/system-common/api-client')>()),
  displayItemCategoriesApi: () => ({
    getDisplayItemCategories: getDisplayItemCategoriesMock,
  }),
  displayItemBrandsApi: () => ({
    getDisplayItemBrands: getDisplayItemBrandsMock,
  }),
  displayItemsApi: () => ({
    getByQuery: getByQueryMock,
  }),
  basketItemsApi: () => ({
    postBasketItem: postBasketItemMock,
    getBasketItems: getBasketItemsMock,
  }),
}))

const { t } = i18n.global

let router: Router

beforeEach(() => {
  vi.resetAllMocks()
  getDisplayItemCategoriesMock.mockImplementation(() =>
    Promise.resolve({
      data: structuredClone(displayItemCategories),
    }),
  )
  getDisplayItemBrandsMock.mockImplementation(() =>
    Promise.resolve({
      data: structuredClone(displayItemBrands),
    }),
  )
  getByQueryMock.mockImplementation(() =>
    Promise.resolve({ data: structuredClone(pagedListDisplayItem) }),
  )
  postBasketItemMock.mockResolvedValue({})
  getBasketItemsMock.mockImplementation(() => Promise.resolve({ data: structuredClone(basket) }))
})

/**
 * テスト用の Pinia ストアを生成します。
 * 結合テストのため、アクションはモック化しません。
 * @returns 初期化済みの TestingPinia インスタンス
 */
function createPinia(): TestingPinia {
  return createTestingPinia({ createSpy: vi.fn, stubActions: false })
}

/**
 * 陳列品画面をテスト用に描画するヘルパー関数です。
 * @param pinia - テストに使用する TestingPinia インスタンス
 * @returns 描画結果。
 */
async function renderDisplayItemPage(pinia: TestingPinia) {
  router = createStubRouter()
  await router.push({ name: routeNames.displayItem })
  return render(DisplayItemPage, { global: { plugins: [pinia, router] } })
}

describe('結果の通知と遷移', () => {
  it('買い物かごに入れられる_入れた陳列品を履歴エントリーの state で渡して買い物かご画面へ遷移する', async () => {
    // Arrange
    const screen = await renderDisplayItemPage(createPinia())
    // Act
    await screen.getByRole('button', { name: '買い物かごに入れる' }).first().click()
    // Assert
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.basket)
    expect(router.options.history.state.addedItemId).toBe(pagedListDisplayItem.items![0]!.id)
  })

  it('買い物かごに入れられない_トーストを表示し陳列品画面にとどまる', async () => {
    // Arrange
    const pinia = createPinia()
    const screen = await renderDisplayItemPage(pinia)
    postBasketItemMock.mockRejectedValue(new ServerError(''))
    // Act
    await screen.getByRole('button', { name: '買い物かごに入れる' }).first().click()
    // Assert
    await expect.poll(() => useNotificationStore(pinia).message).toBe(t('failedToAddItemToCarts'))
    expect(router.currentRoute.value.name).toBe(routeNames.displayItem)
  })

  it('陳列品を取得できない_トーストを表示する', async () => {
    // Arrange
    getByQueryMock.mockRejectedValue(new ServerError(''))
    const pinia = createPinia()
    // Act
    await renderDisplayItemPage(pinia)
    // Assert
    await expect.poll(() => useNotificationStore(pinia).message).toBe(t('failedToGetItems'))
  })
})

describe('絞り込み', () => {
  it('カテゴリを選んだあと「すべて」に戻す_条件を指定せずに読み込み直す', async () => {
    // Arrange
    const screen = await renderDisplayItemPage(createPinia())
    const categorySelect = screen.getByLabelText('カテゴリ')
    await categorySelect.selectOptions(displayItemCategories[0]!.name)
    await expect
      .poll(() => getByQueryMock)
      .toHaveBeenLastCalledWith(undefined, displayItemCategories[0]!.id, undefined, undefined)
    // Act
    await categorySelect.selectOptions('すべて')
    // Assert
    await expect
      .poll(() => getByQueryMock)
      .toHaveBeenLastCalledWith(undefined, undefined, undefined, undefined)
  })
})
