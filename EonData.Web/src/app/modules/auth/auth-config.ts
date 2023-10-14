import { ProtectedResourceScopes } from '@azure/msal-angular';
import { LogLevel, Configuration, BrowserCacheLocation } from '@azure/msal-browser';
import { environment } from '@environments/environment';

const isIE = window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1;

//export const b2cPolicies = {
//  names: {
//    signUpSignIn: "b2c_1_susi_reset_v2",
//    editProfile: "b2c_1_edit_profile_v2"
//  },
//  authorityDomain: `${environment.b2c.tenantName}.${environment.b2c.tenantDomain}`,
//  authorities: {
//    signUpSignIn: {
//      authority: `https://${authorityDomain}/your-tenant-name.onmicrosoft.com/b2c_1_susi_reset_v2`,
//    },
//    editProfile: {
//      authority: "https://your-tenant-name.b2clogin.com/your-tenant-name.onmicrosoft.com/b2c_1_edit_profile_v2"
//    }
//  },
//};

interface B2CPolicyNames {
  [key: string]: string;
}

class b2cPolicies {
  private static policyNames: B2CPolicyNames = {
    "SignUpSignIn": "B2C_1A_SIGNUP_SIGNIN",
    "EditProfile": "B2C_1A_PROFILEEDIT"
  }

  public static getPolicyUrl(policyName: string): string {
    return `https://${environment.b2c.tenantName}.b2clogin.com/${environment.b2c.tenantName}.onmicrosoft.com/${this.policyNames[policyName]}`
  }
}

export const msalConfig: Configuration = {
  auth: {
    clientId: environment.b2c.appId,
    authority: b2cPolicies.getPolicyUrl("SignUpSignIn"),
    knownAuthorities: [`${environment.b2c.tenantName}.b2clogin.com`],
    redirectUri: '/auth',
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage,
    storeAuthStateInCookie: isIE,
  },
  system: {
    loggerOptions: {
      loggerCallback: authLogMessage,
      logLevel: (environment.production) ? LogLevel.Error : LogLevel.Verbose,
      piiLoggingEnabled: false
    }
  }
}

function authLogMessage(logLevel: LogLevel, message: string) {
  console.log(`auth: ${logLevel} :: ${message}`);
}

export const protectedResources = new Map<string, (string | ProtectedResourceScopes)[] | null>([
  [`${environment.apiUrl}/contact`, [
    { httpMethod: "POST", scopes: null },
    { httpMethod: "GET",  scopes: ["https://eonid.onmicrosoft.com/eondata-api/default"] },
  ]],
  [`${environment.apiUrl}/*`, ["https://eonid.onmicrosoft.com/eondata-api/default"]],
  ['https://graph.microsoft.com/v1.0/me', ['user.read']]
]);

//export const loginRequest = {
//  scopes: []
//};
