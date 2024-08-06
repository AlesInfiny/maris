import './assets/base.css';
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { authenticationGuard } from '@/shared/authentication/authentication-guard';
import { router } from './router';
import App from './App.vue';
import { worker } from '../mock/browser';

if (import.meta.env.MODE === 'mock') {
  worker.start({
    onUnhandledRequest: 'bypass',
  });
}

const app = createApp(App);
const pinia = createPinia();

app.config.errorHandler = (err: unknown, vm, info) => {
  // 本サンプルAPではログの出力とエラー画面への遷移を行っています。
  // APの要件によってはサーバーやログ収集ツールにログを送信し、エラーを握りつぶすこともあります。

  // eslint-disable-next-line no-console
  console.log(err, vm, info);
  router.replace({ name: 'error' });
};

app.use(pinia);
app.use(router);

authenticationGuard(router);

app.mount('#app');
