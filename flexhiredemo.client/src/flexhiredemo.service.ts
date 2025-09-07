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

  getJobApplications(limit:number|null): Observable<any> {
    return this.http.get(`${this.baseUrl}/jobapplications/${limit}`);
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

  unregisterWebhook(webhookId: string): Observable<any> {
    // Since the web API accepts a string in the body of the request,
    // the webhookId must be enclosed in quotations.
    return this.http.post(`${this.baseUrl}/flexhire/unregister-webhook`,
      JSON.stringify(webhookId), // wrap string in double quotes
      {
        headers: { 'Content-Type': 'application/json' }
      });
  }

  simulateWebhook(): Observable<any> {
    return this.http.post(`${this.baseUrl}/flexhire/simulate-webhook`, '');
  }
}
