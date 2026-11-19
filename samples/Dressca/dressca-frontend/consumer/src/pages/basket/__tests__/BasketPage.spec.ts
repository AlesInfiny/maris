import { describe, it, expect, vi } from 'vitest'
import { render } from 'vitest-browser-vue'
import type { Router } from 'vue-router'
import { routeNames, type BasketRouteState } from '@/core/router/route-names'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { i18n } from '@/system-common/locales/i18n'
import { createTestingPinia } from '@pinia/testing'
import BasketPage from '@/pages/basket/BasketPage.vue'
import type { GetBasketItemsResponse } from '@/system-common/generated/api-client'
import { ServerError } from '@/system-common/error-handler/custom-error'
import { useNotificationStore } from '@/business-common/stores/notification'
import { createAxiosError, createProblemDetails } from './helpers'

/**
 * 買い物かごに陳列品が入っている状態のモックレスポンスを生成します。
 * 主にテストやスタブデータとして利用することを想定しています。
 * @returns `GetBasketItemsResponse` 型のオブジェクト
 */
function createGetBasketItemsResponse(): GetBasketItemsResponse {
  return {
    buyerId: 'xxxxxxxxxxxxxxxxxxxxxxxxxx',
    account: {
      consumptionTaxRate: 0.1,
      consumptionTax: 248,
      deliveryCharge: 500,
      totalItemsPrice: 1980,
      totalPrice: 2728,
    },
    basketItems: [
      {
        displayItemId: '019b76da-a800-7004-8001-000000000001',
        quantity: 1,
        unitPrice: 1980,
        displayItem: {
          id: '019b76da-a800-7004-8001-000000000001',
          name: 'クルーネック Tシャツ - ブラック',
          productCode: 'C000000001',
          assetCodes: [],
        },
        subTotal: 1980,
      },
      {
        displayItemId: '019b76da-a800-7004-8001-000000000002',
        quantity: 2,
        unitPrice: 4800,
        displayItem: {
          id: '019b76da-a800-7004-8001-000000000002',
          name: '裏起毛 スキニーデニム',
          productCode: 'C000000002',
          assetCodes: ['4aed07c4ed5d45a5b97f11acedfbb601'],
        },
        subTotal: 9600,
      },
    ],
  }
}

/**
 * 空の買い物かご状態のモックレスポンスを生成します。
 * 主に「商品未追加時の画面」やテストケースに利用されます。
 * @returns `GetBasketItemsResponse` 型のオブジェクト
 */
function createEmptyGetBasketItemsResponse(): GetBasketItemsResponse {
  return {
    buyerId: 'xxxxxxxxxxxxxxxxxxxxxxxxxx',
    account: {
      consumptionTaxRate: 0.1,
      consumptionTax: 0,
      deliveryCharge: 500,
      totalItemsPrice: 0,
      totalPrice: 0,
    },
    basketItems: [],
  }
}

/**
 *  vi.mock はファイルの先頭に巻き上げられるので、
 * 複数回 vi.mock を宣言すると、最後に定義されたもので上書きされてしまいます。
 * モックの振る舞いをテストケースごとに変更したい場合は、
 *  vi.hoisted を使用します。
 */
const { getBasketItemsMock } = vi.hoisted(() => {
  return {
    getBasketItemsMock: vi.fn(),
  }
})

vi.mock('@/system-common/api-client', async (importOriginal) => ({
  ...(await importOriginal<typeof import('@/system-common/api-client')>()),
  // basketItemsApi() を呼ぶと { getBasketItems: getBasketItemsMock } が返る
  basketItemsApi: () => ({
    getBasketItems: getBasketItemsMock,
  }),
}))

let router: Router

/**
 * 買い物かご画面をテスト用に描画するヘルパー関数です。
 * @param stubRouter テストに使用するルーター。
 * @returns 描画結果。
 */
function renderBasketPage(stubRouter: Router = createStubRouter()) {
  router = stubRouter
  const pinia = createTestingPinia({
    createSpy: vi.fn, // 明示的に設定する必要があります。
    stubActions: false, // 結合テストなので、アクションはモック化しないように設定します。
  })
  i18n.global.locale.value = 'ja' // テストを実行するブラウザーの言語設定に依存しないよう、日本語に固定します。
  return render(BasketPage, {
    global: { plugins: [pinia, router, i18n] },
  })
}

describe('買い物かごのアイテムを表示する_アイテムが入っている', () => {
  it('取得したアイテムの情報が表示される', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByText('クルーネック Tシャツ - ブラック')).toBeVisible()
    await expect.element(screen.getByText('裏起毛 スキニーデニム')).toBeVisible()
  })

  it('「買い物を続ける」ボタンが表示されている', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: '買い物を続ける' })).toBeVisible()
  })

  it('「レジに進む」ボタンが表示されている', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: 'レジに進む' })).toBeVisible()
  })
})

describe('買い物かごのアイテムを表示する_アイテムが0件', () => {
  it('0件を示すメッセージが表示される', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createEmptyGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByText('買い物かごに商品がありません。')).toBeVisible()
  })

  it('「買い物を続ける」ボタンが表示されている', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createEmptyGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: '買い物を続ける' })).toBeVisible()
  })

  it('「レジに進む」ボタンが表示されていない', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createEmptyGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByText('買い物かごに商品がありません。')).toBeVisible()
    await expect.element(screen.getByRole('button', { name: 'レジに進む' })).not.toBeInTheDocument()
  })
})

describe('買い物かごのアイテムを表示する_サーバーエラー', () => {
  it('サーバーエラー_通知ストアにサーバーエラーを示すメッセージが格納される', async () => {
    // Arrange
    const expectDetail = 'expectDetail'
    const expectExceptionId = 'serverError'
    const expectStatus = 500
    const expectTitle = 'expectTitle'
    const problem = createProblemDetails({
      detail: expectDetail,
      exceptionId: expectExceptionId,
      status: expectStatus,
      title: expectTitle,
    })
    const error = createAxiosError(problem)
    getBasketItemsMock.mockRejectedValue(new ServerError('', error))
    const expectMessage = 'サーバーエラーが発生しました。'

    // Act
    await renderBasketPage()
    const notificationStore = useNotificationStore()

    // Assert
    await expect.poll(() => notificationStore.message).toBe(expectMessage)
    expect(notificationStore.id).toBe(expectExceptionId)
    expect(notificationStore.title).toBe(expectTitle)
    expect(notificationStore.detail).toBe(expectDetail)
    expect(notificationStore.status).toBe(expectStatus)
  })
})

describe('結果の通知と遷移', () => {
  it('購入できない陳列品が入っている_トーストを表示しレジに進むボタンが非活性', async () => {
    // Arrange
    const response = createGetBasketItemsResponse()
    response.deletedItemIds = [response.basketItems![0]!.displayItemId]
    getBasketItemsMock.mockResolvedValue({ data: response })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: 'レジに進む' })).toBeDisabled()
    expect(useNotificationStore().message).toBe(i18n.global.t('basketContainsUnavailableItem'))
  })

  it('レジに進む_注文確認画面へ遷移する', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createGetBasketItemsResponse() })
    const screen = await renderBasketPage()
    // Act
    await screen.getByRole('button', { name: 'レジに進む' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.checkout)
  })
})

describe('直前に買い物かごに入れた陳列品', () => {
  it('履歴エントリーの state に陳列品の ID がある_その陳列品を表示する', async () => {
    // Arrange
    const response = createGetBasketItemsResponse()
    getBasketItemsMock.mockResolvedValue({ data: response })
    const stubRouter = createStubRouter()
    const state: BasketRouteState = { addedItemId: response.basketItems![0]!.displayItemId }
    await stubRouter.push({ name: routeNames.basket, state })
    // Act
    const screen = await renderBasketPage(stubRouter)
    // Assert
    await expect.element(screen.getByText(i18n.global.t('addedItemsToBasket'))).toBeVisible()
  })

  it('履歴エントリーの state に陳列品の ID がない_表示しない', async () => {
    // Arrange
    getBasketItemsMock.mockResolvedValue({ data: createGetBasketItemsResponse() })
    // Act
    const screen = await renderBasketPage()
    // Assert
    await expect.element(screen.getByText('クルーネック Tシャツ - ブラック')).toBeVisible()
    await expect
      .element(screen.getByText(i18n.global.t('addedItemsToBasket')))
      .not.toBeInTheDocument()
  })
})
