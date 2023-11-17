import { MsalGuardConfiguration, MsalInterceptorConfiguration, ProtectedResourceScopes } from '@azure/msal-angular';
import { Configuration, BrowserCacheLocation, LogLevel, IPublicClientApplication, PublicClientApplication, InteractionType } from '@azure/msal-browser';
import { environment } from '@environments/environment';
interface B2CPolicyNames {
  [key: string]: string;
}

export class MsalConfig {
  private static readonly apiScopeUri = `https://${environment.b2c.tenantName}.onmicrosoft.com/eondata-api/default`;

  // map of protected API resources
  private static protectedResources = new Map<string, (string | ProtectedResourceScopes)[] | null>([
    // contact endpoint should allow anon to use POST
    [`${environment.apiUrl}/contact`, [
      { httpMethod: "POST", scopes: null },
      { httpMethod: "GET", scopes: [MsalConfig.apiScopeUri] },
    ]],
    // api requires auth by default
    [`${environment.apiUrl}/*`, [MsalConfig.apiScopeUri]],
    // scopes for ms graph
    ['https://graph.microsoft.com/v1.0/me', ['user.read']]
  ]);

  private static readonly policyNames: B2CPolicyNames = {
    "SignUpSignIn": "B2C_1A_SIGNUP_SIGNIN"
    //"EditProfile": "B2C_1A_PROFILEEDIT"
  }

  // MSAL instance configuration
  static readonly MSALConfig: Configuration = {
    auth: {
      clientId: environment.b2c.appId,
      authority: `https://${environment.b2c.tenantName}.b2clogin.com/${environment.b2c.tenantName}.onmicrosoft.com/${MsalConfig.policyNames["SignUpSignIn"]}`,
      knownAuthorities: [`${environment.b2c.tenantName}.b2clogin.com`],
      redirectUri: '/auth',
      postLogoutRedirectUri: '/auth'
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
      storeAuthStateInCookie: (window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1),
    },
    system: {
      allowNativeBroker: false,
      loggerOptions: {
        loggerCallback: MsalConfig.authLogMessage,
        logLevel: (environment.production) ? LogLevel.Error : LogLevel.Warning,
        piiLoggingEnabled: false
      }
    }
  };

  // MSAL guard configuration
  static readonly MSALGuardConfig: MsalGuardConfiguration = {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [MsalConfig.apiScopeUri]
    },
    loginFailedRoute: '/auth'
  };

  // MSAL interceptor configuration
  static readonly MSALInterceptorConfig: MsalInterceptorConfiguration = {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: MsalConfig.protectedResources
  };

  /* factory methods used in the module definition */
  static MSALInstanceFactory(): IPublicClientApplication {

    return new PublicClientApplication(MsalConfig.MSALConfig);
  }

  static MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
    return MsalConfig.MSALInterceptorConfig;
  }

  static MSALGuardConfigFactory(): MsalGuardConfiguration {
    return MsalConfig.MSALGuardConfig;
  }

  // log message callback
  private static authLogMessage(logLevel: LogLevel, message: string) {
    console.log(`auth: ${logLevel} :: ${message}`);
  }
}
