import { NgModule } from '@angular/core';
import { RouterModule, Routes, UrlSegment } from '@angular/router';
import { EonShareComponent } from './components/eonshare/eonshare.component';

export function MultiSegmentUrlMatcher(url: UrlSegment[]) {
  return { consumed: url, posParams: { objectKey: new UrlSegment(url.map(segment => segment.path).join('/'), {}) } };
}

const routes: Routes = [
  { path: '', component: EonShareComponent },
  { matcher: MultiSegmentUrlMatcher, component: EonShareComponent}
  /*{ path: ':objectKey', component: EonshareComponent }*/
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EonshareRoutingModule { }
