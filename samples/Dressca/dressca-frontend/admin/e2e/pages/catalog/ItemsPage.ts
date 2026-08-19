import { BasePage } from '../base/BasePage'

export class ItemsPage extends BasePage {
  async navigate() {
    await this.goto('/catalog/items')
  }
}
