import { test, expect } from '../fixture/itemManagementTestFixture'

test('カタログアイテムを追加する', async ({
  loginPage,
  itemsPage,
  homePage,
  itemsAddPage,
  page,
}) => {
  await loginPage.navigate()
  await loginPage.login('test@example.com', 'password')
  await expect(page).toHaveTitle(/Dressca 管理/)

  await homePage.goToCatalogItems()
  await expect(page).toHaveURL(/\/catalog\/items/)

  await itemsPage.clickAddItemButton()
  await expect(page).toHaveURL(/\/catalog\/items\/add/)

  const testItemName = 'テスト用アイテム' + crypto.randomUUID()
  await itemsAddPage.enterItemName(testItemName)
  await itemsAddPage.clickAddButton()
  await expect(itemsAddPage.modalTitle).toBeVisible()

  await itemsAddPage.clickModalCloseButton()
  await expect(page).toHaveURL(/\/catalog\/items/)
  await expect(itemsPage.getItemRowsByItemName(testItemName)).toHaveCount(1)
})
