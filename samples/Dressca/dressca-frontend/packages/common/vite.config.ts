import { fileURLToPath, URL } from 'url';
import { resolve } from 'path';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueJsx from '@vitejs/plugin-vue-jsx';

// https://vitejs.dev/config/
export default defineConfig({
  build: {
    lib: {
      entry: resolve(__dirname, 'src/index.ts'),
      formats: ['es'],
    },
    rollupOptions: {
      output: {
        entryFileNames: `dist/[name]-[hash].js`,
        globals: {
          vue: 'Vue',
        },
      },
      external: ['vue'],
    },
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  plugins: [vue(), vueJsx()],
});
