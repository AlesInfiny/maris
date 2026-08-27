import { BasePage } from '../base/BasePage'

export class CheckoutPage extends BasePage {
  readonly submitButton = this.page.getByRole('button', { name: '注文を確定する' })
  async navigate() {
    await this.goto('/ordering/checkout')
  }

  async clickSubmitButton() {
    await this.submitButton.click()
  }
}
