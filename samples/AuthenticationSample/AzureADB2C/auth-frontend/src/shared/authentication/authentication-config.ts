import { LogLevel, PublicClientApplication } from '@azure/msal-browser';

export const b2cPolicies = {
  names: {
    signUpSignIn: import.meta.env.VITE_ADB2C_B2CPOLICIES_NAMES_SIGNUP_SIGNIN,
  },
  authorities: {
    signUpSignIn: {
      authority: import.meta.env.VITE_ADB2C_AUTHORITIES_SIGNUP_SIGNIN_AUTHORITY,
    },
  },
  authorityDomain: import.meta.env.VITE_ADB2C_B2CPOLICIES_AUTHORITYDOMAIN,
};

// Config object to be passed to Msal on creation
export const msalConfig = {
  auth: {
    clientId: import.meta.env.VITE_ADB2C_APP_CLIENT_ID,
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [b2cPolicies.authorityDomain],
    redirectUri: import.meta.env.VITE_ADB2C_APP_URI,
  },
  cache: {
    cacheLocation: 'localStorage',
  },
  system: {
    loggerOptions: {
      loggerCallback: (
        level: LogLevel,
        message: string,
        containsPii: boolean,
      ) => {
        if (containsPii) {
          return;
        }
        switch (level) {
          case LogLevel.Error:
            console.error(message);
            return;
          case LogLevel.Info:
            console.info(message);
            return;
          case LogLevel.Verbose:
            console.debug(message);
            return;
          case LogLevel.Warning:
            console.warn(message);
            return;
          default:
            return;
        }
      },
      logLevel: LogLevel.Verbose,
    },
  },
};

export const msalInstance = new PublicClientApplication(msalConfig);

// 配列内で import.meta.env を呼び出そうとすると何でか最初の1文字しか読み込んでくれない
export const loginRequest = {
  scopes: [
    'openid',
    'https://alesmaiamarisb2ctest.onmicrosoft.com/alesmaris-api/api.read',
  ],
};

export const tokenRequest = {
  scopes: [
    'https://alesmaiamarisb2ctest.onmicrosoft.com/alesmaris-api/api.read',
  ],
};
