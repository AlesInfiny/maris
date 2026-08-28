import { test, expect } from '../fixture/shoppingTestFixture'

test('1つのアイテムの購入を完了できる', async ({
  page,
  catalogPage,
  basketPage,
  checkoutPage,
  loginPage,
}) => {
  await catalogPage.navigate()
  await expect(page).toHaveTitle(/Dressca/)

  await catalogPage.putItemInBasket()
  await expect(page).toHaveURL(/\/basket/)

  await basketPage.clickSubmitButton()
  await expect(page).toHaveURL(/\/authentication\/login/)

  await loginPage.login('test@example.com', 'password')
  await expect(page).toHaveURL(/\/ordering\/checkout/)

  await checkoutPage.clickSubmitButton()
  await expect(page).toHaveURL(/\/ordering\/done/)
})
