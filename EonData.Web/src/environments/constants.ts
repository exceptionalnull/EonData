import packageJson from '../../package.json';

export class EnvironmentConstants {
  static readonly appVersion: string = packageJson.version;
}
