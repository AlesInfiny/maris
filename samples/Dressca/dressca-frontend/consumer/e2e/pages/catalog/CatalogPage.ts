import { BasePage } from '../base/BasePage'

export class CatalogPage extends BasePage {
  readonly submitButton = this.page.getByRole('button', { name: '買い物かごに入れる' }).first()
  async navigate() {
    await this.goto('/')
  }

  async putItemInBasket() {
    await this.submitButton.click()
  }
}
