/// <reference types="vite/client" />
interface ImportMetaEnv {
  readonly VITE_NO_ASSET_URL: string;
  readonly VITE_ASSET_URL: string;
  readonly VITE_BACKEND_ENDPOINT_ORIGIN: string;
  readonly VITE_API_PREFIX: string;
  readonly VITE_ADB2C_B2CPOLICIES_NAMES_SIGNUP_SIGNIN: string;
  readonly VITE_ADB2C_AUTHORITIES_SIGNUP_SIGNIN_AUTHORITY: string;
  readonly VITE_ADB2C_B2CPOLICIES_AUTHORITYDOMAIN: string;
  readonly VITE_ADB2C_SCOPE: string;
  readonly VITE_ADB2C_APP_CLIENT_ID: string;
  readonly VITE_ADB2C_APP_URI: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
