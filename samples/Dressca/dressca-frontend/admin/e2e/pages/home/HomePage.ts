import { BasePage } from '../base/BasePage'

export class HomePage extends BasePage {
  readonly catalogItemLink = this.page.getByRole('link', { name: 'カタログアイテム管理' }).last()

  async navigate() {
    await this.goto('/')
  }

  async goToCatalogItems() {
    await this.catalogItemLink.click()
  }
}
