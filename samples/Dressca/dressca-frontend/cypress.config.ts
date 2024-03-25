import { defineConfig } from 'cypress';
import { setupNodeEvents } from './cypress/plugins/index';

export default defineConfig({
  e2e: {
    // We've imported your old cypress plugins here.
    // You may want to clean this up later by importing these.
    setupNodeEvents,
    baseUrl: 'http://localhost:5050',
  },
});
