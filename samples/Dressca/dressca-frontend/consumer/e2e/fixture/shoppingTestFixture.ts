import { test as base } from '@playwright/test'
import { LoginPage } from '../pages/authentication/LoginPage'
import { CatalogPage } from '../pages/catalog/CatalogPage'
import { BasketPage } from '../pages/basket/BasketPage'
import { CheckoutPage } from '../pages/ordering/CheckoutPage'

type ShoppingTestFixture = {
  loginPage: LoginPage
  catalogPage: CatalogPage
  basketPage: BasketPage
  checkoutPage: CheckoutPage
}

export const test = base.extend<ShoppingTestFixture>({
  loginPage: async ({ page }, use) => {
    await use(new LoginPage(page))
  },
  catalogPage: async ({ page }, use) => {
    await use(new CatalogPage(page))
  },
  basketPage: async ({ page }, use) => {
    await use(new BasketPage(page))
  },
  checkoutPage: async ({ page }, use) => {
    await use(new CheckoutPage(page))
  },
})

export { expect } from '@playwright/test'
