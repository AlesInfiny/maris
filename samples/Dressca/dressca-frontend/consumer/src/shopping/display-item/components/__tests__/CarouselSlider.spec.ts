import { describe, it, expect } from 'vitest'
import { render } from 'vitest-browser-vue'
import { page, userEvent } from 'vitest/browser'
import CarouselSlider from '../CarouselSlider.vue'

const items = [
  { name: 'test1', id: 1 },
  { name: 'test2', id: 2 },
  { name: 'test3', id: 3 },
]

// 表示要素の中でドラッグする距離を確保するため、表示要素に幅を持たせます。
const itemSlot =
  '<template #default="{ item }"><div style="width: 300px">{{ item.name }}</div></template>'

/**
 * カルーセルをテスト用に描画するヘルパー関数です。
 * @param slotItems カルーセルに渡す要素の配列。
 * @returns 描画結果。
 */
function renderCarousel(slotItems: typeof items = items) {
  return render(CarouselSlider, {
    props: { items: slotItems },
    slots: { default: itemSlot },
  })
}

describe('CarouselSlider', () => {
  it('slotにコンテンツが挿入できる', async () => {
    const screen = await render(CarouselSlider, {
      props: { items },
      slots: {
        default: '<div>slot content</div>',
      },
    })

    await expect.element(screen.getByText('slot content')).toBeVisible()
  })

  it('引数の配列が表示できる', async () => {
    const screen = await renderCarousel()

    await expect.element(screen.getByText('test1')).toBeVisible()
    await expect.element(screen.getByText('test2')).not.toBeInTheDocument()
    await expect.element(screen.getByText('test3')).not.toBeInTheDocument()
  })

  it('引数の要素が0の時なにも表示しない', async () => {
    const screen = await renderCarousel([])

    await expect.element(page.elementLocator(screen.container)).toBeEmptyDOMElement()
  })

  it('右矢印をクリックすると表示要素が進む', async () => {
    const screen = await renderCarousel()

    await screen.getByRole('button', { name: '次のスライド' }).click()

    await expect.element(screen.getByText('test2')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('左矢印をクリックすると表示要素が戻る', async () => {
    const screen = await renderCarousel()

    await screen.getByRole('button', { name: '前のスライド' }).click()

    await expect.element(screen.getByText('test3')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('表示要素を左にスライドすると要素が進む', async () => {
    const screen = await renderCarousel()
    const slide = screen.getByText('test1')

    await userEvent.dragAndDrop(slide, slide, {
      sourcePosition: { x: 250, y: 5 },
      targetPosition: { x: 50, y: 5 },
    })

    await expect.element(screen.getByText('test2')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('表示要素を右にスライドすると要素が進む', async () => {
    const screen = await renderCarousel()
    const slide = screen.getByText('test1')

    await userEvent.dragAndDrop(slide, slide, {
      sourcePosition: { x: 50, y: 5 },
      targetPosition: { x: 250, y: 5 },
    })

    await expect.element(screen.getByText('test3')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('ページインジケーターをクリックするとそれが示す要素を表示する', async () => {
    const screen = await renderCarousel()

    await screen.getByRole('button', { name: '3 枚目のスライドを表示' }).click()

    await expect.element(screen.getByText('test3')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('表示中の要素を示すページインジケーターだけが現在位置として示される', async () => {
    const screen = await renderCarousel()

    await screen.getByRole('button', { name: '2 枚目のスライドを表示' }).click()

    await expect
      .element(screen.getByRole('button', { name: '2 枚目のスライドを表示' }))
      .toHaveAttribute('aria-current', 'true')
    await expect
      .element(screen.getByRole('button', { name: '1 枚目のスライドを表示' }))
      .toHaveAttribute('aria-current', 'false')
  })

  it('キーボードで矢印を操作すると表示要素が進む', async () => {
    const screen = await renderCarousel()

    await userEvent.keyboard('{Tab}{Tab}')
    await expect.element(screen.getByRole('button', { name: '次のスライド' })).toHaveFocus()
    await userEvent.keyboard('{Enter}')

    await expect.element(screen.getByText('test2')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('キーボードでページインジケーターを操作するとそれが示す要素を表示する', async () => {
    const screen = await renderCarousel()

    await userEvent.keyboard('{Tab}{Tab}{Tab}{Tab}{Tab}')
    await expect
      .element(screen.getByRole('button', { name: '3 枚目のスライドを表示' }))
      .toHaveFocus()
    await userEvent.keyboard(' ')

    await expect.element(screen.getByText('test3')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })
})
