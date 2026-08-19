import { test, expect } from '@playwright/test'
import { LoginPage } from '../pages/authentication/LoginPage'

test('has title', async ({ page }) => {
  await page.goto('https://playwright.dev/')

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Playwright/)
})

test('get started link', async ({ page }) => {
  await page.goto('https://playwright.dev/')

  // Click the get started link.
  await page.getByRole('link', { name: 'Get started' }).click()

  // Expects page to have a heading with the name of Installation.
  await expect(page.getByRole('heading', { name: 'Installation' })).toBeVisible()
})

test('access login page', async ({ page }) => {
  const loginPage = new LoginPage(page)
  await loginPage.navigate()
  await expect(page).toHaveURL(/.*authentication\/login/)
})

test('login', async ({ page }) => {
  const loginPage = new LoginPage(page)
  await loginPage.navigate()
  await loginPage.login('test@example.com', 'testpassword')
  await expect(page).toHaveTitle(/Dressca/)
})

test('access root page', async ({ page }) => {
  await page.goto('http://localhost:5173/')
  await expect(page).toHaveTitle(/Dressca/)
})
