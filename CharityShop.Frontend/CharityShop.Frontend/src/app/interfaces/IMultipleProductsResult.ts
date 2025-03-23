import { HealthStatus } from "./IHealthResult";
import { ProductResponseDto } from "./IProductResponseDto";

export interface MultipleProductsResult {
  products: ProductResponseDto[];
  healthStatus: HealthStatus;
}