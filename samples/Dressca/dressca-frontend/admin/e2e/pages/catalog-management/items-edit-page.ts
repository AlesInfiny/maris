import type { Locator, Page } from '@playwright/test'
import { BasePage } from '../base/base-page'

/**
 * カタログアイテム編集画面のページオブジェクトです。
 */
export class ItemsEditPage extends BasePage {
  /** カタログアイテム編集画面の見出しです。 */
  private readonly heading: Locator

  /** アイテム名の入力欄です。 */
  private readonly itemNameInput: Locator

  /** 説明の入力欄です。 */
  private readonly itemDescriptionInput: Locator

  /** 単価の入力欄です。 */
  private readonly priceInput: Locator

  /** 商品コードの入力欄です。 */
  private readonly productCodeInput: Locator

  /** 現在のアイテム名です。 */
  private readonly currentItemName: Locator

  /** 現在の説明です。 */
  private readonly currentDescription: Locator

  /** 現在の単価です。 */
  private readonly currentUnitPrice: Locator

  /** 現在の商品コードです。 */
  private readonly currentProductCode: Locator

  /** 更新ボタンです。 */
  private readonly updateButton: Locator

  /** 更新確認モーダルです。 */
  private readonly updateConfirmationModal: Locator

  /** 更新確認モーダルの確認ボタンです。 */
  private readonly updateConfirmationModalOkButton: Locator

  /** 更新成功を通知するモーダルです。 */
  private readonly updateNoticeModal: Locator

  /** 更新成功を通知するモーダルの確認ボタンです。 */
  private readonly updateNoticeCloseButton: Locator

  /**
   * {@link ItemsEditPage} クラスの新しいインスタンスを初期化します。
   * @param page 操作対象のページ。
   */
  constructor(page: Page) {
    super(page)
    this.heading = page.getByText('カタログアイテム編集')
    this.itemNameInput = page.locator('#item-name')
    this.itemDescriptionInput = page.locator('#item-description')
    this.priceInput = page.locator('#unit-price')
    this.productCodeInput = page.locator('#product-code')
    this.currentItemName = page.locator('#current-item-name')
    this.currentDescription = page.locator('#current-description')
    this.currentUnitPrice = page.locator('#current-unit-price')
    this.currentProductCode = page.locator('#current-product-code')
    this.updateButton = page.getByRole('button', { name: '更新', exact: true })
    this.updateConfirmationModal = page.getByRole('dialog', {
      name: 'カタログアイテムを更新しますか？',
    })
    this.updateConfirmationModalOkButton = this.updateConfirmationModal.getByRole('button', {
      name: 'はい',
    })
    this.updateNoticeModal = page.getByRole('dialog', { name: '更新成功' })
    this.updateNoticeCloseButton = this.updateNoticeModal.getByRole('button', { name: 'はい' })
  }

  /**
   * カタログアイテム編集画面を表示します。
   * @param itemId 編集するアイテムのID。
   */
  async open(itemId: string): Promise<void> {
    await this.navigateTo(`/catalog/items/edit/${itemId}`)
  }

  /**
   * カタログアイテム編集画面の見出しを表す Locator を返します。
   * @returns カタログアイテム編集画面の見出し。
   */
  getHeading(): Locator {
    return this.heading
  }

  /**
   * カタログアイテムの情報を編集します。
   * @param itemName アイテム名。
   * @param itemDescription 説明。
   * @param price 単価。
   * @param productCode 商品コード。
   */
  async editCatalogItem(
    itemName: string,
    itemDescription: string,
    price: string,
    productCode: string,
  ): Promise<void> {
    await this.itemNameInput.fill(itemName)
    await this.itemDescriptionInput.fill(itemDescription)
    await this.priceInput.fill(price)
    await this.productCodeInput.fill(productCode)
  }

  /**
   * 入力した内容でカタログアイテムを更新します。
   */
  async updateCatalogItem(): Promise<void> {
    await this.updateButton.click()
  }

  /**
   * 更新確認モーダルを表す Locator を返します。
   * @returns 更新確認モーダル。
   */
  getUpdateConfirmationModal(): Locator {
    return this.updateConfirmationModal
  }

  /**
   * 更新確認モーダルで「はい」をクリックして更新を確定します。
   */
  async confirmUpdate(): Promise<void> {
    await this.updateConfirmationModalOkButton.click()
  }

  /**
   * 更新成功を通知するモーダルを表す Locator を返します。
   * @returns 更新成功を通知するモーダル。
   */
  getUpdateNoticeModal(): Locator {
    return this.updateNoticeModal
  }

  /**
   * 更新成功を通知するモーダルを閉じます。
   */
  async closeUpdateNotice(): Promise<void> {
    await this.updateNoticeCloseButton.click()
  }

  /**
   * 現在のアイテム名を表す Locator を返します。
   * @returns 現在のアイテム名。
   */
  getCurrentItemName(): Locator {
    return this.currentItemName
  }
  /**
   * 現在の説明を表す Locator を返します。
   * @returns 現在の説明。
   */
  getCurrentDescription(): Locator {
    return this.currentDescription
  }

  /**
   * 現在の単価を表す Locator を返します。
   * @returns 現在の単価。
   */
  getCurrentUnitPrice(): Locator {
    return this.currentUnitPrice
  }
  /**
   * 現在の商品コードを表す Locator を返します。
   * @returns 現在の商品コード。
   */
  getCurrentProductCode(): Locator {
    return this.currentProductCode
  }
}
