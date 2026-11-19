import { describe, it, expect, beforeEach } from 'vitest'
import { render } from 'vitest-browser-vue'
import { createPinia, setActivePinia, type Pinia } from 'pinia'
import { defineComponent, h } from 'vue'
import { useAuthenticationStore } from '../../stores/authentication'
import AuthenticationMenu from '../AuthenticationMenu.vue'

let pinia: Pinia

beforeEach(() => {
  sessionStorage.clear()
  pinia = createPinia()
  setActivePinia(pinia)
})

/**
 * 遷移先を href に描画するだけの RouterLink のスタブです。
 * 認証のコンテキストは vue-router を参照できないため、ルーターの代わりに使用します。
 */
const RouterLinkStub = defineComponent({
  props: { to: { type: String, required: true } },
  setup(props, { slots }) {
    return () => h('a', { href: props.to }, slots.default?.())
  },
})

/**
 * メニューをテスト用に描画するヘルパー関数です。
 * @returns 描画結果。
 */
function renderMenu() {
  return render(AuthenticationMenu, {
    props: { loginLocation: '/authentication/login' },
    global: { plugins: [pinia], stubs: { RouterLink: RouterLinkStub } },
  })
}

describe('AuthenticationMenu', () => {
  it('未認証_ログインのリンクを指定した遷移先で表示する', async () => {
    // Arrange
    // Act
    const screen = await renderMenu()
    // Assert
    await expect
      .element(screen.getByRole('link', { name: 'ログイン' }))
      .toHaveAttribute('href', '/authentication/login')
  })

  it('認証済み_ログアウトのボタンを表示する', async () => {
    // Arrange
    useAuthenticationStore(pinia).signIn()
    // Act
    const screen = await renderMenu()
    // Assert
    await expect.element(screen.getByRole('button', { name: 'ログアウト' })).toBeVisible()
    await expect.element(screen.getByRole('link')).not.toBeInTheDocument()
  })

  it('認証済みでログアウトする_未認証になりloggedOutを発行してログインのリンクに切り替わる', async () => {
    // Arrange
    useAuthenticationStore(pinia).signIn()
    const screen = await renderMenu()
    // Act
    await screen.getByRole('button', { name: 'ログアウト' }).click()
    // Assert
    await expect.element(screen.getByRole('link', { name: 'ログイン' })).toBeVisible()
    await expect.element(screen.getByRole('button')).not.toBeInTheDocument()
    expect(useAuthenticationStore(pinia).isAuthenticated).toBe(false)
    expect(screen.emitted('loggedOut')).toHaveLength(1)
  })
})
