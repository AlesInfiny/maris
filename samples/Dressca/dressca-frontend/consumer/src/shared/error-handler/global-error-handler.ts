/* eslint-disable no-console */
import type { App, ComponentPublicInstance } from 'vue'
import { router } from '../../router'

export const globalErrorHandler = {
  /* eslint no-param-reassign: 0 */
  install(app: App) {
    app.config.errorHandler = (
      err: unknown,
      instance: ComponentPublicInstance | null,
      info: string,
    ) => {
      // 本サンプルAPではログの出力とエラー画面への遷移を行っています。
      // APの要件によってはサーバーやログ収集ツールにログを送信し、エラーを握りつぶすこともあります。
      console.error(err, instance, info)
      router.replace({ name: 'error' })
    }

    // Vue.js 以外のエラー
    // テストやデバッグ時にエラーの発生を検知するために利用する
    window.addEventListener('error', (event) => {
      console.error(event)
    })

    // テストやデバッグ時に予期せぬ非同期エラーの発生を検知するために利用する
    window.addEventListener('unhandledrejection', (event) => {
      console.error(event)
    })
  },
}
