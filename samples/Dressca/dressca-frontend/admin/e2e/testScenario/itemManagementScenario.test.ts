import { test, expect } from '../fixture/itemManagementTestFixture'

test('カタログアイテムを追加する', async ({
  loginPage,
  itemsPage,
  homePage,
  itemsAddPage,
  page,
}) => {
  const testItemName = 'テスト用アイテム' + crypto.randomUUID()

  await test.step('ログインしてホーム画面に遷移する', async () => {
    await loginPage.navigate()
    await loginPage.login('test@example.com', 'password')
    await expect(homePage.pageTitle).toBeVisible()
    await expect(homePage.menuTable).toBeVisible()
  })

  await test.step('カタログアイテム一覧画面に遷移する', async () => {
    await homePage.goToCatalogItems()
    await expect(page).toHaveURL(/\/catalog\/items/)
  })

  await test.step('カタログアイテムを追加する', async () => {
    await itemsPage.clickAddItemButton()
    await expect(page).toHaveURL(/\/catalog\/items\/add/)

    await itemsAddPage.enterItemName(testItemName)
    await itemsAddPage.clickAddButton()
    await expect(itemsAddPage.modalTitle).toBeVisible()
  })

  await test.step('追加したカタログアイテムが一覧画面に表示されることを確認する', async () => {
    await itemsAddPage.clickModalCloseButton()
    await expect(page).toHaveURL(/\/catalog\/items/)
    await expect(itemsPage.getItemRowsByItemName(testItemName)).toHaveCount(1)
  })
})
