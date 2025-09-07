import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WebhookSignalrService {
  private hubConnection!: signalR.HubConnection;

  // Observable to notify components about webhook events
  public webhookReceived$ = new BehaviorSubject<any>(null);

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5173/webhookHub`) // <-- Put your backend SignalR hub URL here
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch((err) => console.error('Error starting SignalR connection:', err));

    // Listen for webhook events pushed by the server
    this.hubConnection.on('WebhookReceived', (payload) => {
      console.log('Webhook event received:', payload);
      this.webhookReceived$.next(payload);
    });
  }
}
