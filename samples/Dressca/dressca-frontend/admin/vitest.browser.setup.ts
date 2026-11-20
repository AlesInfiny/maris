import { beforeAll, afterEach, afterAll, vi } from 'vitest'
import '@/core/styles/base.css'
import { worker } from './mock/browser'

/*
 * Vitest ブラウザーモードで、共通で実行したい処理を定義する設定ファイルです。
 * アプリケーションと同じスタイルを読み込み、CSS を反映した表示状態を検証できるようにします。
 * また、モックの Service Worker を起動、初期化、終了します。
 */

beforeAll(async () => {
  await worker.start({
    quiet: true,
    // Vite のモジュール読み込みも Service Worker を経由するため、モック漏れとして扱うのは API へのリクエストだけにします。
    onUnhandledRequest(request, print) {
      if (new URL(request.url).pathname.startsWith('/api/')) {
        print.error()
      }
    },
  })
})

afterEach(() => {
  worker.resetHandlers()
  vi.restoreAllMocks()
})

afterAll(() => {
  worker.stop()
})
