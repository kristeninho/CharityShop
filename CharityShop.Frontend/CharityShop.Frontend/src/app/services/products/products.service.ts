import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { MultipleProductsResult } from '../../interfaces/IMultipleProductsResult';
import { ProductRequestDto } from '../../interfaces/IProductRequestDto';


@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private baseUrl = '/api/products'; // Base URL for the Products API

  constructor(private http: HttpClient) {}

  /**
   * Fetch all products.
   * @returns An observable of `MultipleProductsResult`.
   */
  getProducts(): Observable<MultipleProductsResult> {
    return this.http.get<MultipleProductsResult>(`${this.baseUrl}`);
  }

  /**
   * Release products.
   * @param products An array of `ProductRequestDto` objects.
   * @returns An observable of string of the response.
   */
  releaseProducts(products: ProductRequestDto[]): Observable<string> {
    return this.http.post(`${this.baseUrl}/release`, products, {
      responseType: 'text', // Expect a text response
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('API Error:', error);
        return throwError(() => new Error('Failed to release products. Please try again.'));
      })
    );
  }

  /**
   * Finalize products.
   * @param products An array of `ProductRequestDto` objects.
   * @returns An observable of string of the response.
   */
  finalizeProducts(products: ProductRequestDto[]): Observable<string> {
    return this.http.post(`${this.baseUrl}/finalize`, products, {
      responseType: 'text', // Expect a text response
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('API Error:', error);
        return throwError(() => new Error('Failed to finalize products. Please try again.'));
      })
    );
  }

  /**
   * Initialize products.
   * @param products An array of `ProductRequestDto` objects.
   * @returns An observable of string of the response.
   */
  initializeProducts(products: ProductRequestDto[]): Observable<string> {
    return this.http.post(`${this.baseUrl}/initialize`, products, {
      responseType: 'text', // Expect a text response
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('API Error:', error);
        return throwError(() => new Error('Failed to initialize products. Please try again.'));
      })
    );
  }


}
