import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs';
import { ContactService } from '../../contact-form.service';

@Component({
  selector: 'app-contact-notification',
  templateUrl: './new-message-notification.component.html',
  styleUrls: ['./new-message-notification.component.scss']
})
export class NewMessageNotificationComponent implements OnInit {
  public unreadMessageCount: number = 0;

  constructor(private contactFormService: ContactService) { }

  ngOnInit() {
    timer(0, 22.2 * 60000).subscribe((i) => {
      this.contactFormService.getMessageCount(true)
        .subscribe((c) => {
          this.unreadMessageCount = c;
        });
    });
  }
}
