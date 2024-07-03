import router from '@/router';
import { showToast } from '@/services/notification/notificationService';
import { CustomError, UnauthorizedError, NetworkError } from './custom-error';
import { AxiosError } from 'axios';

export function errorHandleBase(error: unknown, callback: () => void) {
  // ハンドリングできるエラーの場合はコールバックを実行
  if (error instanceof CustomError || error instanceof AxiosError) {
    callback();

    // エラーの種類によって共通処理を行う
    // switch だと instanceof での判定ができないため if 文で判定
    if (error instanceof UnauthorizedError) {
      router.replace({ name: 'authentication/login' });
      showToast('ログインしてください。');
    } else if (error instanceof NetworkError) {
      showToast('ネットワークエラーが発生しました。');
      console.log(error);
    }
  } else {
    // ハンドリングできないエラーの場合は上位にエラーを投げる
    throw error;
  }
}
