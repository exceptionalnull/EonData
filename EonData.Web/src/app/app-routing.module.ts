import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { BrowserUtils } from '@azure/msal-browser';
import { AboutComponent } from './components/about/about.component';
import { HomeComponent } from './components/home/home.component';

// application level routing definition
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule) },
  { path: 'clouds', loadChildren: () => import('./modules/cloud-control/cloud-control.module').then(m => m.CloudControlModule), canActivate: [MsalGuard] },
  { path: 'nms', loadChildren: () => import('./modules/nms-tools/nms-tools.module').then(m => m.NmsToolsModule) }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes, {
    useHash: false,
    // Don't perform initial navigation in iframes or popups
    initialNavigation: !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup() ? 'enabledNonBlocking' : 'disabled' // Set to enabledBlocking to use Angular Universal
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
