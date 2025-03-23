export interface ProductResponseDto { //TODO: move interfaces to their directory
    id: number;
    name: string | null;
    price: number;
    type: string | null;
    totalQuantity: number;
    bookedQuantity: number;
    availableQuantity: number;
  }