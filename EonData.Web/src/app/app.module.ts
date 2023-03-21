import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HomeComponent } from './components/home/home.component';
import { CloudControlModule } from './modules/cloud-control/cloud-control.module';
import { AboutComponent } from './components/about/about.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthModule } from './modules/auth/auth.module';
import { MsalRedirectComponent } from '@azure/msal-angular';
import { ContactFormModule } from './modules/contact-form/contact-form.module';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent
  ],
  imports: [
    AuthModule,
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    CloudControlModule,
    ContactFormModule
  ],
  providers: [],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
