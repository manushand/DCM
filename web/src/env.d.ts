/// <reference types="react-scripts" />

declare namespace NodeJS {
  interface ProcessEnv {
    readonly NODE_ENV: 'development' | 'production' | 'test';
    readonly PUBLIC_URL: string;
    readonly REACT_APP_USE_MOCKS?: string;
    // Add more if needed
    // readonly REACT_APP_API_BASE_URL?: string;
  }
}
