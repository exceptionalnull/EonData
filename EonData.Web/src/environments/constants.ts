import packageJson from '../../package.json';

export class EnvironmentConstants {
  static readonly appVersion: string = packageJson.version;
  static readonly apiVersion: string = "0.1.0.0";
}
