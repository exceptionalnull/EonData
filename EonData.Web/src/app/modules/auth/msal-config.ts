import { MsalGuardConfiguration, MsalInterceptorConfiguration, ProtectedResourceScopes } from '@azure/msal-angular';
import { Configuration, BrowserCacheLocation, LogLevel, IPublicClientApplication, PublicClientApplication, InteractionType } from '@azure/msal-browser';
import { environment } from '@environments/environment';
interface B2CPolicyNames {
  [key: string]: string;
}

export class MsalConfig {
  // map of protected API resources
  private static protectedResources = new Map<string, (string | ProtectedResourceScopes)[] | null>([
    [`${environment.apiUrl}/contact`, [
      { httpMethod: "POST", scopes: null },
      { httpMethod: "GET", scopes: ["https://eonid.onmicrosoft.com/eondata-api/default"] },
    ]],
    [`${environment.apiUrl}/*`, ["https://eonid.onmicrosoft.com/eondata-api/default"]],
    ['https://graph.microsoft.com/v1.0/me', ['user.read']]
  ]);

  private static readonly policyNames: B2CPolicyNames = {
    "SignUpSignIn": "B2C_1A_SIGNUP_SIGNIN",
    "EditProfile": "B2C_1A_PROFILEEDIT"
  }

  // MSAL instance configuration
  static readonly MSALConfig: Configuration = {
    auth: {
      clientId: environment.b2c.appId,
      authority: `https://${environment.b2c.tenantName}.b2clogin.com/${environment.b2c.tenantName}.onmicrosoft.com/${MsalConfig.policyNames["SignUpSignIn"]}`,
      knownAuthorities: [`${environment.b2c.tenantName}.b2clogin.com`],
      redirectUri: '/',
      postLogoutRedirectUri: '/'
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
      storeAuthStateInCookie: (window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1),
    },
    system: {
      allowNativeBroker: false,
      loggerOptions: {
        loggerCallback: MsalConfig.authLogMessage,
        logLevel: (environment.production) ? LogLevel.Error : LogLevel.Verbose,
        piiLoggingEnabled: false
      }
    }
  };

  // get a distinct list of all scopes in the protected resouce map
  private static readonly allScopes = Array.from(MsalConfig.protectedResources.values()).reduce((acc: string[], curr) => {
    if (Array.isArray(curr)) {
      curr.forEach(item => {
        if (typeof item === 'string' && !acc.includes(item)) {
          acc.push(item);
        }
        else if (typeof item === 'object' && item != null && 'scopes' in item) {
          // for ProtectedResourceScope objects filter out any null scopes
          (item.scopes || []).filter(scope => scope !== null).forEach(scope => {
            if (typeof scope === 'string' && !acc.includes(scope)) {
              acc.push(scope);
            }
          });
        }
      });
    }
    return acc;
  }, []);

  // MSAL guard configuration
  static readonly MSALGuardConfig: MsalGuardConfiguration = {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: MsalConfig.allScopes
    },
    loginFailedRoute: '/'
  };

  // MSAL interceptor configuration
  static readonly MSALInterceptorConfig: MsalInterceptorConfiguration = {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: MsalConfig.protectedResources
  };

  /* factory methods used in the module definition */
  static MSALInstanceFactory(): IPublicClientApplication {
    return new PublicClientApplication(this.MSALConfig);
  }

  static MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
    return this.MSALInterceptorConfig;
  }

  static MSALGuardConfigFactory(): MsalGuardConfiguration {
    return this.MSALGuardConfig;
  }

  // log message callback
  private static authLogMessage(logLevel: LogLevel, message: string) {
    console.log(`auth: ${logLevel} :: ${message}`);
  }
}
