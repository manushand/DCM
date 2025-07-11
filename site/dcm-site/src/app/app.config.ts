import { ApplicationConfig, provideBrowserGlobalErrorListeners, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS, withJsonpSupport } from '@angular/common/http';
import { CorsInterceptor } from './interceptors/cors.interceptor';
import { BrowserModule } from '@angular/platform-browser';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(BrowserModule),
    provideBrowserGlobalErrorListeners(),
    // Using Zone.js for change detection to ensure HTTP responses trigger UI updates
    // explicitly not using provideZonelessChangeDetection() here
    provideRouter(routes),
    provideHttpClient(
      withInterceptorsFromDi(),
      withJsonpSupport()
    ),
    { provide: HTTP_INTERCEPTORS, useClass: CorsInterceptor, multi: true }
  ]
};
