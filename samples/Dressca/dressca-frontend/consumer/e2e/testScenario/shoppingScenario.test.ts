import { test, expect } from '@playwright/test'
import { LoginPage } from '../pages/authentication/LoginPage'
import { CatalogPage } from '../pages/catalog/CatalogPage'
import { BasketPage } from '../pages/basket/BasketPage'
import { CheckoutPage } from '../pages/ordering/CheckoutPage'

// test('ログインページにアクセスする', async ({ page }) => {
//   const loginPage = new LoginPage(page)
//   await loginPage.navigate()
//   await expect(page).toHaveURL(/.*authentication\/login/)
// })

// test('ログイン成功', async ({ page }) => {
//   const loginPage = new LoginPage(page)
//   await loginPage.navigate()
//   await loginPage.login('test@example.com', 'password')
//   await expect(page).toHaveTitle(/Dressca/)
// })

// test('ホーム画面にアクセスする', async ({ page }) => {
//   await page.goto('http://localhost:5173/')
//   await expect(page).toHaveTitle(/Dressca/)
// })

test('1つのアイテムの購入を完了できる', async ({ page }) => {
  const catalogPage = new CatalogPage(page)
  await catalogPage.navigate()
  await expect(page).toHaveTitle(/Dressca/)

  await catalogPage.putItemInBasket()
  await expect(page).toHaveURL(/\/basket/)

  const basketPage = new BasketPage(page)
  await basketPage.clickSubmitButton()
  await expect(page).toHaveURL(/\/authentication\/login/)

  const loginPage = new LoginPage(page)
  await loginPage.login('test@example.com', 'password')
  await expect(page).toHaveURL(/\/ordering\/checkout/)

  const checkoutPage = new CheckoutPage(page)
  await checkoutPage.clickSubmitButton()
  await expect(page).toHaveURL(/\/ordering\/done/)
})
