import { createApp, markRaw } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';

import '@/assets/base.css';
import '@/config/yup.config';

// Azure AD B2C に必要
import { msalInstance } from '../authConfig';

import { authenticationGuard } from '@/shared/authentication/authentication-guard';

const app = createApp(App);
const pinia = createPinia();

app.config.errorHandler = (err: unknown, vm, info) => {
  // 本サンプルAPではログの出力とエラー画面への遷移を行っています。
  // APの要件によってはサーバーやログ収集ツールにログを送信し、エラーを握りつぶすこともあります。
  console.log(err, vm, info);
  router.replace({ name: 'error' });
};

pinia.use(({ store }) => {
  store.router = markRaw(router);
});

app.use(pinia);
app.use(router);

// Azure AD B2C に必要
app.use(msalInstance);

authenticationGuard(router);

app.mount('#app');
