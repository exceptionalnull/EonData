import { EnvironmentConstants } from './constants';
import { Environment } from './environment.interface';

export const environment: Environment = {
  production: true,
  apiUrl: 'https://api.eondata.net/api',
  b2c: {
    tenantName: 'eonid',
    appId: '385afbcc-67d0-4337-af02-94715a52d794'
  },
  /* constants */
  version: EnvironmentConstants.appVersion,
  apiVersion: EnvironmentConstants.apiVersion
};
