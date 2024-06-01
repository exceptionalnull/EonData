import { Component } from '@angular/core';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'EonData.Web';
  toolbarBackground = '/assets/spacegrid.png';
  toolbarStyle: { [klass: string]: string; } = {
    'border': 'none',
    'border-radius': '1.618em',
    'background-image': 'url(' + this.toolbarBackground + ')',
    'background-attachment': 'fixed',
    'background-size': 'cover'
  };
  appVersion = environment.version;
  environmentLabel = (environment.production) ? "" : "::DEV";
}
