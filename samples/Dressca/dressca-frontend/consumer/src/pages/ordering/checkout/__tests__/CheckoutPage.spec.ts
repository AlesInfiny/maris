import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render } from 'vitest-browser-vue'
import { createTestingPinia, type TestingPinia } from '@pinia/testing'
import { useEventBus } from '@vueuse/core'
import type { GetBasketItemsResponse } from '@/system-common/generated/api-client'
import type { Router } from 'vue-router'
import CheckoutPage from '@/pages/ordering/checkout/CheckoutPage.vue'
import { routeNames } from '@/core/router/route-names'
import { createStubRouter } from '@/pages/__tests__/helpers/stub-router'
import { i18n } from '@/system-common/locales/i18n'
import { ServerError, UnauthorizedError } from '@/system-common/error-handler/custom-error'
import { unauthorizedErrorEventKey } from '@/system-common/events'
import { useNotificationStore } from '@/business-common/stores/notification'

const { getBasketItemsMock, postOrderMock } = vi.hoisted(() => ({
  getBasketItemsMock: vi.fn(),
  postOrderMock: vi.fn(),
}))

vi.mock('@/system-common/api-client', async (importOriginal) => ({
  ...(await importOriginal<typeof import('@/system-common/api-client')>()),
  basketItemsApi: () => ({
    getBasketItems: getBasketItemsMock,
  }),
  ordersApi: () => ({
    postOrder: postOrderMock,
  }),
}))

const { t } = i18n.global

let router: Router

beforeEach(() => {
  vi.resetAllMocks()
})

/**
 * 決まった買い物かごを返すように、買い物かごの取得をモックします。
 * @param itemCount 買い物かごに入っている陳列品の数。
 */
function useBasketResponse(itemCount: number) {
  const response: GetBasketItemsResponse = {
    buyerId: 'buyer-id',
    basketItems: Array.from({ length: itemCount }, (_, index) => ({
      displayItemId: `item-${index}`,
      quantity: 1,
      unitPrice: 1980,
      subTotal: 1980,
    })),
    deletedItemIds: [],
  }
  getBasketItemsMock.mockResolvedValue({ data: response })
}

/**
 * 注文確認画面をテスト用に描画するヘルパー関数です。
 * @returns 描画結果と TestingPinia インスタンス
 */
async function renderCheckoutPage() {
  const pinia: TestingPinia = createTestingPinia({ createSpy: vi.fn, stubActions: false })
  router = createStubRouter()
  await router.push({ name: routeNames.checkout })
  const screen = await render(CheckoutPage, { global: { plugins: [pinia, router] } })
  return { screen, pinia }
}

describe('注文を確定するボタン', () => {
  it('買い物かごの読み込み中_押せない', async () => {
    // Arrange
    getBasketItemsMock.mockReturnValue(new Promise(() => {}))
    // Act
    const { screen } = await renderCheckoutPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: '注文を確定する' })).toBeDisabled()
  })

  it('買い物かごの読み込みが完了した_押せる', async () => {
    // Arrange
    useBasketResponse(1)
    // Act
    const { screen } = await renderCheckoutPage()
    // Assert
    await expect.element(screen.getByRole('button', { name: '注文を確定する' })).toBeEnabled()
  })
})

describe('結果の通知と遷移', () => {
  it('買い物かごが空_陳列品画面へ遷移する', async () => {
    // Arrange
    useBasketResponse(0)
    // Act
    await renderCheckoutPage()
    // Assert
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.displayItem)
  })

  it('注文を確定できる_注文完了画面へ遷移する', async () => {
    // Arrange
    useBasketResponse(1)
    postOrderMock.mockResolvedValue({
      headers: { location: 'http://localhost/api/orders/order-id' },
    })
    const { screen } = await renderCheckoutPage()
    // Act
    await screen.getByRole('button', { name: '注文を確定する' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.done)
    expect(router.currentRoute.value.params).toEqual({ orderId: 'order-id' })
  })

  it('注文を確定できない_トーストを表示しエラー画面へ遷移する', async () => {
    // Arrange
    useBasketResponse(1)
    postOrderMock.mockRejectedValue(new ServerError(''))
    const { screen, pinia } = await renderCheckoutPage()
    // Act
    await screen.getByRole('button', { name: '注文を確定する' }).click()
    // Assert
    await expect.poll(() => router.currentRoute.value.name).toBe(routeNames.error)
    expect(useNotificationStore(pinia).message).toBe(t('failedToOrderItems'))
  })

  it('認証が切れている_画面はトーストも遷移もしない', async () => {
    // Arrange
    useBasketResponse(1)
    postOrderMock.mockRejectedValue(new UnauthorizedError(''))
    const unauthorizedListener = vi.fn()
    const off = useEventBus(unauthorizedErrorEventKey).on(unauthorizedListener)
    const { screen, pinia } = await renderCheckoutPage()
    // Act
    await screen.getByRole('button', { name: '注文を確定する' }).click()
    // Assert
    // ログイン画面への遷移は、共通のエラー処理が発行する認証エラーのイベントを受けて App.vue が行う
    await expect.poll(() => unauthorizedListener).toHaveBeenCalledOnce()
    off()
    expect(useNotificationStore(pinia).message).not.toBe(t('failedToOrderItems'))
    expect(router.currentRoute.value.name).toBe(routeNames.checkout)
  })
})
