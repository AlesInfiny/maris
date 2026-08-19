import { BasePage } from '../base/BasePage'

export class ItemsAddPage extends BasePage {
  async navigate() {
    await this.goto('/catalog/items/add')
  }
}
