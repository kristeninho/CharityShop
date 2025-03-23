import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../services/products/products.service';
import { ProductRequestDto } from '../../interfaces/IProductRequestDto';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductResponseDto } from '../../interfaces/IProductResponseDto';

@Component({
  selector: 'app-stock',
  imports: [FormsModule, CommonModule],
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css'],
})
export class StockComponent implements OnInit {
  products: (ProductResponseDto & { quantityToAdd: number })[] = [];

  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {
    this.fetchProducts();
  }

  fetchProducts(): void {
    this.productsService.getProducts().subscribe({
      next: (result) => {
        this.products = result.products?.map((product) => ({
          ...product,
          quantityToAdd: product.totalQuantity, // Initialize quantityToAdd for each product
        })) || [];
      },
      error: (err) => {
        console.error('Failed to fetch products:', err);
      },
    });
  }

  initializeStock(): void {
    const productsToInitialize = this.products
      .filter((product) => product.quantityToAdd > 0 && product.type === "Items")
      .map((product) => ({
        id: product.id,
        quantity: product.quantityToAdd,
      }));

    if (productsToInitialize.length > 0) {
      this.productsService.initializeProducts(productsToInitialize).subscribe({
        next: (response) => {
          console.log(response)
          alert('Stock initialized successfully!');
          this.fetchProducts(); // Refresh the product list
        },
        error: (err) => {
          console.error('Failed to initialize stock:', err);
          alert('Failed to initialize stock. Please try again.');
        },
      });
    } else {
      alert('Please enter quantities for at least one product.');
    }
  }
}