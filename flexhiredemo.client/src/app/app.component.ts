import { Component, OnInit } from '@angular/core';
import { FlexhireDemoService } from '../flexhiredemo.service';
import { WebhookSignalrService } from '../signarlr.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  activeTab: string = 'profile';
  profile: any = null;
  jobs: any[] = [];
  limitOptions = [
    { label: '10', value: 10 },
    { label: '25', value: 25 },
    { label: '50', value: 50 },
    { label: 'All', value: 100 }
  ];
  selectedLimit: number = 10;
  webhookId: any = "";

  constructor(
    private flexhireDemoService: FlexhireDemoService,
    private webhookSignalrService: WebhookSignalrService)
  {
  }

  ngOnInit(): void {
    this.getProfile();
    this.getJobs();

    this.webhookSignalrService.startConnection();
    // Subscribe to webhook events and reload jobs on event
    this.webhookSignalrService.webhookReceived$.subscribe((payload) => {
      if (payload) {
        console.log('Reloading due to webhook...');
        this.getProfile();
        this.getJobs();
      }
    });
  }

  getProfile(): void {
    this.flexhireDemoService.getProfile().subscribe(data => {
      this.profile = data;
      this.webhookId = this.profile?.webhookId;
      sessionStorage.setItem("webhookId", this.profile?.webhookId);
    });
  }

  getJobs(): void {
    const limit = this.selectedLimit === 100 ? null : this.selectedLimit;
    this.flexhireDemoService.getJobApplications(limit).subscribe(data => {
      this.jobs = data;
    });
  }

  switchTab(tabName: string): void {
    this.activeTab = tabName;
  }

  onLimitChange() {
    this.getJobs();
  }

  apiKeyInput: string = '';

  applyApiKey() {
    if (!this.apiKeyInput) {
      alert('Please enter a valid API key.');
      return;
    }

    // Call backend to store API key
    this.flexhireDemoService.updateApiKey(this.apiKeyInput).subscribe({
      next: () => {
        window.location.reload(); // Or use Angular routing to reload data
      },
      error: err => alert('Failed to apply API key: ' + err.message)
    });
  }

  registerWebhook() {
    this.flexhireDemoService.registerWebhook().subscribe({
      next: (webhook) => {
        this.webhookId = webhook.id;
        sessionStorage.setItem("webhookId", webhook.id);
        alert(`Webhook ${webhook.id} registered with Flexhire.`);
      },
      error: err => alert('Webhook registration failed: ' + err.message)
    });
  }

  unregisterWebhook() {
    this.flexhireDemoService.unregisterWebhook(this.webhookId).subscribe({
      next: (webhook) => {
        this.webhookId = "";
        alert(`Webhook ${webhook.id} unregistered with Flexhire.`);
      },
      error: err => alert('Webhook de-registration failed: ' + err.message)
    });
  }

  simulateWebhook() {
    this.flexhireDemoService.simulateWebhook().subscribe({
      next: () => alert('Webhook simulated.'),
      error: err => alert('Webhook simulation failed: ' + err.message)
    });
  }
}
