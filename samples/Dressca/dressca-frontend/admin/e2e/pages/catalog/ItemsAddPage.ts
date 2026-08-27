import { BasePage } from '../base/BasePage'

export class ItemsAddPage extends BasePage {
  readonly itemNameInput = this.page.locator('#item-name')
  readonly addButton = this.page.getByRole('button', { name: '追加' })
  readonly modalTitle = this.page.getByText('追加成功')
  readonly modalCloseButton = this.page.getByRole('button', { name: 'はい' })

  async navigate() {
    await this.goto('/catalog/items/add')
  }

  async enterItemName(itemName: string) {
    await this.itemNameInput.fill(itemName)
  }

  async clickAddButton() {
    await this.addButton.click()
  }

  async clickModalCloseButton() {
    await this.modalCloseButton.click()
  }
}
