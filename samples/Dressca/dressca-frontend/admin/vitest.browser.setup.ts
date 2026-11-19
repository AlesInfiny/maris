import { beforeAll, afterEach, afterAll, vi } from 'vitest'
import { worker } from './mock/browser'

/*
 * Vitest ブラウザーモードで、共通で実行したい処理を定義する設定ファイルです。
 * ブラウザー上ではモックの Service Worker を起動、初期化、終了します。
 */

beforeAll(async () => {
  await worker.start({ onUnhandledRequest: 'bypass' })
})

afterEach(() => {
  worker.resetHandlers()
  vi.restoreAllMocks()
})

afterAll(() => {
  worker.stop()
})
