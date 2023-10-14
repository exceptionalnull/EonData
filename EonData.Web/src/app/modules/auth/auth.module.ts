import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MsalGuard, MsalInterceptor, MsalModule, MsalRedirectComponent } from '@azure/msal-angular';
import { InteractionType, PublicClientApplication } from '@azure/msal-browser';
import { msalConfig, protectedResources } from './auth-config';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { CallbackComponent } from './components/callback/callback.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RouterModule } from '@angular/router';
import { ContactFormModule } from '../contact-form/contact-form.module';




@NgModule({
  declarations: [
    LoginFormComponent,
    CallbackComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MsalModule.forRoot(new PublicClientApplication(msalConfig),
      {
        interactionType: InteractionType.Redirect,
        authRequest: {
          scopes: ["https://eonid.onmicrosoft.com/eondata-api/default"]
        },
        loginFailedRoute: '/auth/failed'
      },
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: protectedResources
      }),
      ContactFormModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    MsalGuard
  ],
  exports: [ MsalModule, LoginFormComponent ]
})
export class AuthModule { }
