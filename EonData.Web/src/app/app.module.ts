import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './components/home/home.component';
import { CloudControlModule } from './modules/cloud-control/cloud-control.module';
import { AboutComponent } from './components/about/about.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthModule } from './modules/auth/auth.module';
import { MsalRedirectComponent } from '@azure/msal-angular';
import { ContactFormModule } from './modules/contact-form/contact-form.module';
import { MatToolbarModule } from '@angular/material/toolbar';


// Get the browser's locale
const browserLocale: string = window.navigator.language;

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent
  ],
  bootstrap: [AppComponent, MsalRedirectComponent],
  imports: [
    AuthModule,
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    CloudControlModule,
    ContactFormModule,
    MatToolbarModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: browserLocale },
    provideHttpClient(withInterceptorsFromDi())
  ]
})
export class AppModule { }
