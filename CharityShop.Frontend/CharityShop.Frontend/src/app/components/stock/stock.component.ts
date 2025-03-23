import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../services/products/products.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductResponseDto } from '../../interfaces/IProductResponseDto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stock',
  imports: [FormsModule, CommonModule],
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css'],
})
export class StockComponent implements OnInit {
  products: (ProductResponseDto & { quantityToAdd: number })[] = [];

  constructor(private productsService: ProductsService, private router: Router) {}

  ngOnInit(): void {
    this.fetchProducts();
  }

  fetchProducts(): void {
    this.productsService.getProducts().subscribe({
      next: (result) => {
        this.products = result.products?.filter(p => p.type == "Items").map((product) => ({
          ...product,
          quantityToAdd: product.totalQuantity, // Initialize quantityToAdd for each product
        })) || [];

        this.orderProducts();
      },
      error: (err) => {
        console.error('Failed to fetch products:', err);
      },
    });
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
          this.router.navigate(['/shop']); // Navigate to the /shop route
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