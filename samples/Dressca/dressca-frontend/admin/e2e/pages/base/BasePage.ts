import type { Page } from '@playwright/test'

export abstract class BasePage {
  constructor(protected page: Page) {}
  async goto(path: string) {
    await this.page.goto(path)
  }
}
