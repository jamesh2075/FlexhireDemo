import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlexhireDemoService {
  private baseUrl = 'http://localhost:5173/api';

  constructor(private http: HttpClient) { }

  getProfile(): Observable<any> {
    return this.http.get(`${this.baseUrl}/profile`);
  }

  getJobApplications(): Observable<any> {
    return this.http.get(`${this.baseUrl}/jobapplications`);
  }

  updateApiKey(apiKey: string): Observable<any> {
    const body = {
      apiKey: apiKey
    };

    return this.http.post(`${this.baseUrl}/flexhire/apikey`, body);
  }

  registerWebhook(): Observable<any> {
    return this.http.post(`${this.baseUrl}/flexhire/register-webhook`, '');
  }

  simulateWebhook(): Observable<any> {
    return this.http.post(`${this.baseUrl}/flexhire/simulate-webhook`, '');
  }
}
