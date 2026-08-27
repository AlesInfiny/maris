import { BasePage } from '../base/BasePage'

export class BasketPage extends BasePage {
  readonly submitButton = this.page.getByRole('button', { name: 'レジに進む' })

  async navigate() {
    await this.goto('/basket')
  }

  async clickSubmitButton() {
    await this.submitButton.click()
  }
}
