import { BasePage } from '../base/BasePage'

export class HomePage extends BasePage {
  readonly pageTitle = this.page.getByText('Dressca 管理 トップ', { exact: true })
  readonly catalogItemLink = this.page.getByRole('link', { name: 'カタログアイテム管理' }).last()
  readonly menuTable = this.page.getByRole('table')

  async navigate() {
    await this.goto('/')
  }

  async goToCatalogItems() {
    await this.catalogItemLink.click()
  }
}
