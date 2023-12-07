import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ShareFolderModel } from './models/ShareFolderModel';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EonShareService {
  private readonly eonShareApiEndpoint = environment.apiUrl + '/fshare';
  constructor(private http: HttpClient) { }

  getFileShare(): Observable<ShareFolderModel[]> {
    return this.http.get<ShareFolderModel[]>(this.eonShareApiEndpoint);
  }
}
