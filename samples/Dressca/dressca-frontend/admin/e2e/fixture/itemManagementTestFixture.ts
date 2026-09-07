import { test as base } from '@playwright/test'
import { LoginPage } from '../pages/authentication/LoginPage'
import { ItemsPage } from '../pages/catalog/ItemsPage'
import { HomePage } from '../pages/home/HomePage'
import { ItemsAddPage } from '../pages/catalog/ItemsAddPage'

type ItemManagementTestFixture = {
  loginPage: LoginPage
  itemsPage: ItemsPage
  homePage: HomePage
  itemsAddPage: ItemsAddPage
}

export const test = base.extend<ItemManagementTestFixture>({
  loginPage: async ({ page }, use) => {
    await use(new LoginPage(page))
  },
  itemsPage: async ({ page }, use) => {
    await use(new ItemsPage(page))
  },
  homePage: async ({ page }, use) => {
    await use(new HomePage(page))
  },
  itemsAddPage: async ({ page }, use) => {
    await use(new ItemsAddPage(page))
  },
})

export { expect } from '@playwright/test'
