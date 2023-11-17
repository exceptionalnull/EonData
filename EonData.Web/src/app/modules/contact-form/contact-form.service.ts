import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { SendMessageModel } from './models/SendMessageModel';
import { ContactMessageModel } from './models/ContactMessageModel';
import { MessageListResponseModel } from './models/MessageListResponseModel';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly contactFormApiEndpoint = environment.apiUrl + '/contact';

  constructor(private http: HttpClient) { }

  submitContactForm(message: SendMessageModel) {
    return this.http.post(this.contactFormApiEndpoint, message);
  }

  getMessageCount(unread?: boolean): Observable<number> {
    const unreadParam = (unread !== undefined) ? `?unread=${unread}` : '';
    return this.http.get<number>(`${this.contactFormApiEndpoint}/total${unreadParam}`);
  }

  getMessageList(unread?: boolean): Observable<MessageListResponseModel> {
    const unreadParam = (unread !== undefined) ? `?unread=${unread}` : '';
    return this.http.get<MessageListResponseModel>(`${this.contactFormApiEndpoint}${unreadParam}`);
  }

  getMessage(messageId: string): Observable<ContactMessageModel | null> {
    return this.http.get<ContactMessageModel | null>(`${this.contactFormApiEndpoint}/${messageId}`);
  }

  setRead(messageId: string): Observable<object> {
    return this.http.put(`${this.contactFormApiEndpoint}/${messageId}/setread`, '');
  }
}
