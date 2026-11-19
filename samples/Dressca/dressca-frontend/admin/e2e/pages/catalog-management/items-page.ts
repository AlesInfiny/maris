import type { Locator, Page } from '@playwright/test'
import { BasePage } from '../base/base-page'

/**
 * カタログアイテム一覧画面のページオブジェクトです。
 */
export class ItemsPage extends BasePage {
  /** カタログアイテム一覧画面の見出しです。 */
  private readonly heading: Locator

  /** アイテム追加画面へ遷移するボタンです。 */
  private readonly addItemButton: Locator

  /** アイテム編集画面へ遷移するボタンです。画面にはカタログアイテムの数だけ存在します。 */
  private readonly editItemButtons: Locator

  /**
   * {@link ItemsPage} クラスの新しいインスタンスを初期化します。
   * @param page 操作対象のページ。
   */
  constructor(page: Page) {
    super(page)
    this.heading = page.getByText('カタログアイテム一覧')
    this.addItemButton = page.getByRole('button', { name: 'アイテム追加' })
    this.editItemButtons = page.getByRole('button', { name: '編集' })
  }

  /**
   * カタログアイテム一覧画面を表示します。
   */
  async open(): Promise<void> {
    await this.navigateTo('/catalog/items')
  }

  /**
   * カタログアイテム一覧画面の見出しを表す Locator を返します。
   * @returns カタログアイテム一覧画面の見出し。
   */
  getHeading(): Locator {
    return this.heading
  }

  /**
   * カタログアイテム追加画面へ遷移します。
   */
  async goToAddItem(): Promise<void> {
    await this.addItemButton.click()
  }

  /**
   * カタログアイテム一覧の最初のアイテムの編集画面へ遷移します。
   * @returns 最初のアイテムの編集ボタン。
   */
  getFirstEditItemButton(): Locator {
    return this.editItemButtons.first()
  }
}
