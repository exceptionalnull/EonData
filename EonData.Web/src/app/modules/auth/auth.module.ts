import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalBroadcastService, MsalGuard, MsalInterceptor, MsalModule, MsalService } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { CallbackComponent } from './components/callback/callback.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RouterModule } from '@angular/router';
import { ContactFormModule } from '../contact-form/contact-form.module';
import { MsalConfig } from './msal-config';

@NgModule({
  declarations: [
    LoginFormComponent,
    CallbackComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MsalModule,
    ContactFormModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MsalConfig.MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MsalConfig.MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MsalConfig.MSALInterceptorConfigFactory
    },
    MsalService,
    MsalGuard,
    MsalBroadcastService
  ],
  exports: [ MsalModule, LoginFormComponent ]
})
export class AuthModule { }
