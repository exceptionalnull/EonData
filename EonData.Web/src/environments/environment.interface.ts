export interface Environment {
  version: string;
  apiVersion: string;
  production: boolean;
  apiUrl: string;
  b2c: {
    tenantName: string;
    appId: string;
  }
}
