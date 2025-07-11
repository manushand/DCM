// This import is required for Zone.js
import 'zone.js';

import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app';
import { injectFavicon } from './app/app.config.favicon';

// Inject favicon to ensure it's displayed
injectFavicon();

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
