export class EnvService {

    public apiLogin = '';
    constructor() {
      this.mergeRuntimeSettings();
    }
    private mergeRuntimeSettings() {
      // Read environment variables from browser window
      const browserWindow = window || {};
      const browserWindowEnv = browserWindow['__env'] || {};
      // Assign environment variables from browser window to env
      // In the current implementation, properties from env.js overwrite defaults from the EnvService.
      // If needed, a deep merge can be performed here to merge properties instead of overwriting them.
      for (const key in browserWindowEnv) {
        if (browserWindowEnv.hasOwnProperty(key)) {
          this[key] = window['__env'][key];
        }
      }
    }
  }
