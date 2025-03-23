import { HealthStatus } from "./IHealthResult";
import { ProductResponseDto } from "./IProductResponseDto";

export interface SingleProductResult {
  product: ProductResponseDto;
  healthStatus: HealthStatus;
}