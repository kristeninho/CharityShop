import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/auth.service'
import { MultipleProductsResult } from '../../interfaces/IMultipleProductsResult';
import { SingleProductResult } from '../../interfaces/ISingleProductResult';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection : HubConnection;
  public productsUpdated = new Subject<MultipleProductsResult>();
  public productUpdated = new Subject<SingleProductResult>();

  constructor(private authService: AuthService) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:8080/productHub', {
        accessTokenFactory: () => this.authService.getToken() || '', // Pass JWT to connection
      })
      .build();

      this.initializeConnection();
      this.registerOnServerEvents();
   }

   private initializeConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => 
        {
          console.error('Error while starting connection: ' + err)
          // Token might be expired, try to re-authenticate
          this.reauthenticateAndRestartConnection();
        });
  }
  private reauthenticateAndRestartConnection() {
    const token = this.authService.getToken();
    if (token) {
      // Try to refresh the token or re-login
      this.authService.login('user', 'pass').subscribe({
        next: () => {
          console.log('Re-authenticated successfully');
          this.initializeConnection(); // Restart the connection
        },
        error: (err) => console.error('Re-authentication failed', err),
      });
    }
  }

  private registerOnServerEvents() {
    // Handle product updates
    this.hubConnection.on('UpdateMultipleProducts', (result: MultipleProductsResult) => {
      this.productsUpdated.next(result); // Emit the updated products
    });
    this.hubConnection.on('UpdateSingleProduct', (result: SingleProductResult) => {
      this.productUpdated.next(result); // Emit the updated product
    });
  }

  public bookProduct(productId: number) {
    console.log(productId);
    this.hubConnection.invoke('BookProduct', productId.toString()).catch((err) => console.error(err));
  }
  
}
