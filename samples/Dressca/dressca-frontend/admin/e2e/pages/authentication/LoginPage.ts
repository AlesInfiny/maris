import { BasePage } from '../base/BasePage'

export class LoginPage extends BasePage {
  readonly usernameInput = this.page.locator('#userName')
  readonly passwordInput = this.page.locator('#password')
  readonly loginButton = this.page.getByRole('button')

  async navigate() {
    await this.goto('/authentication/login')
  }

  async login(user: string, pass: string) {
    await this.usernameInput.fill(user)
    await this.passwordInput.fill(pass)
    await this.loginButton.click()
  }
}
