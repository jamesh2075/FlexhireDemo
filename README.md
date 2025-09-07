# 🧰 FlexhireDemo

**FlexhireDemo** is a full-stack demo application built with **ASP.NET Core 8.0 Web API**, **Angular 20.2.2**, and **TypeScript 5.9.2**, showcasing integration with [Flexhire API](https://www.flexhire.com/) via **GraphQL**, 
and supporting real-time updates using **webhooks**.

---

## 🛠️ Technologies Used

| Tech Stack       | Purpose                                  |
|------------------|-------------------------------------------|
| ASP.NET Core     | Backend API and webhook handler           |
| Angular          | Frontend UI                              |
| SignalR          | Real-time updates from server to UI       |
| GraphQL          | Communication with Flexhire API          |
| Flexhire Webhooks| Receive updates (e.g., job applications) |

---

## 📦 Features

✅ Connects to the Flexhire GraphQL API  
✅ Queries job applications with pagination  
✅ Registers and handles Flexhire webhooks  
✅ Displays live updates via SignalR  
✅ Clean separation of backend/frontend  
✅ Built with modern .NET and Angular versions  

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Node.js & npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- A Flexhire API key (you must have access to the Flexhire platform)

---

### 🔧 Backend Setup (ASP.NET Core Web API)

1. Navigate to the backend project folder:
   cd FlexhireDemo.Server

3. Update the appsettings.json file with your Flexhire API key
  
   {
  "Flexhire": {
    "ApiUrl": "https://flexhire.com/api/v2",
    "ApiKey": "your-flexhire-api-key",
    "WebhookUrl": "http://flexhireapi.azurewebsites.net/api/flexhire/handlewebhook"
  }
}

4. Restore the .NET Core packages. Then build and run the backend API:
dotnet restore
dotnet run

5. Navigate to the front project folder:
cd..
cd frontend

5. Install the NodeJS packages. Then build and run the front-end web application:
npm install
ng serve

6. Open your browser to: http://localhost:4200
The Profile and Jobs tabs allow you to view your Flexhire profile and job applications.
The Tools tab allows you to:
* Specify another Flexhire API key
* Register a webhook for automatically receiving updates.
* Simulate a webhook that would originate from Flexhire
