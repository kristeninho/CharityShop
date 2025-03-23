import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from '../../services/signalR/signalr.service';
import { ProductResponseDto } from '../../interfaces/IProductResponseDto';
import { ProductsService } from '../../services/products/products.service';
import { MultipleProductsResult } from '../../interfaces/IMultipleProductsResult';
import { CommonModule } from '@angular/common';
import { ProductRequestDto } from '../../interfaces/IProductRequestDto';
import { SingleProductResult } from '../../interfaces/ISingleProductResult';
import { MatDialog } from '@angular/material/dialog';
import { CheckoutModalComponent } from '../checkout-modal/checkout-modal.component';

@Component({
  selector: 'app-shop',
  imports: [CommonModule],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent  implements OnInit, OnDestroy{
  products: ProductResponseDto[] = [];
  basket: ProductRequestDto[] = [];
  totalPayment: number = 0;
  currency = "€"

  // For SignalR
  public messages: string[] = [];
  
  constructor(private signalrService: SignalrService, 
    private productsService: ProductsService,
    private dialog: MatDialog
  ) {}
  
  ngOnInit(): void {
    this.signalrService.productsUpdated.subscribe((result: MultipleProductsResult) => {
      this.updateProducts(result);
    });
    this.signalrService.productUpdated.subscribe((result: SingleProductResult) => {
      this.updateProduct(result);
    });

    this.fetchProducts();

    // Add beforeunload event listener
    window.addEventListener('beforeunload', this.resetBasketOnUnload.bind(this));
  }

  ngOnDestroy(): void {
    // Remove beforeunload event listener
    window.removeEventListener('beforeunload', this.resetBasketOnUnload.bind(this));

    // Call resetBasket when the component is destroyed
    this.resetBasketNoConfirmation();
  }

  private resetBasketOnUnload(event: BeforeUnloadEvent): void {
    // Call resetBasket when the page is about to be unloaded
    this.resetBasketNoConfirmation();
  }

  private updateProduct(result: SingleProductResult): void {
    if (result.product) {
      // Update the existing products array
      this.products = this.products.map((product) => {
        if (product.id === result.product.id) {
          // Replace the product with the updated version
          return result.product;
        }
        return product; // Keep the product unchanged
      });

      this.orderProducts();
    }  
  }

  private orderProducts(): void{
    this.products = this.products.sort((a, b) => {      
      const typeA = a.type ?? '';
      const typeB = b.type ?? '';
      const nameA = a.name?.toLowerCase() ?? '';
      const nameB = b.name?.toLowerCase() ?? '';
  
      // Sort by type first
      if (typeA < typeB) return -1;
      if (typeA > typeB) return 1;
  
      // If type is the same, sort by name
      if (nameA < nameB) return -1;
      if (nameA > nameB) return 1;
  
      return 0;
    });
  }

  private updateProducts(result: MultipleProductsResult): void {
    if (result.products) {
      // Create a map of updated products for quick lookup
      const updatedProductsMap = new Map<number, ProductResponseDto>();
      result.products.forEach((product) => {
        updatedProductsMap.set(product.id, product);
      });
  
      // Update the existing products array
      this.products = this.products.map((product) => {
        if (updatedProductsMap.has(product.id)) {
          // Replace the product with the updated version
          return updatedProductsMap.get(product.id)!;
        }
        return product; // Keep the product unchanged
      });
    }
  }
  
  // Book product
  private bookProduct(productId: number): void {
    if (productId !== undefined){
      this.signalrService.bookProduct(productId);
    }
  }

  fetchProducts(): void{
    this.productsService.getProducts().subscribe({
      next: (result: MultipleProductsResult) => {
        console.log(result)
        this.products = result.products;
        this.orderProducts();
      },
      error: (err) => console.error('Error loading products:', err),
    });
  }

  addToBasket(product: ProductResponseDto): void {
    if (product.availableQuantity > 0) {

      // Send message via SignalR to book product
      this.bookProduct(product.id)

      // Check if the product is already in the basket
      const existingProduct = this.basket.find((item) => item.id === product.id);

      if (existingProduct) {
        // Increment the quantity if the product is already in the basket
        existingProduct.quantity++;
      } else {
        // Add the product to the basket with a quantity of 1
        this.basket.push({ id: product.id, quantity: 1 });
      }

      // Update the total payment
      this.totalPayment += product.price // Assuming each product has a price
      product.availableQuantity--; // Decrease available quantity
    }
  }

  resetBasket(): void {
    if (!this.basketIsEmpty){
      console.log("RESET BASKET CALLED")
      if (confirm('Are you sure you want to reset the basket?')) {
        this.productsService.releaseProducts(this.basket).subscribe({
          next: () => {
            this.basket = [];
            this.totalPayment = 0;
          },
          error: (err) => {
            console.error('Release failed:', err);
            alert('Release failed. Please try again.');
          },
        });
      }
    }    
  }
  resetBasketNoConfirmation(): void {
    if (!this.basketIsEmpty){
      console.log("RESET BASKET CALLED")
        this.productsService.releaseProducts(this.basket).subscribe({
          next: () => {
            this.basket = [];
            this.totalPayment = 0;
          },
          error: (err) => {
            console.error('Release failed:', err);
            alert('Release failed. Please try again.');
          },
        });
    }    
  }

  checkout(): void {
    if (!this.basketIsEmpty){

        const checkoutProducts = this.basket.map(basketItem => {
          const product = this.products.find(p => p.id === basketItem.id);
          return {
            id: basketItem.id,
            quantity: basketItem.quantity,
            name: product!.name,
            price: product!.price * basketItem.quantity
          };
        });

        // Open the checkout modal
        const dialogRef = this.dialog.open(CheckoutModalComponent, {
          width: '300px',
          data: { totalPayment: this.totalPayment, checkoutProducts: checkoutProducts },
        });

        dialogRef.afterClosed().subscribe((result) => {
          if (result) {
            // Handle the confirmed payment
            console.log('Amount Paid:', result.amountPaid);
            console.log('Change to Return:', result.change);

            // Call finalizeProducts with the basket
            this.productsService.finalizeProducts(this.basket).subscribe({
              next: () => {
                alert('Checkout successful!');
                this.basket = [];
                this.totalPayment = 0;
              },
              error: (err) => {
                console.error('Checkout failed:', err);
                alert('Checkout failed. Please try again.');
              },
            });
          }
        });
    }    
  }

  get basketIsEmpty(): boolean {
    return this.basket.length < 1;
  }

  getItemSelectedQuantity(id: number): number {
    const existingProduct = this.basket.find((item) => item.id === id);

    if (existingProduct) {
      return existingProduct.quantity
    } else {
      return 0;
    }
  }
}