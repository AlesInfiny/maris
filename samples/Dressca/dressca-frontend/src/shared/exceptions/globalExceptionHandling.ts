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
    // 不明なエラーの場合はエラー画面に遷移、エラー画面からホームに戻れるようにする
    window.addEventListener('error', (event) => {
      console.log(event);
      router.replace({ name: 'error' });
    });

    window.addEventListener('unhandledrejection', (event) => {
      // やりたいことがあまりない
      // 開発時に予期せぬ非同期のエラーが発生した時に利用する
      // Console.log でエラー内容を確認することができます。
      console.log(event);
    });
  },
};

export default ExceptionHandlingPlugin;
