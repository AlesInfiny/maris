import { BasePage } from '../base/BasePage'

export class ItemsEditPage extends BasePage {
  async navigate(itemId: string) {
    await this.goto(`/catalog/items/edit/${itemId}`)
  }
}
