import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {

  public hubConnection: HubConnection;
  public messages: string[] = [];
  public amount: string;

  ngOnInit(): void {
    let builder = new HubConnectionBuilder();

    this.hubConnection = builder.withUrl("/hubs/payment").build();

    this.hubConnection.on("UpdateStatus", response => {
      this.messages.push(`Payment <b>${response.paymentReference}</b> status: <b>${response.status}</b> at ${this.formatTime()}`);
    });

    this.hubConnection.onclose(err => console.log(`Connection closed: ${err.message} at ${new Date().toTimeString()}`));
    this.hubConnection.start().catch(err => console.log(err.message));
  }

  submit(): void {
    this.hubConnection.invoke("Deposit", this.amount);
    this.amount = "";
  }

  formatTime(): string {
    let dateTime = new Date();
    return `${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}.${dateTime.getMilliseconds()}`;
  }
}
