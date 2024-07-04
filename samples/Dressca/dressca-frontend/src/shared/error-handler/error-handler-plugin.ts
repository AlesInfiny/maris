import type { App } from 'vue';
import router from '../../router';

const ErrorHandlerPlugin = {
  install: function (app: App) {
    app.config.errorHandler = (err: unknown, vm, info) => {
      // 本サンプルAPではログの出力とエラー画面への遷移を行っています。
      // APの要件によってはサーバーやログ収集ツールにログを送信し、エラーを握りつぶすこともあります。
      console.log(err, vm, info);
      router.replace({ name: 'error' });
    };

    // Vue.js 以外のエラー
    // テストやデバッグ時にエラーの発生を検知するために利用する
    window.addEventListener('error', (event) => {
      console.log(event);
    });

    window.addEventListener('unhandledrejection', (event) => {
      // やりたいことがあまりない
      // テストやデバッグ時に予期せぬ非同期エラーの発生を検知するために利用する
      console.log(event);
    });
  },
};

export default ErrorHandlerPlugin;
