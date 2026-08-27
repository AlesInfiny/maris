import { test, expect } from '@playwright/test'
import { LoginPage } from '../pages/authentication/LoginPage'
import { ItemsPage } from '../pages/catalog/ItemsPage'
import { HomePage } from '../pages/home/HomePage'
import { ItemsAddPage } from '../pages/catalog/ItemsAddPage'

test('カタログアイテムを追加する', async ({ page }) => {
  const loginPage = new LoginPage(page)
  await loginPage.navigate()
  await loginPage.login('test@example.com', 'password')
  await expect(page).toHaveTitle(/Dressca 管理/)

  const homePage = new HomePage(page)
  await homePage.goToCatalogItems()
  await expect(page).toHaveURL(/\/catalog\/items/)

  const itemsPage = new ItemsPage(page)
  await itemsPage.clickAddItemButton()
  await expect(page).toHaveURL(/\/catalog\/items\/add/)

  const itemsAddPage = new ItemsAddPage(page)
  const testItemName = 'テスト用アイテム' + crypto.randomUUID()
  await itemsAddPage.enterItemName(testItemName)
  await itemsAddPage.clickAddButton()

  await expect(itemsAddPage.modalTitle).toBeVisible()
  await itemsAddPage.clickModalCloseButton()

  await expect(page).toHaveURL(/\/catalog\/items/)
  const itemsPage2 = new ItemsPage(page)
  // 追加したテスト用アイテムが存在していることを確認
  await expect(itemsPage2.getItemRowsByItemName(testItemName)).toHaveCount(1)
})
