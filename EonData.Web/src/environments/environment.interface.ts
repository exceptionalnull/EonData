export interface Environment {
  production: boolean;
  apiUrl: string;
  b2c: {
    tenantName: string;
    appId: string;
  }
}
