import { BasePage } from '../base/BasePage'

export class CheckoutPage extends BasePage {
  async navigate() {
    await this.goto('/ordering/checkout')
  }
}
