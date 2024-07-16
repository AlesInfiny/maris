import { resolve } from 'path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue';
import vueJsx from '@vitejs/plugin-vue-jsx';

// https://vitejs.dev/config/
export default defineConfig({
  build: {
    lib: {
      entry: resolve(__dirname, 'src/index.ts'), // エントリポイント
      name: 'CommonLib', // グローバル変数として公開するライブラリの変数名
      fileName: 'common-lib', // 生成するファイルのファイル名を指定します。
      formats: ['es']
    },
  },
  plugins: [
    vue(),
    vueJsx()]
})
