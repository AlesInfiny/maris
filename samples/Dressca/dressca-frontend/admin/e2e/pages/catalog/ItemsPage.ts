import { BasePage } from '../base/BasePage'
import { expect, type Locator } from '@playwright/test'

export class ItemsPage extends BasePage {
  readonly addItemButton = this.page.getByRole('button', { name: 'アイテム追加' })

  async navigate() {
    await this.goto('/catalog/items')
  }

  async clickAddItemButton() {
    await this.addItemButton.click()
  }

  getItemRowsByItemName(itemName: string): Locator {
    return this.page.getByRole('row').filter({ hasText: itemName })
  }
}
