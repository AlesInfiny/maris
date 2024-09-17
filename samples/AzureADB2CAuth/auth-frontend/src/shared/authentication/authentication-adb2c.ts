import {
  BrowserAuthError,
  InteractionRequiredAuthError,
} from '@azure/msal-browser';
import type { AccountFilter } from '@azure/msal-common';
import {
  msalInstance,
  loginRequest,
  tokenRequest,
} from './authentication-config';

msalInstance.initialize();

export async function signInAzureADB2C(): Promise<boolean> {
  try {
    const loginResponse = await msalInstance.loginPopup(loginRequest);
    return true;
  } catch (error) {
    if (error instanceof BrowserAuthError) {
      return false;
    }
    throw error;
  }
}

export async function getTokenAzureADB2C(): Promise<string> {
  const accountFilter = {} as AccountFilter;
  accountFilter.homeAccountId = msalInstance.getActiveAccount()?.homeAccountId;

  const account = msalInstance.getAccount(accountFilter);

  tokenRequest.account = account ?? undefined;
  try {
    const tokenResponse = await msalInstance.acquireTokenSilent(tokenRequest);

    if (!tokenResponse.accessToken || tokenResponse.accessToken === '') {
      throw new InteractionRequiredAuthError('accessToken is null or empty.');
    }
    return tokenResponse.accessToken;
  } catch (error) {
    if (error instanceof InteractionRequiredAuthError) {
      // ユーザーによる操作が必要な場合にスローされるエラーがスローされた場合、トークン呼び出しポップアップ画面を表示する。
      const tokenResponse = await msalInstance.acquireTokenPopup(tokenRequest);
      return tokenResponse.accessToken;
    }
    // eslint-disable-next-line no-console
    console.log(error);
    throw error;
  }
}
