import { BasePage } from '../base/BasePage'

export class DonePage extends BasePage {
  async navigate(orderId: string) {
    await this.goto(`/ordering/done/${orderId}`)
  }
}
