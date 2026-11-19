import { describe, it, expect, beforeEach } from 'vitest'
import { render } from 'vitest-browser-vue'
import { i18n } from '@/system-common/locales/i18n'
import type { BasketLine } from '../../models/basket-line'
import BasketItem from '../BasketItem.vue'

/**
 * 買い物かごの行を生成します。
 * @returns `BasketLine` 型のオブジェクト
 */
function createBasketLine(): BasketLine {
  return {
    displayItemId: '019b76da-a800-7004-8001-00000000000a',
    name: '',
    assetCodes: [],
    quantity: 2,
    subTotal: 100000,
    unitPrice: 50000,
    available: true,
  }
}

/**
 * 買い物かごの行をテスト用に描画するヘルパー関数です。
 * @param available 陳列品を購入できるかどうか。
 * @returns 描画結果。
 */
function renderBasketItem(available: boolean) {
  return render(BasketItem, {
    props: { item: createBasketLine(), available },
    global: { plugins: [i18n] },
  })
}

describe('BasketItem', () => {
  beforeEach(() => {
    i18n.global.locale.value = 'ja'
  })

  it('小計が日本円形式で表示できる', async () => {
    // Arrange
    const available = true

    // Act
    const screen = await renderBasketItem(available)

    // Assert
    await expect.element(screen.getByText('￥100,000')).toBeVisible()
  })

  it('単価が日本円形式で表示できる', async () => {
    // Arrange
    const available = true

    // Act
    const screen = await renderBasketItem(available)

    // Assert
    await expect.element(screen.getByText('￥50,000')).toBeVisible()
  })

  it('販売中止中のメッセージが表示できる', async () => {
    // Arrange
    const available = false

    // Act
    const screen = await renderBasketItem(available)

    // Assert
    await expect
      .element(
        screen.getByText('こちらの商品は現在販売しておりません。買い物かごから削除してください。'),
      )
      .toBeVisible()
  })

  it('数量が 0 のとき更新ボタンが非活性になる', async () => {
    // Arrange
    const screen = await renderBasketItem(true)
    const quantityInput = screen.getByRole('spinbutton')
    const updateButton = screen.getByRole('button', { name: '更新' })
    // 初期表示でも非活性なので、一度活性にしてから非活性に変わることを確かめます。
    await quantityInput.fill('3')
    await expect.element(updateButton).toBeEnabled()

    // Act
    await quantityInput.fill('0')

    // Assert
    await expect.element(updateButton).toBeDisabled()
  })

  it('数量が 1000 のとき更新ボタンが非活性になる', async () => {
    // Arrange
    const screen = await renderBasketItem(true)
    const quantityInput = screen.getByRole('spinbutton')
    const updateButton = screen.getByRole('button', { name: '更新' })
    // 初期表示でも非活性なので、一度活性にしてから非活性に変わることを確かめます。
    await quantityInput.fill('3')
    await expect.element(updateButton).toBeEnabled()

    // Act
    await quantityInput.fill('1000')

    // Assert
    await expect.element(updateButton).toBeDisabled()
  })

  it('数量が有効な値に変更されたとき更新ボタンが活性になる', async () => {
    // Arrange
    const screen = await renderBasketItem(true)

    // Act
    await screen.getByRole('spinbutton').fill('3')

    // Assert
    await expect.element(screen.getByRole('button', { name: '更新' })).toBeEnabled()
  })
})
