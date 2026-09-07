import { test, expect } from '../fixture/shoppingTestFixture'

test('1つの陳列品の購入を完了できる', async ({
  page,
  catalogPage,
  basketPage,
  checkoutPage,
  loginPage,
}) => {
  await test.step('買い物かごに陳列品を入れる', async () => {
    await catalogPage.navigate()
    await expect(page).toHaveTitle(/Dressca/)

    await catalogPage.putItemInBasket()
    await expect(page).toHaveURL(/\/basket/)
  })

  await test.step('ログインして注文内容確認画面へ遷移する', async () => {
    await basketPage.clickSubmitButton()
    await expect(page).toHaveURL(/\/authentication\/login/)

    await loginPage.login('test@example.com', 'password')
    await expect(page).toHaveURL(/\/ordering\/checkout/)
  })

  await test.step('注文を確定する', async () => {
    await checkoutPage.clickSubmitButton()
    await expect(page).toHaveURL(/\/ordering\/done/)
  })
})
