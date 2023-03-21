import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ContactMessageModel } from './models/ContactMessageModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private readonly contactFormApiEndpoint = environment.apiUrl + '/contact';

  constructor(private http: HttpClient) { }

  submitContactForm(message: ContactMessageModel) {
    return this.http.post(this.contactFormApiEndpoint, message);
  }

  getMessageCount(onlyUnread: boolean): Observable<number> {
    return this.http.get<number>(`${this.contactFormApiEndpoint}/total?unread=${onlyUnread}`);
  }
}
