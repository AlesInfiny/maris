import { App } from 'vue';
import router from '../../router';
import { useNotificationStore } from '@/stores/notification/notification';

const ExceptionHandlingPlugin = {
  install: function (app: App) {
    const notificationStore = useNotificationStore();

    app.config.errorHandler = (err: unknown, vm, info) => {
      // 本サンプルAPではログの出力とエラー画面への遷移を行っています。
      // APの要件によってはサーバーやログ収集ツールにログを送信し、エラーを握りつぶすこともあります。
      console.log(err, vm, info);
      router.replace({ name: 'error' });
    };

    // Vue.js 以外のエラー
    window.addEventListener('error', (event) => {
      console.log(event);
      router.replace({ name: 'error' });
    });

    window.addEventListener('unhandledrejection', (event) => {
      console.log(event);
      notificationStore.setMessage('エラーが発生しました。');
    });
  },
};

export default ExceptionHandlingPlugin;
