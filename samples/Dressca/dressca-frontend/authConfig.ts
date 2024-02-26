import { LogLevel, PublicClientApplication } from '@azure/msal-browser';

export const b2cPolicies = {
  names: {
    signUpSignIn: 'B2C_1_signupsignintest001',
  },
  authorities: {
    signUpSignIn: {
      authority:
        'https://alesmaiamarisb2ctest.b2clogin.com/alesmaiamarisb2ctest.onmicrosoft.com/B2C_1_signupsignintest001',
    },
  },
  authorityDomain: 'alesmaiamarisb2ctest.b2clogin.com',
};

// Config object to be passed to Msal on creation
export const msalConfig = {
  auth: {
    clientId: 'f8021ade-a554-4e71-a15e-f8c93443f176',
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [b2cPolicies.authorityDomain],
    redirectUri: '/', // Must be registered as a SPA redirectURI on your app registration
    postLogoutRedirectUri: '/', // Must be registered as a SPA redirectURI on your app registration
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

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export const loginRequest = {
  scopes: ['demo.Read', 'demo.Write'],
};

export const tokenRequest = {
  scopes: ['demo.Read', 'demo.Write'],
};
