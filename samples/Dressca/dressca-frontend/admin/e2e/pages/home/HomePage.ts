import { BasePage } from '../base/BasePage'

export class HomePage extends BasePage {
  async navigate() {
    await this.goto('/')
  }
}
