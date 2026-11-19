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

/**
 * 矢印とページインジケーターを、画面上の並び順で取得します。
 * どれもアイコンだけで、ロールや名前を持たないためです。
 * @param container カルーセルを描画した要素。
 * @returns 左右の矢印と、要素ごとのページインジケーターのロケーター。
 */
function getControls(container: HTMLElement) {
  const [leftArrow, rightArrow, ...pageIndicators] = Array.from(
    container.querySelectorAll('svg'),
    (element) => page.elementLocator(element),
  )
  return { leftArrow: leftArrow!, rightArrow: rightArrow!, pageIndicators }
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

    await getControls(screen.container).rightArrow.click()

    await expect.element(screen.getByText('test2')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })

  it('左矢印をクリックすると表示要素が戻る', async () => {
    const screen = await renderCarousel()

    await getControls(screen.container).leftArrow.click()

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

    await getControls(screen.container).pageIndicators[2]!.click()

    await expect.element(screen.getByText('test3')).toBeVisible()
    await expect.element(screen.getByText('test1')).not.toBeInTheDocument()
  })
})
