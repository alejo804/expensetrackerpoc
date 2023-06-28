import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-expense-form',
  templateUrl: './expense-form.component.html',
  styleUrls: ['./expense-form.component.css']
})
export class ExpenseFormComponent {
  expenseDescription: string = '';
  expenseAmount: number = 0;
  private hubConnection: HubConnection | undefined;
  apiUrl = environment.apiUrl;



  constructor(private http: HttpClient) {
    this.initializeSignalR();
  }

  private initializeSignalR() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://signalrhubdr.service.signalr.net', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }) // Replace with the URL of your SignalR hub
      .build();

    this.hubConnection.on('ExpenseRegistered', (response: any) => {
      console.log('Expense registered:', response);
      // Handle the response from SignalR here
    });

    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connected successfully.');
      })
      .catch(err => {
        console.error('Error connecting to SignalR:', err);
      });
  }

  registerExpense() {
    const expense = {
      description: this.expenseDescription,
      amount: this.expenseAmount
    };

    this.http.post(this.apiUrl, expense) // Replace with the URL of your HTTPS API
      .subscribe(
        (response) => {
          console.log('Expense registered successfully:', response);
        },
        (error) => {
          console.error('Error registering expense:', error);
        }
      );
  }
}
