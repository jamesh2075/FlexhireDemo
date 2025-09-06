import { Component, OnInit } from '@angular/core';
import { FlexhireDemoService } from '../flexhiredemo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  activeTab: string = 'profile';
  profile: any = null;
  jobs: any[] = [];

  constructor(private flexhireDemoService: FlexhireDemoService) { }

  ngOnInit(): void {
    this.getProfile();
    this.getJobs();
  }

  getProfile(): void {
    this.flexhireDemoService.getProfile().subscribe(data => {
      this.profile = data;
    });
  }

  getJobs(): void {
    this.flexhireDemoService.getJobApplications().subscribe(data => {
      this.jobs = data;
    });
  }

  switchTab(tabName: string): void {
    this.activeTab = tabName;
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
      next: () => alert('Webhook registered with Flexhire.'),
      error: err => alert('Webhook registration failed: ' + err.message)
    });
  }

  simulateWebhook() {
    this.flexhireDemoService.simulateWebhook().subscribe({
      next: () => alert('Webhook simulated.'),
      error: err => alert('Webhook simulation failed: ' + err.message)
    });
  }
}
