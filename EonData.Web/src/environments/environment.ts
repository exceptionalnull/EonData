import { EnvironmentConstants } from './constants';
import { Environment } from './environment.interface';

export const environment : Environment = {
  production: false,
  apiUrl: 'https://localhost:7041',
  b2c: {
    tenantName: 'eonid',
    appId: '385afbcc-67d0-4337-af02-94715a52d794'
  },
  version: EnvironmentConstants.appVersion
};
