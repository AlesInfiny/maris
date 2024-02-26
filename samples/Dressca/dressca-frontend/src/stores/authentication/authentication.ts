import { defineStore } from 'pinia';
import { string } from 'yup';
import { InteractionRequiredAuthError } from '@azure/msal-browser';
import { loginRequest, tokenRequest, msalInstance } from '../../../authConfig';

msalInstance.initialize();

export const useAuthenticationStore = defineStore({
  id: 'authentication',
  state: (token: string) => ({
    accessToken: token,
    homeAccountId: string,
    authenticated: false,
  }),
  actions: {
    async signIn() {
      if (!this.authenticated) {
        msalInstance
          .loginPopup(loginRequest)
          .then(function (loginResponse) {
            this.homeAccountId = loginResponse.account.homeAccountId;
            this.authenticated = true;
            this.accessToken = loginResponse.account.idToken;
          })
          .catch(function (error) {
            // TODO 認証に失敗した場合の処理
            console.log(error);
          });
      }
    },
    async getToken() {
      msalInstance
        .acquireTokenSilent(tokenRequest)
        .then(function (accessTokenResponse) {
          this.accessToken = accessTokenResponse.accessToken;
          this.authenticated = true;
          this.homeAccountId = accessTokenResponse.account.homeAccountId;
        })
        .catch(function (error) {
          if (error instanceof InteractionRequiredAuthError) {
            // ユーザーによる操作が必要な場合にスローされるエラーがスローされた場合、トークン呼び出しポップアップ画面を表示する。
            msalInstance
              .acquireTokenPopup(tokenRequest)
              .then(function (accessTokenResponse) {
                this.accessToken = accessTokenResponse.accessToken;
                this.authenticated = true;
                this.homeAccountId = accessTokenResponse.account.homeAccountId;
              })
              .catch(function (error) {
                console.log(error);
              });
          }
          console.log(error);
        });
    },
  },
  getters: {
    isAuthenticated(state) {
      return state.authenticated;
    },
    getAccessToken(state) {
      return state.accessToken;
    },
    getHomeAccountId(state) {
      return state.homeAccountId;
    },
  },
});
