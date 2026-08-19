import { BasePage } from '../base/BasePage'

export class BasketPage extends BasePage {
  async navigate() {
    await this.goto('/basket')
  }
}
