import { createApp, markRaw } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';

import '@/assets/base.css';
import '@/config/yup.config';
import '@/config/axios.config';
import 'vue-virtual-carousel/dist/style.css';

import { authenticationGuard } from '@/shared/authentication/authentication-guard';

const app = createApp(App);
const pinia = createPinia();

app.config.errorHandler = (err: unknown, vm, info) => {
  // エラーに対する処理
  console.log(err, vm, info);
  router.replace('/error');
};

pinia.use(({ store }) => {
  store.router = markRaw(router);
});

app.use(pinia);
app.use(router);

authenticationGuard(router);

app.mount('#app');
