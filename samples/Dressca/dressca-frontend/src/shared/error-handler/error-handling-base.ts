import router from '@/router';
import { showToast } from '@/services/notification/notificationService';
import { CustomError, UnauthorizedError, NetworkError } from './custom-error';
import { useRoutingStore } from '@/stores/routing/routing';

export function errorHandleBase(
  error: unknown,
  callback: () => void,
  handlingUnauthorizedError: (() => void) | null = null,
  handlingNetworkError: (() => void) | null = null,
) {
  // ハンドリングできるエラーの場合はコールバックを実行
  if (error instanceof CustomError) {
    callback();

    // エラーの種類によって共通処理を行う
    // switch だと instanceof での判定ができないため if 文で判定
    if (error instanceof UnauthorizedError) {
      if (handlingUnauthorizedError) {
        handlingUnauthorizedError();
      } else {
        const routingStore = useRoutingStore();
        routingStore.setRedirectFrom(router.currentRoute.value.path.slice(1));
        router.push({ name: 'authentication/login' });
        showToast('ログインしてください。');
      }
    } else if (error instanceof NetworkError) {
      if (handlingNetworkError) {
        handlingNetworkError();
      } else {
        showToast('ネットワークエラーが発生しました。');
      }
    }
  } else {
    // ハンドリングできないエラーの場合は上位にエラーを投げる
    throw error;
  }
}
