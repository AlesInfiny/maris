import { fileURLToPath, URL } from 'url';

import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueJsx from '@vitejs/plugin-vue-jsx';
import { setupMockPlugin } from './vite-plugins/setup-mock';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const plugins = [vue(), vueJsx()];

  return {
    plugins: mode === 'mock' ? [...plugins, setupMockPlugin()] : plugins,
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
  };
});
