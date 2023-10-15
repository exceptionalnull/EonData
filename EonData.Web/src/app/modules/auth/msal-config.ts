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
      postLogoutRedirectUri: '/',
      /* The authorityMetadata below is a workaround for a bug that should be fixed in the next release after MSAL v3.0.1
       * See more: https://github.com/AzureAD/microsoft-authentication-library-for-js/issues/6340
       */
      authorityMetadata: '{ "token_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/token", "token_endpoint_auth_methods_supported": ["client_secret_post", "private_key_jwt", "client_secret_basic"], "jwks_uri": "https://login.microsoftonline.com/common/discovery/v2.0/keys", "response_modes_supported": ["query", "fragment", "form_post"], "subject_types_supported": ["pairwise"], "id_token_signing_alg_values_supported": ["RS256"], "response_types_supported": ["code", "id_token", "code id_token", "id_token token"], "scopes_supported": ["' + environment.b2c.appId + '", "openid", "profile", "email", "offline_access"], "issuer": "https://login.microsoftonline.com/' + environment.b2c.tenantName + '/v2.0", "request_uri_parameter_supported": false, "userinfo_endpoint": "https://graph.microsoft.com/oidc/userinfo", "authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/authorize", "device_authorization_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/devicecode", "http_logout_supported": true, "frontchannel_logout_supported": true, "end_session_endpoint": "https://login.microsoftonline.com/common/oauth2/v2.0/logout", "claims_supported": ["sub", "iss", "cloud_instance_name", "cloud_instance_host_name", "cloud_graph_host_name", "msgraph_host", "aud", "exp", "iat", "auth_time", "acr", "nonce", "preferred_username", "name", "tid", "ver", "at_hash", "c_hash", "email"], "kerberos_endpoint": "https://login.microsoftonline.com/common/kerberos", "tenant_region_scope": null, "cloud_instance_name": "microsoftonline.com", "cloud_graph_host_name": "graph.windows.net", "msgraph_host": "graph.microsoft.com", "rbac_url": "https://pas.windows.net" }'
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
