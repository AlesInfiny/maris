import { BasePage } from '../base/BasePage'

export class CatalogPage extends BasePage {
  async navigate() {
    await this.goto('/')
  }
}
